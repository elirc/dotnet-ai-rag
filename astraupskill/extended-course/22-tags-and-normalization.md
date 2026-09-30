# 22. A comma-separated field can lose structure

JoinTags trims input strings, removes whitespace-only values, deduplicates case-insensitively and joins with commas. SplitTags splits on commas and trims entries. This compact representation is easy to inspect, but it assumes commas are separators rather than valid characters inside a tag. The retrieval tokenizer then performs another normalization layer over the joined text.

## Work a round trip

Start with tags queue, Queue and a whitespace-padded worker value. JoinTags keeps one case-insensitive queue representative and trims worker. Splitting the result returns the separated values. Now add a single tag containing a comma, such as a synthetic product label with two parts. Joining and splitting can turn one logical tag into two because there is no escaping protocol in these helpers.

This may be acceptable if request validation prohibits commas in individual tags. Inspect that boundary before declaring data loss for accepted input. If commas are allowed, propose a structured representation such as a JSON array or a normalized relation. The migration must preserve existing values and explain ambiguity in historical strings that cannot be unambiguously split back into original tags.

## Case and identity

Case-insensitive deduplication preserves one representative spelling, while retrieval lowercases tokens. A UI might display the first spelling it received. If the product wants canonical display names or aliases, that needs a separate tag identity model. A string comparison helper does not create a managed taxonomy.

Create a fixture with Queue, queue and QUEUE, then compare stored display text, split values and retrieval tokens. Explain which differences are presentation and which affect matching. Adding punctuation can split one visible tag into multiple tokens, so a tag label's visual identity is not necessarily one retrieval term.

## Length and storage limits

The DbContext limits TagsCsv length for relevant entities. A request containing many individually short tags can exceed the joined length. Application validation should consider the stored representation if the product wants a clear user error before database failure. An EF InMemory test may not enforce the same relational column limits, so use the appropriate provider for that claim.

Design boundary cases for exactly the allowed joined length and one character beyond it. Include commas and trimming in the calculation. A test that counts only original string lengths can miss separator overhead. A proposed normalized tag table changes those limits and query costs, so do not treat it as a purely mechanical storage replacement.

## Retrieval effects

Tags carry three points per matching query term in the current retriever. A tag-normalization change can therefore alter ranking. If migration splits or merges tags differently, rerun the labeled evaluation set and exact score fixtures. A data-cleanup script can unintentionally change retrieval behavior even without touching LexicalRetriever.

Suppose an imported tag contains queue-depth. The tokenizer yields queue and depth, each eligible for tag weight. A proposed taxonomy might instead treat it as one canonical concept. That would be a new retrieval contract. State the choice and test both relevant and irrelevant queries rather than assuming structured storage alone improves quality.

## Exercise

Submit a round-trip matrix, accepted-input policy, joined-length test and migration proposal. Mark historical ambiguities that cannot be recovered automatically. A strong answer connects representation, validation, display and ranking while keeping each responsibility explicit.

[Index](README.md) · [Review notes](answers/11-time-tags-and-metadata.md)
