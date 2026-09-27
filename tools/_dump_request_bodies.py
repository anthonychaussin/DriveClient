import json
from pathlib import Path

j = json.loads(Path(r"c:\Users\chaus\source\repos\DriveClient\DriveClient\infomaniak_api_kdrive.json").read_text(encoding="utf-8"))

targets = [
    ("/2/drive/{drive_id}/files/{file_id}/access", "post"),
    ("/2/drive/{drive_id}/files/{file_id}/access/users", "post"),
    ("/2/drive/{drive_id}/files/{file_id}/access/users/{user_id}", "put"),
    ("/2/drive/{drive_id}/files/{file_id}/access/teams", "post"),
    ("/2/drive/{drive_id}/files/{file_id}/access/teams/{team_id}", "put"),
    ("/2/drive/{drive_id}/files/{file_id}/access/invitations", "post"),
    ("/2/drive/{drive_id}/files/{file_id}/access/invitations/check", "post"),
    ("/2/drive/{drive_id}/files/{file_id}/access/request", "post"),
    ("/2/drive/{drive_id}/files/{file_id}/access/applications", "post"),
    ("/2/drive/{drive_id}/files/{file_id}/access/check", "post"),
    ("/2/drive/{drive_id}/files/{file_id}/access/force", "post"),
    ("/2/drive/{drive_id}/users", "post"),
    ("/2/drive/{drive_id}/users/{user_id}", "put"),
    ("/2/drive/{drive_id}/users/{user_id}/manager", "patch"),
    ("/2/drive/{drive_id}/settings/ai", "put"),
    ("/2/drive/{drive_id}/settings/link", "put"),
    ("/2/drive/{drive_id}/settings/office", "put"),
    ("/2/drive/{drive_id}/settings/trash", "put"),
    ("/2/drive/{drive_id}/preferences", "put"),
    ("/2/drive/preferences", "patch"),
    ("/2/drive/{drive_id}/imports/oauth", "post"),
    ("/2/drive/{drive_id}/imports/kdrive", "post"),
    ("/2/drive/{drive_id}/imports/webdav", "post"),
    ("/2/drive/{drive_id}/imports/sharelink", "post"),
    ("/2/drive/{drive_id}/files/categories/{category_id}", "post"),
    ("/2/drive/{drive_id}/categories/rights", "post"),
    ("/2/drive/{drive_id}/activities/reports", "post"),
    ("/2/drive/{drive_id}/files/{file_id}/dropbox", "post"),
    ("/2/drive/{drive_id}/files/{file_id}/dropbox", "put"),
    ("/2/drive/{drive_id}/files/{file_id}/dropbox/invite", "post"),
    ("/2/drive/{drive_id}", "put"),
    ("/2/drive/{drive_id}/users/invitation/{invitation_id}", "put"),
    ("/2/drive/{drive_id}/upload/cancel", "post"),
    ("/3/drive/{drive_id}/upload/session/batch", "post"),
    ("/3/drive/{drive_id}/upload/session/batch/finish", "post"),
    ("/2/drive/{drive_id}/files/{file_id}/versions/current", "post"),
    ("/2/drive/{drive_id}/files/{file_id}/categories/{category_id}/ai-feedback", "post"),
]

out = Path(r"c:\Users\chaus\source\repos\DriveClient\tools\_request_bodies.json")
data = {}
for path, method in targets:
    op = j["paths"][path][method]
    rb = op.get("requestBody", {})
    content = rb.get("content", {})
    schema = None
    for v in content.values():
        schema = v.get("schema")
        break
    data[f"{method.upper()} {path}"] = schema
out.write_text(json.dumps(data, indent=2), encoding="utf-8")
print("wrote", out, "keys", len(data))
