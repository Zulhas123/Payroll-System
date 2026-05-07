using System.ComponentModel.DataAnnotations;

namespace PayrollSystem.Domain.Entities;

public sealed class PayrollLine
{
    public int Id { get; set; }

    public int PayrollRunId { get; set; }
    public PayrollRun? PayrollRun { get; set; }

    public int EmployeeId { get; set; }
    public Employee? Employee { get; set; }

    [Range(0, 1_000_000)]
    public decimal Basic { get; set; }

    [Range(0, 1_000_000)]
    public decimal Allowances { get; set; }

    [Range(0, 1_000_000)]
    public decimal Deductions { get; set; }

    public decimal NetPay => Basic + Allowances - Deductions;
}
