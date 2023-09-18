namespace SNS2Slack;

public class Alarm
{
    public string? AlarmName { get; set; }
    public string? AlarmDescription { get; set; }
    public string? AWSAccountId { get; set; }
    public string? NewStateValue { get; set; }
    public string? NewStateReason { get; set; }
    public string? OldStateValue { get; set; }
    public Trigger? Trigger { get; set; }
}

public class Trigger
{
    public string? MetricName { get; set; }
    public string? Namespace { get; set; }
    public string? Statistic { get; set; }
    public string? GreaterThanOrEqualToThreshold { get; set; }
    public decimal Period { get; set; }
    public decimal Threshold { get; set; }
    public decimal EvaluationPeriods { get; set; }
}