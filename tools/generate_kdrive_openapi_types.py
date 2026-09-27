#!/usr/bin/env python3
"""Generate C# DTOs from Infomaniak OpenAPI schemas prefixed 91ac10ff_."""

from __future__ import annotations

import json
import re
from pathlib import Path

ROOT = Path(__file__).resolve().parents[1]
SPEC = ROOT / "DriveClient" / "infomaniak_api_kdrive.json"
OUT_DIR = ROOT / "DriveClient" / "Models" / "Generated"
CONTEXT_OUT = ROOT / "DriveClient" / "Helpers" / "KDriveOpenApiJsonContext.Generated.cs"

PREFIX = "91ac10ff_"
COMMON = ("ErrorResponse", "Pagination", "Response")
REF_KEY = "$ref"
NS = "kDriveClient.Models.Generated"


PROPERTY_PASCAL_OVERRIDES = {
    "sharelink": "ShareLink",
    "dropbox": "Dropbox",
    "etag": "Etag",
    "uuid": "Uuid",
    "url": "Url",
}


def pascal(name: str) -> str:
    if name in PROPERTY_PASCAL_OVERRIDES:
        return PROPERTY_PASCAL_OVERRIDES[name]
    parts = re.split(r"[^A-Za-z0-9]+", name)
    return "".join(p[:1].upper() + p[1:] for p in parts if p)


def csharp_type_name(schema_key: str) -> str:
    if schema_key.startswith(PREFIX):
        return "KDriveApi" + pascal(schema_key[len(PREFIX) :])
    return "KDriveApi" + pascal(schema_key)


def ref_name(ref: str) -> str | None:
    marker = "#/components/schemas/"
    if not ref.startswith(marker):
        return None
    return ref[len(marker) :]


def xml_escape(text: str) -> str:
    return (
        text.replace("&", "&amp;")
        .replace("<", "&lt;")
        .replace(">", "&gt;")
        .replace('"', "&quot;")
    )


def clean_doc(text: str | None, limit: int = 280) -> str | None:
    if not text:
        return None
    text = re.sub(r"<[^>]+>", " ", text)
    text = re.sub(r"\s+", " ", text).strip()
    if not text:
        return None
    if len(text) > limit:
        text = text[: limit - 1] + "…"
    return text


def collect_schemas(all_schemas: dict) -> dict[str, dict]:
    selected: dict[str, dict] = {}
    for key, schema in all_schemas.items():
        if key.startswith(PREFIX) or key in COMMON:
            selected[key] = schema

    # Ensure internal refs within selected set stay included (already filtered).
    return selected


def resolve_prop_type(
    prop: dict,
    *,
    parent_csharp: str,
    prop_name: str,
    inline_defs: list[tuple[str, dict, str | None]],
    known: set[str],
    reserved_csharp: set[str],
) -> str:
    if REF_KEY in prop:
        target = ref_name(prop[REF_KEY])
        if target and target in known:
            return csharp_type_name(target) + "?"
        return "JsonElement?"

    if "allOf" in prop:
        for part in prop["allOf"]:
            if REF_KEY in part:
                target = ref_name(part[REF_KEY])
                if target and target in known:
                    return csharp_type_name(target) + "?"
        merged_props: dict = {}
        desc = prop.get("description")
        for part in prop["allOf"]:
            if isinstance(part, dict) and part.get("type") == "object":
                merged_props.update(part.get("properties") or {})
                desc = desc or part.get("description")
        if merged_props:
            nested = unique_inline_name(parent_csharp, prop_name, reserved_csharp)
            inline_defs.append((nested, {"type": "object", "properties": merged_props, "description": desc}, desc))
            return nested + "?"
        return "JsonElement?"

    t = prop.get("type")
    if t == "array":
        items = prop.get("items") or {}
        inner = resolve_prop_type(
            items,
            parent_csharp=parent_csharp,
            prop_name=prop_name + "Item",
            inline_defs=inline_defs,
            known=known,
            reserved_csharp=reserved_csharp,
        ).rstrip("?")
        return f"List<{inner}>?"

    if t == "object" or ("properties" in prop and t is None):
        props = prop.get("properties") or {}
        if not props:
            return "JsonElement?"
        nested = unique_inline_name(parent_csharp, prop_name, reserved_csharp)
        inline_defs.append((nested, prop, prop.get("description") or prop.get("title")))
        return nested + "?"

    if t == "integer":
        return "long?"
    if t == "number":
        return "double?"
    if t == "boolean":
        return "bool?"
    if t == "string":
        return "string?"
    return "JsonElement?"


