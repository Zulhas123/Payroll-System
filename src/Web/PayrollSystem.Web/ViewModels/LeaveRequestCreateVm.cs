using System.ComponentModel.DataAnnotations;

namespace PayrollSystem.Web.ViewModels;

public sealed class LeaveRequestCreateVm
{
    [Required]
    [Display(Name = "Employee")]
    public int EmployeeId { get; set; }

    [Required]
    [Display(Name = "Leave type")]
    public int LeaveTypeId { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Start date")]
    public DateOnly StartDate { get; set; } = DateOnly.FromDateTime(DateTime.Today);

    [DataType(DataType.Date)]
    [Display(Name = "End date")]
    public DateOnly EndDate { get; set; } = DateOnly.FromDateTime(DateTime.Today);

    [StringLength(500)]
    public string? Reason { get; set; }
}

