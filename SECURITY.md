# Security Policy

## Reporting

This is a local training project. In a real organization, report suspected vulnerabilities privately through the security team or repository security advisory process.

## Baseline Checks

- Do not commit production secrets.
- Keep JWT signing keys in environment/secret storage outside source control.
- Require authorization attributes on protected endpoints.
- Use human approvals before agent-triggered risky actions.
- Add tests for auth boundaries and resource ownership.
