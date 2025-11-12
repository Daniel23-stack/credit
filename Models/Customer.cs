using System.ComponentModel.DataAnnotations;

namespace CreditRiskApp.Models;

/// <summary>
/// Represents a customer with credit-related information.
/// </summary>
public class Customer
{
    /// <summary>
    /// Unique identifier for the customer.
    /// </summary>
    [Required]
    public int CustomerId { get; set; }

    /// <summary>
    /// Name of the customer.
    /// </summary>
    [Required]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Percentage of payments made on time (0 to 100).
    /// </summary>
    [Range(0, 100, ErrorMessage = "PaymentHistory must be between 0 and 100.")]
    public int PaymentHistory { get; set; }

    /// <summary>
    /// Percentage of credit limit used (0 to 100).
    /// </summary>
    [Range(0, 100, ErrorMessage = "CreditUtilization must be between 0 and 100.")]
    public int CreditUtilization { get; set; }

    /// <summary>
    /// Age of credit history in years.
    /// </summary>
    [Range(0, int.MaxValue, ErrorMessage = "AgeOfCreditHistory must be a non-negative number.")]
    public int AgeOfCreditHistory { get; set; }
}

