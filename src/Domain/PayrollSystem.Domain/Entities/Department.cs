using System.ComponentModel.DataAnnotations;

namespace PayrollSystem.Domain.Entities;

public sealed class Department
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;
}
