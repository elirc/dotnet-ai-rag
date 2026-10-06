# Evaluation Harness

> **Implementation status (verified 2026-10-06):** there are no `eval_cases`/`eval_results` tables. `EvalRuns` exists and is seeded with demo rows for the Evals page. The only executable evaluation is `sample-eval-cases/retrieval.json` (3 cases), loaded by the unit test `Checked_in_evaluation_cases_are_loaded_and_pass` in `tests/UnitTests/LexicalRetrieverTests.cs`. It checks top title / match count, not precision, recall, or MRR.

Phase 6 makes quality measurable. The harness should test retrieval, RAG answers, agent workflows, tool use, and regressions over time.

## Eval Types

- Retrieval eval: precision, recall, MRR, and hit rate at K.
- RAG answer eval: citations, expected facts, unsupported claims, and format.
- Agent workflow eval: expected tools, correct service, relevant evidence, mitigation, and safety.
- Tool use eval: failed calls, duplicate calls, unnecessary calls, and missing required tools.
- Regression eval: compare suites across repeated runs.

## Data Model

- `eval_cases`
- `eval_runs`
- `eval_results`

## Teaching Note

Evals are not just tests. Tests catch broken contracts; evals catch quality drift. The project should include both.
