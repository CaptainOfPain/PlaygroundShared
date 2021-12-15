using Newtonsoft.Json;

namespace PlaygroundShared.Domain.Domain;

public struct AggregateId : IEquatable<AggregateId>
{
    public Guid Id { get; }
        
    public AggregateId(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(id));
        }
            
        Id = id;
    }

    [JsonConstructor]
    public AggregateId(string id)
    {
        if (!Guid.TryParse(id, out var parsedId))
        {
            throw new ArgumentNullException(nameof(id));
        }

        Id = parsedId;
    }

    public bool Equals(AggregateId other)
    {
        return Id.Equals(other.Id);
    }

    public override bool Equals(object obj)
    {
        return obj is AggregateId other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public override string ToString()
    {
        return Id.ToString();
    }
        
    public static bool operator == (AggregateId a, AggregateId b)
    {
        return a.Equals(b);
    }

    public static bool operator !=(AggregateId a, AggregateId b)
    {
        return !(a == b);
    }

    public static AggregateId Generate() => new AggregateId(Guid.NewGuid());

    public Guid ToGuid() => Id;
}