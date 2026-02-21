# E-Commerce API Practice Project

Welcome to the E-Commerce API Practice Project! This solution serves as a comprehensive playground demonstrating the application of advanced C# language features, Object-Oriented Programming (OOP) concepts, Design Patterns, and Clean Architecture principles in a .NET Core Web API environment.

## 🏗️ Architecture

The project follows **Clean Architecture** principles, enforcing separation of concerns and dependency inversion across four distinct layers:

1. **`ECommerceAPI.Domain`**: Core business logic, Entities, Value Objects, and abstract Contracts (Interfaces) with *zero* external dependencies.
2. **`ECommerceAPI.Application`**: Use cases orchestrated via CQRS (MediatR), DTOs, and Strategy pattern implementations. Depends only on the Domain.
3. **`ECommerceAPI.Infrastructure`**: Concrete implementation details for Data Access (Entity Framework Core DbContext, Repositories, Unit of Work), and external services. 
4. **`ECommerceAPI.Presentation`**: The ASP.NET Core Web API surface, housing thin Controllers and API documentation endpoints (Scalar).
5. **Global Exception Handling**: A centralized middleware in the Presentation layer (`GlobalExceptionHandler.cs`) built using .NET 8's `IExceptionHandler`. It catches all unhandled exceptions, logs them, and formats them into a standardized RFC 7807 `ProblemDetails` JSON response, ensuring the API always returns structured errors (e.g., 400 Bad Request for Validation, 500 for server errors).

---

## 🧬 Object-Oriented Programming (OOP) Concepts

The core pillars of OOP form the foundation of our Domain layer:

*   **Abstraction**: 
    *   *Where*: `ECommerceAPI.Domain/Common/Entity.cs`, `ECommerceAPI.Domain/Interfaces/`
    *   *How*: By defining base classes (like `Entity`) and interfaces (`IUnitOfWork`, `IRepository<T>`), we expose contracts and behaviors without revealing complex implementations.
*   **Encapsulation**: 
    *   *Where*: `ECommerceAPI.Domain/Entities/Order.cs`, `ECommerceAPI.Domain/Entities/OrderItem.cs`
    *   *How*: Properties like the `Order.Items` collection are hidden behind private fields (`_items`). State changes can only occur through controlled methods such as `AddLineItem()`, ensuring invariants are always maintained.
*   **Inheritance**: 
    *   *Where*: `ECommerceAPI.Domain/Entities/Product.cs`, `PhysicalProduct.cs`, `DigitalProduct.cs`
    *   *How*: `PhysicalProduct` and `DigitalProduct` inherit common properties (Id, Name, Price) from the abstract `Product` base class.
*   **Polymorphism**: 
    *   *Where*: `ECommerceAPI.Domain/Entities/Product.cs`
    *   *How*: The abstract method `CalculateShippingCost()` is declared in the base `Product` class. Both `PhysicalProduct` and `DigitalProduct` provide their independent overridden implementation of this calculation.

---

## 🚀 C# Language Features

This project specifically highlights various C# keywords and features to solve specific software design problems:

*   **`class` & `abstract`**: 
    *   *Where*: Ubiquitous across the solution (e.g., `Product.cs`, `Entity.cs`). 
    *   *How*: Serves as the blueprint for reference types. `abstract` ensures the `Product` base class cannot be instantiated directly.
*   **`interface`**: 
    *   *Where*: `IRepository.cs`, `IUnitOfWork.cs`, `IDiscountStrategy.cs`
    *   *How*: Defines strict contracts for Dependency Injection, enabling loose coupling.
*   **`struct` (Value Objects)**: 
    *   *Where*: `ECommerceAPI.Domain/ValueObjects/Money.cs`
    *   *How*: Used to define the `Money` type. Structs are value types allocated on the stack (preventing garbage collection overhead) and enforce equality by value rather than reference. Features operator overloading for arithmetic operations.
*   **`enum`**: 
    *   *Where*: `ECommerceAPI.Domain/Enums/OrderStatus.cs`
    *   *How*: Heavily enforces strict, self-documenting categorizations for the state of an `Order`.
*   **`record`**: 
    *   *Where*: `ECommerceAPI.Application/Commands/CreateOrderCommand.cs`, `ECommerceAPI.Application/DTOs/OrderItemDto.cs`
    *   *How*: Defines highly concise, immutable reference types, making them perfect for thread-safe Data Transfer Objects (DTOs) and CQRS Commands.
*   **`sealed`**: 
    *   *Where*: `CreateOrderCommandHandler.cs`, `NoDiscountStrategy.cs`
    *   *How*: Placed on classes to explicitly prevent further inheritance, which provides a slight performance optimization at runtime and clearly communicates design intent.
*   **`generics` (`<T>`)**: 
    *   *Where*: `ECommerceAPI.Domain/Interfaces/IRepository<T>.cs`, `ECommerceAPI.Infrastructure/Repositories/Repository.cs`
    *   *How*: Allows the repository pattern to be strongly typed yet reusable across any domain `Entity`, preventing duplicate code.
*   **`delegate` & `event`**: 
    *   *Where*: `ECommerceAPI.Domain/Entities/Order.cs`
    *   *How*: A custom delegate `OrderCompletedHandler` defines a callback signature. The `OnOrderCompleted` event allows external subscribers to react when an order is completed, demonstrating a pub/sub mechanism within the domain.
*   **`ref` properties**: 
    *   *Where*: `Order.ApplyTax(ref decimal currentTotal, decimal taxRate)`
    *   *How*: Passed arguments dynamically modify the memory address of the original value type directly, bypassing the need to return a new object to the caller.

---

## 🧩 Design Patterns 

The application architecture heavily relies on established design patterns:

*   **Repository & Unit of Work**: (`ECommerceAPI.Infrastructure/Repositories`) Abstracts DbContext operations and wraps changes in a single transactional save, keeping data access logic out of the application layer.
*   **CQRS (Command Query Responsibility Segregation)**: (`ECommerceAPI.Application/Commands`) Mediated by the `MediatR` library, Write operations (Commands) are handled strictly separately from Read operations (Queries).
*   **Strategy Pattern**: (`ECommerceAPI.Application/Strategies`) Encapsulates different discount algorithms (`FixedAmountDiscountStrategy`, `PercentageDiscountStrategy`) behind a common `IDiscountStrategy` interface, allowing behavior to be selected at runtime.
*   **Decorator Pattern**: (`ECommerceAPI.Infrastructure/Repositories/CachedRepository.cs`) Dynamically adds Memory Caching behavior to standard database queries without modifying the underlying `Repository<T>` code.
*   **Singleton**: (`ECommerceAPI.Infrastructure/Configuration/AppConfiguration.cs`) Ensures only one instance of application settings exists across the lifetime of the application.

---

## 🧪 Testing

The solution includes an xUnit suite (`ECommerceAPI.Tests`) executing validation across boundaries:
1. **Domain Tests**: Verifying Polymorphism (`CalculateShippingCost`), Encapsulation (`AddLineItem`), and Event triggers without touching databases.
2. **Application Tests**: Utilizing **Moq** to inject mocked repositories/Unit of Work, and **FluentAssertions** to validate CQRS logic and exception throwing seamlessly.

To run the application or tests, navigate via the CLI to the root folder:
*   `dotnet build`
*   `dotnet test`
*   `dotnet run --project ECommerceAPI.Presentation/ECommerceAPI.Presentation.csproj`
