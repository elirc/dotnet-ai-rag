# Annotation Drills

## Drill 1: AuthService Login

Annotate [AuthService.LoginAsync](../../../src/Infrastructure/Auth/AuthService.cs#L12-L24).

| Mark | Question |
| --- | --- |
| Input | What can the caller control? |
| Dependency | Which dependencies are injected? |
| Invariant | What must be true before token creation? |
| Side effect | Does this write anything? |
| Failure mode | What returns null? |

Self-grade: Basic = labels lines; Solid = explains inactive users; Strong = notices rate-limit/audit gaps.

## Drill 2: JWT Creation

Annotate [JwtTokenService.CreateToken](../../../src/Infrastructure/Auth/JwtTokenService.cs#L14-L35). Identify every claim and explain who consumes it.

Self-grade: Basic = names claims; Solid = explains signing; Strong = discusses revocation and expiry tradeoffs.

## Drill 3: React Login Mutation

Annotate [LoginPage](../../../frontend/src/pages/LoginPage.tsx#L21-L35). Track form values, mutation success, session write, and navigation.

Self-grade: Basic = follows success path; Solid = explains error rendering; Strong = identifies where stale `from` path could matter.

## Drill 4: EF Mapping

Annotate [NetAiOpsDbContext](../../../src/Infrastructure/Persistence/NetAiOpsDbContext.cs#L12-L30). Mark schema contract, index, conversion, and required fields.

Self-grade: Basic = maps C# to columns; Solid = explains unique index; Strong = raises case-insensitive email question.

## Drill 5: Protected Route

Annotate [ProtectedRoute](../../../frontend/src/components/ProtectedRoute.tsx#L9-L18). Label what it protects and what it does not protect.

Self-grade: Basic = redirect; Solid = client vs server auth; Strong = threat-models fake localStorage token.

