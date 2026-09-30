# 32. Review a recommendation with another engineer

Two learners use one synthetic investigation from the course. One prepares the recommendation and evidence bundle; the other acts as a skeptical reviewer. The goal is to reach a justified next step, which may be further investigation or explicit abstention. No operational action is executed in this workshop.

## Prepare the bundle

The author supplies incident identity and time provenance, log bounds, evidence count, selected IDs, retrieval query, corpus version if known, matched document IDs, scores, terms and execution mode. Unknown fields remain visibly unknown. The author also supplies one proposed mitigation as a separate item, clearly distinguished from the retrieval output.

The reviewer first checks whether the bundle is internally consistent. Does the sample size respect the bound? Do scores match field arithmetic? Does model metadata match the deterministic path? Are the referenced documents the versions actually reviewed? An internally consistent bundle can still support an incorrect hypothesis, so this is only the first stage.

## Challenge relevance and causality

The reviewer asks what evidence would distinguish the proposed cause from an alternative. A queue runbook matching queue terms is not proof that queue saturation caused the incident. A log showing timeouts can be a symptom of several failures. Name one alternative explanation and one discriminating observation that would change the decision.

Avoid collecting more data indiscriminately. If the question is whether database pool exhaustion preceded queue lag, a targeted time-aligned query may be more useful than another broad search for queue. The workshop should teach information value: choose the next observation that separates plausible hypotheses.

## Challenge authority

Even a plausible mitigation needs an authorized action contract. Who can approve it, what exact target and parameters are being approved, and which evidence version supports the decision? The current application creates a Pending approval, but the proposed execution workflow must be specified separately. The reviewer should reject an answer that treats pending status as permission.

Introduce a runbook edit after the author prepared the bundle. Ask whether the review still concerns the old snapshot or the new content. Then introduce another investigation run with different evidence. The author must explain how the selected recommendation is identified and whether a fresh review is required under the chosen policy.

## Challenge recovery

Suppose the browser loses the response after investigation persistence. How does the user recover the same run? Suppose a future executor accepts an action but loses its acknowledgment. How does the system avoid repeating it blindly? These are different boundaries, and the design should use separate command or attempt identities where needed.

The reviewer should insist on a concrete state and evidence table rather than a promise to retry safely. A receipt can help recover a local command, while an external outcome may require reconciliation. A timeout alone does not reveal which durable effects occurred.

## Final handoff

Write a user-facing summary and an operator-facing handoff. The user summary should state whether useful evidence was found and what needs review next. The operator handoff should include identities, last confirmed phase, unresolved hypotheses and the next discriminating observation. Both should agree on uncertainty and avoid overstating the heuristic score.

The workshop passes when the pair can explain one supported conclusion, one unsupported conclusion and one safe next step. This is the central outcome of the course: precise retrieval mechanics combined with disciplined evidence and authority reasoning.

## Keep a decision log

Record each revised conclusion with the observation that changed it. For example, a runbook initially judged relevant may become unsuitable after the reviewer discovers that it targets an older deployment. A queue hypothesis may weaken when time-aligned logs show database saturation first. The decision log should preserve the earlier reasoning rather than editing it to look as though the final answer was obvious from the start.

Use four fields: prior belief, new observation, revised conclusion and remaining uncertainty. Avoid numerical confidence unless the team has defined what the number means. Plain statements such as plausible, contradicted by this evidence or unresolved are often more honest in a small educational investigation.

Finally, identify one observation that would reverse the proposed next step. This counterfactual tests whether the recommendation is responsive to evidence or merely defended after the fact. If no possible observation could change it, the reviewer should ask whether the conclusion is a policy decision, an assumption or an unsupported certainty. Distinguishing those categories makes both the technical record and the human handoff more useful.

Have a second reviewer challenge the strongest conclusion using only the recorded evidence. Resolve disagreements by naming the missing observation and its collection method, then preserve that unresolved question in the handoff.

[Index](README.md) · [Answers](answers/14-final-calculations.md)
