# Maintainer Communication

## Ask Without Outsourcing Thinking

Weak: "Auth is broken, what do I do?"

Strong: "Login returns 401. I confirmed the request body from [api.login](../../../frontend/src/app/api.ts#L29-L33), hit [AuthController.Login](../../../src/Api/Controllers/AuthController.cs#L13-L17), and suspect the seed user is absent because Docker was not running. Is the expected local flow to run migrations on startup?"

## Repro Report Template

- Environment
- Command/page
- Expected
- Actual
- What I checked
- Logs/output
- Suspected layer

## Respectful Disagreement

When challenging a design, state the tradeoff. Example: "localStorage is simpler for the lab, but once incident-close actions exist, I think httpOnly cookies deserve an RFC because token theft blast radius increases."

