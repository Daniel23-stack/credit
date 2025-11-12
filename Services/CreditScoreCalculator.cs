using CreditRiskApp.Models;

namespace CreditRiskApp.Services;

/// <summary>
/// Service for calculating credit scores based on customer data.
/// </summary>
public static class CreditScoreCalculator
{
    /// <summary>
    /// Calculates a credit score for a customer using the formula:
    /// CreditScore = (0.4 * PaymentHistory) + (0.3 * (100 - CreditUtilization)) + (0.3 * Min(AgeOfCreditHistory, 10))
    /// </summary>
    /// <param name="customer">The customer for whom to calculate the credit score.</param>
    /// <returns>An integer credit score.</returns>
    /// <exception cref="ArgumentNullException">Thrown when customer is null.</exception>
    public static int CalculateCreditScore(Customer customer)
    {
        if (customer == null)
        {
            throw new ArgumentNullException(nameof(customer), "Customer cannot be null.");
        }

        // Clamp values to valid ranges to handle edge cases
        var paymentHistory = Math.Max(0, Math.Min(100, customer.PaymentHistory));
        var creditUtilization = Math.Max(0, Math.Min(100, customer.CreditUtilization));
        var ageOfCreditHistory = Math.Max(0, customer.AgeOfCreditHistory);

        // Apply the credit score formula
        // CreditScore = (0.4 * PaymentHistory) + (0.3 * (100 - CreditUtilization)) + (0.3 * Min(AgeOfCreditHistory, 10))
        var score = (0.4 * paymentHistory) 
                  + (0.3 * (100 - creditUtilization)) 
                  + (0.3 * Math.Min(ageOfCreditHistory, 10));

        // Round to nearest integer (away from zero for .5 cases)
        return (int)Math.Round(score, MidpointRounding.AwayFromZero);
    }
}

