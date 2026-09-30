# Sample Eval Cases

`retrieval.json` is the first deterministic local evaluation set. Unit tests run
the same BillingService, AuthService, and unrelated-query expectations without
an embedding or model provider.

The first planned cases will cover:

- BillingService timeout retrieval
- AuthService expired signing key investigation
- SearchService latency mitigation
