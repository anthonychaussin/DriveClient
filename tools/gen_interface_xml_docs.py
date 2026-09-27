#!/usr/bin/env python3
"""Generate English XML documentation stubs for IKDrive* interface members lacking docs."""
from __future__ import annotations

import re
from pathlib import Path

ROOT = Path(__file__).resolve().parents[1] / "DriveClient" / "kDriveClient" / "Interface"

METHOD_RE = re.compile(
    r"(?P<indent>^[ \t]*)(?P<sig>Task(?:<[^;]+>)?\s+(?P<name>\w+)\s*\((?P<params>[^)]*)\)\s*;)",
    re.MULTILINE,
)
PROP_RE = re.compile(
    r"(?P<indent>^[ \t]*)(?P<sig>(?:int|bool|string|IProgress<[^>]+>\?)\s+(?P<name>\w+)\s*\{\s*get;\s*(?:set;\s*)?\})",
    re.MULTILINE,
)

SUMMARY_BY_PREFIX = {
    "Get": "Gets",
    "List": "Lists",
    "Search": "Searches",
    "Create": "Creates",
    "Update": "Updates",
    "Delete": "Deletes",
    "Remove": "Removes",
    "Add": "Adds",
    "Move": "Moves",
    "Copy": "Copies",
    "Rename": "Renames",
    "Trash": "Moves to trash",
    "Restore": "Restores",
    "Count": "Counts",
    "Find": "Finds",
    "Ensure": "Ensures",
    "Invite": "Invites",
    "Share": "Shares",
    "Tag": "Tags",
    "Favorite": "Marks as favorite",
    "Unlock": "Unlocks",
    "Convert": "Converts",
    "Duplicate": "Duplicates",
    "Cancel": "Cancels",
    "Clear": "Clears",
    "Empty": "Empties",
    "Permanently": "Permanently deletes",
    "Force": "Forces",
    "Check": "Checks",
    "Send": "Sends",
    "Decline": "Declines",
    "Enable": "Enables",
    "Disable": "Disables",
    "Finish": "Finishes",
    "Build": "Builds",
}


def humanize(name: str) -> str:
    # Strip Async suffix
    if name.endswith("Async"):
        name = name[:-5]
    # Insert spaces before capitals
    spaced = re.sub(r"(?<!^)([A-Z])", r" \1", name).strip()
    return spaced


def build_summary(name: str) -> str:
    human = humanize(name)
    for prefix, verb in SUMMARY_BY_PREFIX.items():
        if name.startswith(prefix):
            rest = humanize(name[len(prefix) :])
            if rest.lower().endswith(" async"):
                rest = rest[: -len(" async")]
            return f"{verb} {rest[0].lower() + rest[1:] if rest else 'resource'}."
    return f"Performs {human[0].lower() + human[1:]}."


def param_docs(params: str) -> str:
    if not params.strip():
        return ""
    lines = []
    for part in params.split(","):
        part = part.strip()
        if not part:
            continue
        # type name = default
        m = re.search(r"(\w+)\s*=", part) or re.search(r"(\w+)$", part.split("=")[0].strip().split()[-1])
        # better: last token before = is name
        left = part.split("=")[0].strip()
        tokens = left.replace("?", "").split()
        if not tokens:
            continue
        pname = tokens[-1]
        if pname == "ct" or pname == "cancellationToken":
            lines.append(f'        /// <param name="{pname}">Cancellation token.</param>')
        elif pname == "query":
            lines.append(
                f'        /// <param name="{pname}">Optional typed query filters (limit, cursor, includes, …).</param>'
            )
        elif pname in ("fileId", "itemId"):
            lines.append(
                f'        /// <param name="{pname}">Remote file or directory id (API uses file_id for both).</param>'
            )
        elif pname in ("directoryId", "parentDirectoryId", "destinationDirectoryId", "trashedDirectoryId"):
            lines.append(f'        /// <param name="{pname}">Directory id.</param>')
        elif pname == "with":
            lines.append(
                f'        /// <param name="{pname}">Optional comma-separated includes (prefer typed query Includes when available).</param>'
            )
        elif pname == "payload" or pname == "request":
            lines.append(f'        /// <param name="{pname}">Typed request body.</param>')
        else:
            lines.append(f'        /// <param name="{pname}">The {humanize(pname).lower()}.</param>')
    return "\n".join(lines)


