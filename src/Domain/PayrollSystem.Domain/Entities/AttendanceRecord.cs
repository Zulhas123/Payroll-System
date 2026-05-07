using System.ComponentModel.DataAnnotations;

namespace PayrollSystem.Domain.Entities;

public enum AttendanceSource
{
    Manual = 0,
    Device = 1,
    Mobile = 2,
}

public sealed class AttendanceRecord
{
    public int Id { get; set; }

    public int EmployeeId { get; set; }
    public Employee? Employee { get; set; }

    [DataType(DataType.Date)]
    public DateOnly Date { get; set; }

    public TimeOnly? CheckIn { get; set; }
    public TimeOnly? CheckOut { get; set; }

    public AttendanceSource Source { get; set; } = AttendanceSource.Manual;

    public decimal? TotalHours
    {
        get
        {
            if (CheckIn is null || CheckOut is null) return null;
            var duration = CheckOut.Value.ToTimeSpan() - CheckIn.Value.ToTimeSpan();
            if (duration < TimeSpan.Zero) return null;
            return (decimal)duration.TotalHours;
        }
    }
}

