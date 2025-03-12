namespace Domain.Entities;
public class AccountHead : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}