def document_file(path: Path) -> bool:
    text = path.read_text(encoding="utf-8")
    original = text

    def repl_method(m: re.Match[str]) -> str:
        start = m.start()
        before = text[:start]
        # Already documented?
        if re.search(r"///\s*<summary>\s*$", before.rstrip()[-80:], re.MULTILINE) or "/// <summary>" in before[-200:]:
            # check last non-empty lines
            tail = before.rstrip().splitlines()[-3:]
            if any("///" in line for line in tail):
                return m.group(0)

        indent = m.group("indent")
        name = m.group("name")
        params = m.group("params")
        summary = build_summary(name)
        docs = [f"{indent}/// <summary>", f"{indent}/// {summary}", f"{indent}/// </summary>"]
        pd = param_docs(params)
        if pd:
            # fix indent in param_docs
            pd_lines = []
            for line in pd.splitlines():
                pd_lines.append(indent + line.lstrip())
            docs.extend(pd_lines)
        docs.append(f"{indent}/// <returns>A task that represents the asynchronous API call.</returns>")
        docs.append(f"{indent}/// <exception cref=\"kDriveClient.Models.Exceptions.KDriveApiException\">Thrown when the API returns an error payload.</exception>")
        return "\n".join(docs) + "\n" + m.group(0)

    # Process methods from bottom to top to keep indices stable — use finditer list reversed
    matches = list(METHOD_RE.finditer(text))
    for m in reversed(matches):
        start = m.start()
        before = text[:start]
        tail_lines = [ln for ln in before.rstrip().splitlines()[-5:] if ln.strip()]
        if tail_lines and any(ln.strip().startswith("///") for ln in tail_lines[-3:]):
            continue
        indent = m.group("indent")
        name = m.group("name")
        params = m.group("params")
        summary = build_summary(name)
        docs = [
            f"{indent}/// <summary>",
            f"{indent}/// {summary}.",
            f"{indent}/// </summary>",
        ]
        # Avoid double period
        docs[1] = f"{indent}/// {summary}" if summary.endswith(".") else f"{indent}/// {summary}."
        for part in params.split(","):
            part = part.strip()
            if not part:
                continue
            left = part.split("=")[0].strip()
            tokens = left.replace("?", "").split()
            if not tokens:
                continue
            pname = tokens[-1]
            if pname in ("ct", "cancellationToken"):
                docs.append(f'{indent}/// <param name="{pname}">Cancellation token.</param>')
            elif pname == "query":
                docs.append(
                    f'{indent}/// <param name="{pname}">Optional typed query filters (limit, cursor, includes, …).</param>'
                )
            elif pname in ("fileId", "itemId"):
                docs.append(
                    f'{indent}/// <param name="{pname}">Remote file or directory id (API uses file_id for both).</param>'
                )
            elif "Directory" in pname or pname.endswith("directoryId"):
                docs.append(f'{indent}/// <param name="{pname}">Directory id.</param>')
            elif pname == "with":
                docs.append(
                    f'{indent}/// <param name="{pname}">Optional includes string; prefer <see cref="kDriveClient.Models.Domain.KDriveItemIncludes"/> on typed queries.</param>'
                )
            elif pname in ("payload", "request"):
                docs.append(f'{indent}/// <param name="{pname}">Typed request body.</param>')
            elif pname == "pageSize":
                docs.append(f'{indent}/// <param name="{pname}">Number of items requested per page.</param>')
            elif pname == "maxItems":
                docs.append(f'{indent}/// <param name="{pname}">Maximum number of items to aggregate.</param>')
            elif pname == "minInterval":
                docs.append(f'{indent}/// <param name="{pname}">Minimum interval between wake calls.</param>')
            else:
                docs.append(f'{indent}/// <param name="{pname}">The {humanize(pname).lower()}.</param>')
        docs.append(f"{indent}/// <returns>A task representing the asynchronous operation.</returns>")
        docs.append(
            f'{indent}/// <exception cref="kDriveClient.Models.Exceptions.KDriveApiException">Thrown when the API returns an error payload.</exception>'
        )
        insertion = "\n".join(docs) + "\n"
        text = text[:start] + insertion + text[start:]

    if text != original:
        path.write_text(text, encoding="utf-8", newline="\n")
        return True
    return False


def main() -> None:
    changed = 0
    for path in sorted(ROOT.glob("*.cs")):
        if document_file(path):
            print("documented", path.name)
            changed += 1
    print(f"Updated {changed} files")


if __name__ == "__main__":
    main()
