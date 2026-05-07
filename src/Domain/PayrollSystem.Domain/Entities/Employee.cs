using System.ComponentModel.DataAnnotations;

namespace PayrollSystem.Domain.Entities;

public sealed class Employee
{
    public int Id { get; set; }

    [Required]
    [StringLength(30)]
    public string EmployeeNo { get; set; } = string.Empty;

    [Required]
    [StringLength(150)]
    public string FullName { get; set; } = string.Empty;

    [EmailAddress]
    [StringLength(200)]
    public string? Email { get; set; }

    public int DepartmentId { get; set; }
    public Department? Department { get; set; }

    [DataType(DataType.Date)]
    public DateOnly HireDate { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);

    [Range(0, 1_000_000)]
    public decimal BasicSalary { get; set; }

    public bool IsActive { get; set; } = true;
}
