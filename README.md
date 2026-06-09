# Sentinel Integration Test Repository — Azure DevOps

**⚠️ This repository is intentionally buggy for testing.**

This repository contains deliberately flawed C# code with logic errors for testing Sentinel's **Azure DevOps work item scanner integration**. It is NOT a real application and should NEVER be used in production.

## Purpose

- Test positive cases: Real bugs/defects that Azure DevOps tracks and Sentinel should fix
- Test negative cases: Already-fixed bugs or closed work items
- Validate LLM reasoning: Verify Sentinel generates correct code fixes
- Integration testing: Verify end-to-end workflow (detect → plan → code → git → PR)

## Intentionally Buggy Code

| File | Bug | Type | Test Purpose |
|---|---|---|---|
| `BuggyCalculator.cs` | Off-by-one in sum_to_n() | LOGIC | POSITIVE: Detects real defect |
| `BuggyCalculator.cs` | Division by zero in safe_divide() | CRASH | POSITIVE: Detects crash risk |
| `FixedCalculator.cs` | Correct implementation | — | NEGATIVE: Shows remediation |

## Test Scenarios

### Scenario 1: Positive Case (Real Bugs)
```bash
# ADO work item: "sum_to_n(5) returns 10, expected 15"
# BuggyCalculator.cs has: range(1, n) instead of range(1, n+1)
# Expected: Sentinel detects, generates fix, creates PR
```

### Scenario 2: Negative Case (Already Fixed)
```bash
# ADO work item references same function
# FixedCalculator.cs has: correct range(1, n+1)
# Expected: Sentinel detects, finds issue is closed/fixed
```

## Files

```
├── BuggyCalculator.cs            # Flawed: off-by-one, no division guard
├── FixedCalculator.cs            # Fixed: correct logic, safe operations
├── UnitTests.cs                  # Test cases for both versions
├── TestApp.csproj                # Project file
└── README.md                     # This file
```

## Bugs Explained

### Bug 1: Off-by-One in sum_to_n()
```csharp
// BAD (BUGGY)
public static int sum_to_n(int n)
{
    int sum = 0;
    for (int i = 1; i < n; i++)  // ← BUG: excludes n itself!
        sum += i;
    return sum;
}
// sum_to_n(5) returns 1+2+3+4 = 10 (should be 1+2+3+4+5 = 15)

// GOOD (FIXED)
public static int sum_to_n(int n)
{
    int sum = 0;
    for (int i = 1; i <= n; i++)  // ✓ Includes n
        sum += i;
    return sum;
}
// sum_to_n(5) returns 1+2+3+4+5 = 15 ✓
```

### Bug 2: Division by Zero
```csharp
// BAD (BUGGY)
public static double safe_divide(int a, int b)
{
    return (double)a / b;  // ← BUG: No guard for b==0!
}
// safe_divide(10, 0) throws DivideByZeroException

// GOOD (FIXED)
public static double safe_divide(int a, int b)
{
    if (b == 0)
        throw new ArgumentException("Denominator cannot be zero", nameof(b));
    return (double)a / b;
}
// safe_divide(10, 0) throws clear ArgumentException ✓
```

## How to Use This Repo

**For Sentinel Integration Testing:**

```bash
# Clone or reference this repo in integration tests
# Tests will:
# 1. Read work item from mocks/ado_defect.json (positive case)
# 2. Read work item from mocks/ado_defect_fixed.json (negative case)
# 3. Create branches like fix/ado-defect-4242
# 4. Generate BuggyCalculator.cs → FixedCalculator.cs changes
# 5. Open PR against Azure DevOps work item
```

**For Manual Testing:**

```bash
# Run unit tests - should all fail with current code
dotnet test

# Expected output:
# ✗ sum_to_n_returns_correct_sum — FAILED (got 10, expected 15)
# ✗ safe_divide_handles_zero — FAILED (DivideByZeroException not caught)
```

## Security & Testing Notes

- ✅ This is a **test-only** repository
- ✅ Bugs are intentional for testing purposes
- ✅ No real business logic
- ❌ DO NOT copy this code to production
- ❌ DO NOT use these implementations anywhere

## ADO Integration

This repo works with Azure DevOps via:
1. Work items linked in `ado_defect.json`
2. Sentinel reads work item details from ADO API
3. Tests create branches and link back to work item
4. PRs reference work item (e.g., "Fixes #4242")

## Repository Lifecycle

This repo is created and maintained specifically for Sentinel integration testing. It is:
- ✅ Public (for testing, no sensitive code)
- ✅ Read-only (tests fork it, never push directly)
- ✅ Immutable (bugs stay, intentionally)

See also: [Sentinel Integration Testing Guide](../../docs/integration-testing.md)
