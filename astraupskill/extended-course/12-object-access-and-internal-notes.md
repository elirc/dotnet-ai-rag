# 12. Read access and field visibility are separate policies

[RequestContext](../../src/Application/Common/RequestContext.cs) derives user ID and role from claims. Admin and Engineer have broad investigation capabilities; SupportAnalyst is included in support mutation capability. Ticket reads also use creator or assignee relationships for callers outside the broad role group. This is object-level access. The visibility of particular fields, such as internal comments, requires a separate review.

## Follow a ticket read

ListTicketsAsync filters non-Admin/Engineer callers to tickets they created or are assigned. GetTicketAsync loads the ticket and returns null when CanReadTicket fails. The controller maps null to NotFound. This can conceal inaccessible objects rather than returning a distinct forbidden response. A test should compare a missing ID with an existing but inaccessible ID under the intended policy.

Once access is granted, ToTicketDetail maps the ticket's comments, including their IsInternal flag. The inspected mapper does not filter comments by that flag. Whether this is correct depends on what Internal means for each authorized role. Do not declare a leak solely from the name; define the intended audience and compare it with actual output.

## Build a visibility matrix

Use roles Admin, Engineer, SupportAnalyst and Viewer, combined with creator, assignee and unrelated relationships. For each, record whether the ticket is readable and whether public comments, internal notes, audit summaries and assignment identifiers should be visible. Mark the current code's observed behavior separately from desired policy.

An object-level rule can be correct while field-level visibility is too broad for the product's intended audience. Conversely, if all authorized ticket readers are meant to see internal notes, the flag may indicate workflow classification rather than confidentiality. The exercise requires a product definition instead of relying on a familiar label from another system.

## Mutation capability and ownership

Support mutations first require CanMutateSupportWork and then use CanReadTicket on the loaded object. This composes a role capability with an object relationship. A SupportAnalyst may have mutation capability but still fail the object relationship if they are neither creator nor assignee. Admin and Engineer pass the broader read condition.

Create fixtures for those combinations and assert no writes occur after rejection. Checking only an exception can miss an audit row or comment added before authorization. The inspected methods perform capability checks early, but a future refactor should retain that ordering. Negative-state assertions protect it.

## Claims parsing

FromClaims requires a name-identifier claim and parses it as a Guid. It defaults a missing role to Viewer. Missing or malformed identifiers can throw before service work. Authentication middleware may normally ensure valid claims, but direct tests should state which boundary they exercise. A RequestContext constructed manually can bypass claim parsing and therefore does not prove token validation.

Role strings are compared with specific casing in the derived properties. Investigate how authentication creates them before assuming arbitrary case normalization. A proposed role enum or validated claim mapping can reduce drift, but it needs compatibility with existing tokens and policies. Do not silently broaden accepted roles in a learning exercise.

## Exercise

Submit the object/field visibility matrix, two unauthorized no-write tests and a claims-boundary test plan. Include a proposed internal-note policy with one counterexample. A strong answer separates authentication, role capability, object relationship and field projection. Those are four different questions even when one request passes through all of them.

[Index](README.md) · [Review notes](answers/06-ticket-and-visibility.md)