def unique_inline_name(parent_csharp: str, prop_name: str, reserved_csharp: set[str]) -> str:
    base = parent_csharp + pascal(prop_name)
    if base not in reserved_csharp:
        reserved_csharp.add(base)
        return base
    candidate = base + "Inline"
    n = 2
    while candidate in reserved_csharp:
        candidate = f"{base}Inline{n}"
        n += 1
    reserved_csharp.add(candidate)
    return candidate


def emit_class(
    csharp_name: str,
    schema: dict,
    known: set[str],
    reserved_csharp: set[str],
    *,
    is_inline: bool = False,
) -> tuple[str, list[str]]:
    """Return (source, referenced_type_names for JsonSerializable)."""
    inline_defs: list[tuple[str, dict, str | None]] = []
    props = schema.get("properties") or {}
    title = schema.get("title") or csharp_name
    desc = clean_doc(schema.get("description") or title)

    lines: list[str] = []
    if desc:
        lines.append("    /// <summary>")
        lines.append(f"    /// {xml_escape(desc)}")
        lines.append("    /// </summary>")
    lines.append("    /// <remarks>Generated from OpenAPI kDrive schema (91ac10ff_* / common envelopes).</remarks>")
    lines.append(f"    public partial class {csharp_name}")
    lines.append("    {")

    type_names = [csharp_name]
    seen_props: set[str] = set()

    for prop_name, prop in props.items():
        prop_pascal = pascal(prop_name)
        if prop_pascal == csharp_name:
            prop_pascal = prop_pascal + "Value"
        if prop_pascal in seen_props:
            continue
        seen_props.add(prop_pascal)

        cs_type = resolve_prop_type(
            prop,
            parent_csharp=csharp_name,
            prop_name=prop_name,
            inline_defs=inline_defs,
            known=known,
            reserved_csharp=reserved_csharp,
        )

        pdesc = clean_doc(prop.get("description") or prop.get("title"))
        if pdesc:
            lines.append("        /// <summary>")
            lines.append(f"        /// {xml_escape(pdesc)}")
            lines.append("        /// </summary>")
        lines.append(f'        [JsonPropertyName("{prop_name}")]')
        lines.append(f"        public {cs_type} {prop_pascal} {{ get; set; }}")
        lines.append("")

    deduped_inline: list[tuple[str, dict, str | None]] = []
    seen_inline: set[str] = set()
    for nested_name, nested_schema, nested_desc in inline_defs:
        if nested_name in seen_inline:
            continue
        seen_inline.add(nested_name)
        deduped_inline.append((nested_name, nested_schema, nested_desc))

    if not is_inline:
        lines.append("        /// <summary>Undocumented / include-only fields.</summary>")
        lines.append("        [JsonExtensionData]")
        lines.append("        public Dictionary<string, JsonElement>? ExtraData { get; set; }")
        lines.append("")

    lines.append("    }")
    lines.append("")

    for nested_name, nested_schema, nested_desc in deduped_inline:
        nested_src, nested_types = emit_class(
            nested_name, nested_schema, known, reserved_csharp, is_inline=True
        )
        lines.append(nested_src)
        type_names.extend(nested_types)

    return "\n".join(lines), type_names


