namespace Domain.Entities;

public abstract class BaseEntity
{
    public int Id { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public  string? CreatedBy { get; set; }
    public  string? UpdatedBy { get; set; }
    public DateTimeOffset Created { get; set; }
    
}
