using System.ComponentModel.DataAnnotations;

namespace PayrollSystem.Web.ViewModels;

public sealed class AttendanceUpsertVm
{
    [Required]
    [Display(Name = "Employee")]
    public int EmployeeId { get; set; }

    [DataType(DataType.Date)]
    public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.Today);

    [DataType(DataType.Time)]
    [Display(Name = "Check-in")]
    public TimeOnly? CheckIn { get; set; }

    [DataType(DataType.Time)]
    [Display(Name = "Check-out")]
    public TimeOnly? CheckOut { get; set; }
}

