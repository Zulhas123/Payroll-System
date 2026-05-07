namespace PayrollSystem.Domain.Entities;

public sealed class PayrollRun
{
    public int Id { get; set; }

    public int PayPeriodId { get; set; }
    public PayPeriod? PayPeriod { get; set; }

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public ICollection<PayrollLine> Lines { get; set; } = new List<PayrollLine>();
}
