# Fake Code Contrasts

## Contrast 1: Missing Server Authorization

Resembles: [DashboardController](../../../src/Api/Controllers/DashboardController.cs#L7-L17).

```csharp
// Illustrative: not from this repo
[HttpGet("admin/settings")]
public IActionResult Settings() => Ok(settings);
```

Better:

```csharp
// Illustrative: not from this repo
[Authorize(Policy = "AdminOnly")]
[HttpGet("admin/settings")]
public IActionResult Settings() => Ok(settings);
```

Lesson: client route guards are not permission checks.

## Contrast 2: Hand-Rolled Fetch Race

Resembles: [DashboardPage](../../../frontend/src/pages/DashboardPage.tsx#L5-L18).

```tsx
// Illustrative: not from this repo
const [data, setData] = useState(null);
getDashboardSummary(token).then(setData);
```

Better: use React Query as in [DashboardPage](../../../frontend/src/pages/DashboardPage.tsx#L5-L18).

Lesson: side effects in render cause repeated requests.

## Contrast 3: Entity As Public Contract

Resembles separation between [AppUser](../../../src/Domain/Users/AppUser.cs#L3-L13) and [DashboardSummaryDto](../../../src/Application/Dashboard/DashboardSummaryDto.cs#L3-L18).

```csharp
// Illustrative: not from this repo
public Task<AppUser> Me() => currentUser;
```

Better: return a DTO without `PasswordHash`.

Lesson: public contracts should not leak internal fields.

## Contrast 4: Ignored Cancellation

Resembles: [AuthService](../../../src/Infrastructure/Auth/AuthService.cs#L12-L24).

```csharp
// Illustrative: not from this repo
await db.Users.SingleOrDefaultAsync(x => x.Email == email);
```

Better: pass `ct` as the repo does.

Lesson: cancellation is part of request ownership.

