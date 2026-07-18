# Phase 2 Journal: Core IT Ops CRUD

## Planning

Phase 2 will turn the foundation into an operations product. The main teaching goal is modeling workflows as stateful resources: tickets, incidents, comments, timeline events, logs, and audit trails.

## Implementation Plan

- Add ticket entities, DTOs, validation, services, controllers, and pages.
- Add incident entities, DTOs, validation, services, controllers, and pages.
- Add synthetic log entities and search endpoints.
- Add audit events for important ticket and incident changes.
- Add focused unit tests for status transitions and service behavior.

## Retrospective Template

After implementation, document which state transitions were allowed, where audit events are written, and how the UI helps users understand the difference between support tickets and production incidents.
