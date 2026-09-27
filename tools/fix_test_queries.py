#!/usr/bin/env python3
from pathlib import Path
import re

root = Path(__file__).resolve().parents[1] / "DriveClientTests"
pattern = re.compile(r"new Dictionary<string,\s*string\?>\s*\{([^}]*)\}")

def repl_dict(m: re.Match[str]) -> str:
    body = m.group(1)
    typed: list[str] = []
    extras: dict[str, str] = {}
    for km in re.finditer(r'\["([^"]+)"\]\s*=\s*"([^"]*)"', body):
        key, val = km.group(1), km.group(2)
        if key == "limit" and val.isdigit():
            typed.append(f"Limit = {val}")
        elif key == "with":
            typed.append(f'With = "{val}"')
        else:
            extras[key] = val
    parts = list(typed)
    if extras:
        extra_items = ", ".join(f'["{k}"] = "{v}"' for k, v in extras.items())
        parts.append(f"Extra = {{ {extra_items} }}")
    return "new KDriveListQuery { " + ", ".join(parts) + " }"

for path in root.rglob("*.cs"):
    text = path.read_text(encoding="utf-8")
    text2 = pattern.sub(repl_dict, text)
    if text2 != text:
        path.write_text(text2, encoding="utf-8", newline="\n")
        print("updated", path.relative_to(root.parent))
