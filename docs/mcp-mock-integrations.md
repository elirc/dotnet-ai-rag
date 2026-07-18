# MCP Mock Integrations

Phase 5 adds an MCP-style integration layer. The first version will not need real MCP transport; it will model external tool capabilities behind a consistent contract.

## Planned Mock Servers

### GitHub

- Search repositories
- Get PR details
- Get recent commits
- Create issue
- Add PR comment

### Jira

- Search tickets
- Create ticket
- Update ticket status
- Add comment

### Slack

- Search messages
- Send incident update
- Get channel history

### Database

- Run readonly SQL query
- Explain query plan mock
- Get deadlock reports

## Logging

Every call should persist server, tool, input, output, status, timing, error message, and optional related agent run.
