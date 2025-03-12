using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;
public class FinYearStatus : BaseEntity
{
    public string FinYear { get; set; } = string.Empty;
    public bool isClosed { get; set; }
}
