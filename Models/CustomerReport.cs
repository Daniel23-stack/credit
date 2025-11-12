namespace CreditRiskApp.Models;

/// <summary>
/// Represents a customer credit report with calculated credit score and risk status.
/// </summary>
public class CustomerReport
{
    /// <summary>
    /// Name of the customer.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Calculated credit score.
    /// </summary>
    public int CreditScore { get; set; }

    /// <summary>
    /// Risk status: "High Risk" or "Low Risk".
    /// </summary>
    public string RiskStatus { get; set; } = string.Empty;
}

