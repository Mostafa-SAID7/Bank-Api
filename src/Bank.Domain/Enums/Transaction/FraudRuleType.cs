namespace Bank.Domain.Enums;

public enum FraudRuleType
{
    TransactionAmount,
    TransactionFrequency,
    LocationAnomaly,
    TimeAnomaly,
    DeviceAnomaly,
    BehaviorAnomaly
}
