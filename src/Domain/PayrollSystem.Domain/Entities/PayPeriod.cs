using System.ComponentModel.DataAnnotations;

namespace PayrollSystem.Domain.Entities;

public sealed class PayPeriod
{
    public int Id { get; set; }

    [DataType(DataType.Date)]
    public DateOnly StartDate { get; set; }

    [DataType(DataType.Date)]
    public DateOnly EndDate { get; set; }

    public ICollection<PayrollRun> PayrollRuns { get; set; } = new List<PayrollRun>();
}
