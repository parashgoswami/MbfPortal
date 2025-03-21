namespace Domain.Entities;
public class Member : BaseEntity
{
    public string EmpNo { get; set; }  = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Nominee { get; set; } 
    public string Email { get; set; } = string.Empty;
    public DateTime DOJ { get; set; }
    public DateTime? DOS { get; set; }
    public int LocationId { get; set; }
    public decimal Share { get; set; }
    public bool IsActive { get; set; } = true;
    public Location? Location { get; set; }
}
