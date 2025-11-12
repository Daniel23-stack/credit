# Credit Risk Management Application

A .NET Console Application that evaluates customer credit scores and generates alerts for high-risk customers.

## Features

- **Credit Score Calculation**: Calculates credit scores using a weighted formula based on payment history, credit utilization, and age of credit history
- **Risk Assessment**: Identifies high-risk customers (credit score < 50)
- **Report Generation**: Generates detailed reports with customer names, credit scores, and risk status
- **JSON Support**: Reads customer data from JSON and saves reports to JSON
- **Unit Tests**: Comprehensive test coverage for credit score calculation

## Credit Score Formula

The credit score is calculated using the following formula:

```
CreditScore = (0.4 × PaymentHistory) + (0.3 × (100 - CreditUtilization)) + (0.3 × Min(AgeOfCreditHistory, 10))
```

Where:
- **PaymentHistory**: Percentage of payments made on time (0-100)
- **CreditUtilization**: Percentage of credit limit used (0-100)
- **AgeOfCreditHistory**: Age of credit history in years (capped at 10)

The score is rounded to the nearest integer.

## Risk Classification

- **High Risk**: Credit score < 50
- **Low Risk**: Credit score ≥ 50

## Prerequisites

- .NET 8 SDK or later
- Windows, macOS, or Linux

## Getting Started

### 1. Clone the Repository

```bash
git clone <repository-url>
cd credit
```

### 2. Restore Dependencies

```bash
dotnet restore
```

### 3. Build the Solution

```bash
dotnet build
```

### 4. Run the Application

```bash
cd CreditRiskApp
dotnet run
```

The application will:
1. Read customer data from `customers.json`
2. Calculate credit scores for each customer
3. Display a formatted report in the console
4. Save the report to `report.json`

### 5. Run Tests

```bash
dotnet test
```

## Input Format

The application expects a JSON file named `customers.json` in the application directory with the following structure:

```json
[
  {
    "CustomerId": 1,
    "Name": "Alice",
    "PaymentHistory": 90,
    "CreditUtilization": 40,
    "AgeOfCreditHistory": 5
  },
  {
    "CustomerId": 2,
    "Name": "Bob",
    "PaymentHistory": 70,
    "CreditUtilization": 90,
    "AgeOfCreditHistory": 15
  }
]
```

## Output Format

The application generates a `report.json` file with the following structure:

```json
[
  {
    "Name": "Alice",
    "CreditScore": 56,
    "RiskStatus": "Low Risk"
  },
  {
    "Name": "Bob",
    "CreditScore": 34,
    "RiskStatus": "High Risk"
  }
]
```

## Project Structure

```
credit/
├── CreditRiskApp/
│   ├── Models/
│   │   ├── Customer.cs
│   │   └── CustomerReport.cs
│   ├── Services/
│   │   ├── CreditScoreCalculator.cs
│   │   └── RiskAssessment.cs
│   ├── Program.cs
│   ├── customers.json
│   └── CreditRiskApp.csproj
├── CreditRiskApp.Tests/
│   ├── CreditScoreCalculatorTests.cs
│   └── CreditRiskApp.Tests.csproj
├── CreditRiskApp.sln
└── README.md
```

## Example Output

```
=== Credit Risk Management Report ===

Name                 Credit Score    Risk Status    
--------------------------------------------------
Alice                56              Low Risk       
Bob                  34              High Risk      
Charlie              48              High Risk      
--------------------------------------------------
Total Customers: 3
High Risk Customers: 2
Low Risk Customers: 1

Report saved to report.json
```

## Edge Cases Handled

- Negative values are clamped to valid ranges
- Values above 100 for percentages are clamped to 100
- Age of credit history above 10 is capped at 10
- Null customer validation
- Missing or invalid JSON file handling

## Testing

The test suite includes:
- Normal case calculations
- Edge cases (boundary values, negative numbers, out-of-range values)
- High-risk threshold testing (scores 49, 50, 51)
- Null argument validation

Run tests with:
```bash
dotnet test
```

## Technologies Used

- .NET 8
- C# 12
- System.Text.Json for JSON serialization
- xUnit for unit testing

## License

This project is provided as-is for evaluation purposes.

