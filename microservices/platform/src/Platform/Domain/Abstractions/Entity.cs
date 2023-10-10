namespace Platform.Domain.Abstractions;

public abstract class Entity<TId> : IEntity, IEquatable<IEntity>  where TId: notnull
{
    private List<IDomainEvent> _domainEvents;
    
    public TId Id { get; protected set; }
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents?.AsReadOnly();

    protected  Entity(){}

    protected Entity(TId id)
    {
        Id = id;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public override bool Equals(object obj)
    {
        return obj is Entity<TId> entity && Id.Equals(entity.Id);
    }
    
    public bool Equals(IEntity other)
    {
        return Equals((object)other);
    }
    
    public static bool operator ==(Entity<TId> left, Entity<TId>  right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Entity<TId>  left, Entity<TId>  right)
    {
        return !Equals(left, right);
    }

    protected void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents = _domainEvents ?? new List<IDomainEvent>();
        _domainEvents.Add(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents?.Clear();
    }
}