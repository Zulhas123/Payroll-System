using System.ComponentModel.DataAnnotations;

namespace PayrollSystem.Web.ViewModels;

public sealed class PayrollRunCreateVm
{
    [DataType(DataType.Date)]
    [Display(Name = "Start date")]
    public DateOnly StartDate { get; set; } = DateOnly.FromDateTime(DateTime.Today).AddDays(-30);

    [DataType(DataType.Date)]
    [Display(Name = "End date")]
    public DateOnly EndDate { get; set; } = DateOnly.FromDateTime(DateTime.Today);
}