def merge_file_system_item(file_v3: dict, dir_v3: dict) -> dict:
    props: dict = {}
    props.update(file_v3.get("properties") or {})
    props.update(dir_v3.get("properties") or {})
    return {
        "title": "File or Directory V3 (union)",
        "description": (
            "Union of OpenAPI FileV3 and DirectoryV3 for endpoints that return either "
            "type (discriminator: type = file|dir)."
        ),
        "type": "object",
        "properties": props,
    }


def main() -> None:
    with SPEC.open(encoding="utf-8") as f:
        spec = json.load(f)

    all_schemas = spec["components"]["schemas"]
    selected = collect_schemas(all_schemas)
    known = set(selected.keys())

    OUT_DIR.mkdir(parents=True, exist_ok=True)
    for old in OUT_DIR.glob("*.cs"):
        old.unlink()

    all_type_names: list[str] = []
    files_written = 0
    reserved_csharp = {csharp_type_name(k) for k in selected.keys()}
    reserved_csharp.add("KDriveApiFileSystemItem")

    # Regular schemas
    for schema_key in sorted(selected.keys()):
        csharp = csharp_type_name(schema_key)
        src_body, type_names = emit_class(csharp, selected[schema_key], known, reserved_csharp)
        content = "\n".join(
            [
                "// <auto-generated/>",
                "#nullable enable",
                "using System.Text.Json;",
                "using System.Text.Json.Serialization;",
                "",
                f"namespace {NS};",
                "",
                src_body.rstrip(),
                "",
            ]
        )
        (OUT_DIR / f"{csharp}.cs").write_text(content, encoding="utf-8", newline="\n")
        all_type_names.extend(type_names)
        files_written += 1

    # Unified file-system item for mixed file/dir payloads
    file_key = PREFIX + "FileV3"
    dir_key = PREFIX + "DirectoryV3"
    if file_key in selected and dir_key in selected:
        union = merge_file_system_item(selected[file_key], selected[dir_key])
        csharp = "KDriveApiFileSystemItem"
        src_body, type_names = emit_class(csharp, union, known, reserved_csharp)
        content = "\n".join(
            [
                "// <auto-generated/>",
                "#nullable enable",
                "using System.Text.Json;",
                "using System.Text.Json.Serialization;",
                "",
                f"namespace {NS};",
                "",
                src_body.rstrip(),
                "",
            ]
        )
        (OUT_DIR / f"{csharp}.cs").write_text(content, encoding="utf-8", newline="\n")
        all_type_names.extend(type_names)
        files_written += 1

    # Deduplicate while preserving order
    seen: set[str] = set()
    unique_types: list[str] = []
    for t in all_type_names:
        if t not in seen:
            seen.add(t)
            unique_types.append(t)

    # Only root schema types need explicit registration; nested property types are discovered.
    root_types = sorted({csharp_type_name(k) for k in selected.keys()} | {"KDriveApiFileSystemItem"})

    # Separate context avoids CS8785 Boolean hintName collisions in the large main context.
    attrs = [f"    [JsonSerializable(typeof({t}))]" for t in root_types]

    context = "\n".join(
        [
            "// <auto-generated/>",
            "#nullable enable",
            "using System.Text.Json.Serialization;",
            "using kDriveClient.Models.Generated;",
            "",
            "namespace kDriveClient.Helpers;",
            "",
            "/// <summary>",
            "/// Source-generated JSON context for OpenAPI kDrive DTOs (<c>91ac10ff_*</c>).",
            "/// Kept separate from <see cref=\"KDriveJsonContext\"/> to avoid generator hint-name collisions.",
            "/// </summary>",
            *attrs,
            "[JsonSourceGenerationOptions(PropertyNameCaseInsensitive = true)]",
            "public partial class KDriveOpenApiJsonContext : JsonSerializerContext",
            "{",
            "}",
            "",
        ]
    )
    CONTEXT_OUT.write_text(context, encoding="utf-8", newline="\n")

    print(f"Generated {files_written} schema files, {len(unique_types)} types → {OUT_DIR}")
    print(f"Updated {CONTEXT_OUT.name} with {len(root_types)} JsonSerializable roots")


if __name__ == "__main__":
    main()
