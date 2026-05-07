using System.ComponentModel.DataAnnotations;

namespace PayrollSystem.Domain.Entities;

public enum LeaveStatus
{
    Pending = 0,
    Approved = 1,
    Rejected = 2,
}

public sealed class LeaveType
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    public bool IsPaid { get; set; } = true;
}

public sealed class LeaveRequest
{
    public int Id { get; set; }

    public int EmployeeId { get; set; }
    public Employee? Employee { get; set; }

    public int LeaveTypeId { get; set; }
    public LeaveType? LeaveType { get; set; }

    [DataType(DataType.Date)]
    public DateOnly StartDate { get; set; }

    [DataType(DataType.Date)]
    public DateOnly EndDate { get; set; }

    [StringLength(500)]
    public string? Reason { get; set; }

    public LeaveStatus Status { get; set; } = LeaveStatus.Pending;

    public DateTime RequestedAtUtc { get; set; } = DateTime.UtcNow;

    [StringLength(120)]
    public string? DecidedBy { get; set; }

    public DateTime? DecidedAtUtc { get; set; }
}

