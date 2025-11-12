using CreditRiskApp.Models;
using CreditRiskApp.Services;
using Xunit;

namespace CreditRiskApp.Tests;

/// <summary>
/// Unit tests for the CreditScoreCalculator class.
/// </summary>
public class CreditScoreCalculatorTests
{
    [Fact]
    public void CalculateCreditScore_WithValidCustomer_ReturnsCorrectScore()
    {
        // Arrange
        var customer = new Customer
        {
            CustomerId = 1,
            Name = "Alice",
            PaymentHistory = 90,
            CreditUtilization = 40,
            AgeOfCreditHistory = 5
        };

        // Expected: (0.4 * 90) + (0.3 * (100 - 40)) + (0.3 * Min(5, 10))
        //          = 36 + 18 + 1.5 = 55.5 -> 56 (rounded)
        var expectedScore = 56;

        // Act
        var result = CreditScoreCalculator.CalculateCreditScore(customer);

        // Assert
        Assert.Equal(expectedScore, result);
    }

    [Fact]
    public void CalculateCreditScore_WithHighCreditUtilization_ReturnsLowerScore()
    {
        // Arrange
        var customer = new Customer
        {
            CustomerId = 2,
            Name = "Bob",
            PaymentHistory = 70,
            CreditUtilization = 90,
            AgeOfCreditHistory = 15
        };

        // Expected: (0.4 * 70) + (0.3 * (100 - 90)) + (0.3 * Min(15, 10))
        //          = 28 + 3 + 3 = 34
        var expectedScore = 34;

        // Act
        var result = CreditScoreCalculator.CalculateCreditScore(customer);

        // Assert
        Assert.Equal(expectedScore, result);
    }

    [Fact]
    public void CalculateCreditScore_WithAgeOfCreditHistoryAbove10_CapsAt10()
    {
        // Arrange
        var customer = new Customer
        {
            CustomerId = 1,
            Name = "Test",
            PaymentHistory = 100,
            CreditUtilization = 0,
            AgeOfCreditHistory = 20 // Should be capped at 10
        };

        // Expected: (0.4 * 100) + (0.3 * (100 - 0)) + (0.3 * Min(20, 10))
        //          = 40 + 30 + 3 = 73
        var expectedScore = 73;

        // Act
        var result = CreditScoreCalculator.CalculateCreditScore(customer);

        // Assert
        Assert.Equal(expectedScore, result);
    }

    [Fact]
    public void CalculateCreditScore_WithZeroPaymentHistory_ReturnsLowScore()
    {
        // Arrange
        var customer = new Customer
        {
            CustomerId = 1,
            Name = "Test",
            PaymentHistory = 0,
            CreditUtilization = 0,
            AgeOfCreditHistory = 10
        };

        // Expected: (0.4 * 0) + (0.3 * (100 - 0)) + (0.3 * Min(10, 10))
        //          = 0 + 30 + 3 = 33
        var expectedScore = 33;

        // Act
        var result = CreditScoreCalculator.CalculateCreditScore(customer);

        // Assert
        Assert.Equal(expectedScore, result);
    }

    [Fact]
    public void CalculateCreditScore_WithPerfectCustomer_ReturnsHighScore()
    {
        // Arrange
        var customer = new Customer
        {
            CustomerId = 1,
            Name = "Test",
            PaymentHistory = 100,
            CreditUtilization = 0,
            AgeOfCreditHistory = 10
        };

        // Expected: (0.4 * 100) + (0.3 * (100 - 0)) + (0.3 * Min(10, 10))
        //          = 40 + 30 + 3 = 73
        var expectedScore = 73;

        // Act
        var result = CreditScoreCalculator.CalculateCreditScore(customer);

        // Assert
        Assert.Equal(expectedScore, result);
    }

    [Fact]
    public void CalculateCreditScore_WithNegativePaymentHistory_ClampsToZero()
    {
        // Arrange
        var customer = new Customer
        {
            CustomerId = 1,
            Name = "Test",
            PaymentHistory = -10, // Should be clamped to 0
            CreditUtilization = 50,
            AgeOfCreditHistory = 5
        };

        // Expected: (0.4 * 0) + (0.3 * (100 - 50)) + (0.3 * Min(5, 10))
        //          = 0 + 15 + 1.5 = 16.5 -> 17 (rounded)
        var expectedScore = 17;

        // Act
        var result = CreditScoreCalculator.CalculateCreditScore(customer);

        // Assert
        Assert.Equal(expectedScore, result);
    }

