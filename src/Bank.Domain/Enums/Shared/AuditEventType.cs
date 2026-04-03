namespace Bank.Domain.Enums;

public enum AuditEventType
{
    UserAction = 1,
    SystemEvent = 2,
    SecurityEvent = 3,
    ComplianceEvent = 4
}
