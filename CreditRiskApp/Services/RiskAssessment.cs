namespace CreditRiskApp.Services;

/// <summary>
/// Service for assessing customer risk based on credit scores.
/// </summary>
public static class RiskAssessment
{
    /// <summary>
    /// High-risk threshold. Customers with scores below this value are considered high risk.
    /// </summary>
    private const int HighRiskThreshold = 50;

    /// <summary>
    /// Determines if a customer is high risk based on their credit score.
    /// </summary>
    /// <param name="creditScore">The customer's credit score.</param>
    /// <returns>True if the customer is high risk (score < 50), false otherwise.</returns>
    public static bool IsHighRisk(int creditScore)
    {
        return creditScore < HighRiskThreshold;
    }

    /// <summary>
    /// Gets the risk status string for a given credit score.
    /// </summary>
    /// <param name="creditScore">The customer's credit score.</param>
    /// <returns>"High Risk" if score < 50, "Low Risk" otherwise.</returns>
    public static string GetRiskStatus(int creditScore)
    {
        return IsHighRisk(creditScore) ? "High Risk" : "Low Risk";
    }
}

