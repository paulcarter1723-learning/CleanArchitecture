# ADR-0001 — The solution follows the Clean Architecture project structure

**Status:** accepted · **Date:** 2023-09-05

### Context

The repository is a template for ASP.NET Core web applications, so its
structure is copied into every solution created from it. The default .NET
project templates put controllers, data access and business rules in one
project, where nothing stops a controller from calling the database directly
or a business rule from depending on Entity Framework. The template also aims
at Domain-Driven Design and CQRS, which need a domain model that stays free of
persistence and HTTP concerns.

This record was written on 2026-10-28. The decision itself dates from the
initial commit.

### Decision

The repository owner decided that the solution follows the Clean Architecture
project structure, with one project per layer and every compile-time
dependency pointing inward:

- **`CleanArchitecture.Domain`** is at the centre. It holds aggregates, entities,
  value objects, domain events, guards and domain exceptions, and references no
  other project in the solution.
- **`CleanArchitecture.Application`** references only the Domain. It holds the
  use cases (commands, queries and their handlers), domain event handlers,
  pipeline behaviours, DTOs and mapping profiles. It defines the interfaces it
  needs from the outside world, such as `IRepository<T>`, `IUnitOfWork`, `IUser`
  and `INotificationsService`.
- **`CleanArchitecture.Infrastructure`** implements those interfaces with
  EF Core and other external services. It references Application and is never
  referenced by it.
- **`CleanArchitecture.Api`** and **`CleanArchitecture.Migrations`** are entry
  points. They compose the Autofac modules from Application and Infrastructure,
  and use **`CleanArchitecture.Hosting`** for shared host defaults.
- **`CleanArchitecture.Web`** hosts the Angular SPA. It shares no code with the
  backend and calls the API over HTTP.

Separate projects let the compiler reject a dependency that points outward.
The rules that project references cannot express, such as where aggregates,
domain events and repositories may live, are enforced by
`tests/CleanArchitecture.Arch.Tests`.

### Consequences

- A new feature touches several projects: the model in Domain, a command or
  query in Application, and persistence configuration in Infrastructure if the
  model changes. A controller action only sends a MediatR request.
- Application code reaches the database only through `IRepository<T>` and
  `IUnitOfWork`. A handler that needs a new kind of data access adds a method to
  an interface in Application and implements it in Infrastructure.
- The architecture tests check namespaces and type dependencies, not package
  references. A library pulled into an inner project, such as MediatR in Domain
  or EF Core in Application, does not fail them, so such a choice has to be made
  deliberately.
- Adding a project means deciding which ring it belongs to, and adding
  architecture tests for it if its boundary matters.

### Revisit when

The solution grows enough feature areas that changes routinely cut across all
layers for one feature, and the team would rather organise code by feature
than by layer.

### Alternatives considered

- **A single project with folders per layer.** Simpler to navigate and build,
  but the layer boundaries would depend on discipline and architecture tests
  alone, with no help from the compiler. Rejected by the repository owner.
- **A traditional N-tier structure** (presentation, business logic, data
  access), where business logic depends on the data access layer. It makes the
  domain depend on persistence, which works against the Domain-Driven Design
  aims of the template. Rejected by the repository owner.
- **Vertical slice architecture**, organising code by feature instead of by
  layer. Not chosen for the template, whose purpose is to demonstrate Clean
  Architecture and Domain-Driven Design. Rejected by the repository owner.
