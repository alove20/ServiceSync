namespace ServiceSync.Core.Enums;

/// <summary>
/// Defines the possible states of an estimate.
/// </summary>
public enum EstimateStatus
{
    Draft,      // 0: Initial state, not yet sent to client
    Sent,       // 1: Sent to the client for approval
    Approved,   // 2: Client has approved the estimate
    Declined    // 3: Client has declined the estimate
}