    [Fact]
    public void CalculateCreditScore_WithPaymentHistoryAbove100_ClampsTo100()
    {
        // Arrange
        var customer = new Customer
        {
            CustomerId = 1,
            Name = "Test",
            PaymentHistory = 150, // Should be clamped to 100
            CreditUtilization = 0,
            AgeOfCreditHistory = 5
        };

        // Expected: (0.4 * 100) + (0.3 * (100 - 0)) + (0.3 * Min(5, 10))
        //          = 40 + 30 + 1.5 = 71.5 -> 72 (rounded)
        var expectedScore = 72;

        // Act
        var result = CreditScoreCalculator.CalculateCreditScore(customer);

        // Assert
        Assert.Equal(expectedScore, result);
    }

    [Fact]
    public void CalculateCreditScore_WithNegativeAgeOfCreditHistory_ClampsToZero()
    {
        // Arrange
        var customer = new Customer
        {
            CustomerId = 1,
            Name = "Test",
            PaymentHistory = 80,
            CreditUtilization = 30,
            AgeOfCreditHistory = -5 // Should be clamped to 0
        };

        // Expected: (0.4 * 80) + (0.3 * (100 - 30)) + (0.3 * Min(0, 10))
        //          = 32 + 21 + 0 = 53
        var expectedScore = 53;

        // Act
        var result = CreditScoreCalculator.CalculateCreditScore(customer);

        // Assert
        Assert.Equal(expectedScore, result);
    }

    [Fact]
    public void CalculateCreditScore_WithNullCustomer_ThrowsArgumentNullException()
    {
        // Arrange
        Customer? customer = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => CreditScoreCalculator.CalculateCreditScore(customer!));
    }

    [Fact]
    public void CalculateCreditScore_WithScore49_IsHighRisk()
    {
        // Arrange
        var customer = new Customer
        {
            CustomerId = 1,
            Name = "Test",
            PaymentHistory = 50,
            CreditUtilization = 80,
            AgeOfCreditHistory = 0
        };

        // Act
        var score = CreditScoreCalculator.CalculateCreditScore(customer);
        var isHighRisk = RiskAssessment.IsHighRisk(score);

        // Assert
        Assert.True(score < 50);
        Assert.True(isHighRisk);
    }

    [Fact]
    public void CalculateCreditScore_WithScore50_IsLowRisk()
    {
        // Arrange
        // PaymentHistory = 88, CreditUtilization = 50, AgeOfCreditHistory = 0
        // = (0.4 * 88) + (0.3 * 50) + (0.3 * 0) = 35.2 + 15 + 0 = 50.2 -> 50
        var customer = new Customer
        {
            CustomerId = 1,
            Name = "Test",
            PaymentHistory = 88,
            CreditUtilization = 50,
            AgeOfCreditHistory = 0
        };

        // Act
        var score = CreditScoreCalculator.CalculateCreditScore(customer);
        var isHighRisk = RiskAssessment.IsHighRisk(score);

        // Assert
        Assert.True(score >= 50);
        Assert.False(isHighRisk);
    }

    [Fact]
    public void CalculateCreditScore_WithScore51_IsLowRisk()
    {
        // Arrange
        // PaymentHistory = 90, CreditUtilization = 50, AgeOfCreditHistory = 0
        // = (0.4 * 90) + (0.3 * 50) + (0.3 * 0) = 36 + 15 + 0 = 51
        var customer = new Customer
        {
            CustomerId = 1,
            Name = "Test",
            PaymentHistory = 90,
            CreditUtilization = 50,
            AgeOfCreditHistory = 0
        };

        // Act
        var score = CreditScoreCalculator.CalculateCreditScore(customer);
        var isHighRisk = RiskAssessment.IsHighRisk(score);

        // Assert
        Assert.True(score >= 50);
        Assert.False(isHighRisk);
    }
}

