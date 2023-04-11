namespace Platform.Domain.Abstractions;

public interface IEntity
{
    public IReadOnlyCollection<IDomainEvent> DomainEvents { get; }
    void ClearDomainEvents();
}