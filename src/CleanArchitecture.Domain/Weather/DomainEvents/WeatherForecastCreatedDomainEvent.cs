using CleanArchitecture.Domain.Abstractions.DomainEvents;

namespace CleanArchitecture.Domain.Weather.DomainEvents
{
    public sealed record WeatherForecastCreatedDomainEvent(Guid Id, int Temperature, string Summary, DateTime Date) : DomainEvent;
}
