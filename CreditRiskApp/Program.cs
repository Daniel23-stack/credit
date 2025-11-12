using System.Text.Json;
using CreditRiskApp.Models;
using CreditRiskApp.Services;

namespace CreditRiskApp;

/// <summary>
/// Main entry point for the Credit Risk Management Application.
/// </summary>
class Program
{
    private const string InputFileName = "customers.json";
    private const string OutputFileName = "report.json";

    static void Main(string[] args)
    {
        try
        {
            // Read customer data from JSON file
            var customers = ReadCustomersFromFile(InputFileName);

            if (customers == null || customers.Count == 0)
            {
                Console.WriteLine("No customers found in the input file.");
                return;
            }

            // Calculate credit scores and generate reports
            var reports = GenerateReports(customers);

            // Display report to console
            DisplayReportToConsole(reports);

            // Save report to JSON file
            SaveReportToFile(reports, OutputFileName);

            Console.WriteLine($"\nReport saved to {OutputFileName}");
        }
        catch (FileNotFoundException ex)
        {
            Console.WriteLine($"Error: File not found - {ex.Message}");
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"Error: Invalid JSON format - {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    /// <summary>
    /// Reads customer data from a JSON file.
    /// </summary>
    /// <param name="fileName">The name of the JSON file to read.</param>
    /// <returns>List of customers.</returns>
    private static List<Customer> ReadCustomersFromFile(string fileName)
    {
        // Try to find the file in the current directory or application base directory
        string filePath = fileName;
        if (!File.Exists(filePath))
        {
            // Try in the application's base directory
            string baseDirectory = AppContext.BaseDirectory;
            filePath = Path.Combine(baseDirectory, fileName);
            
            if (!File.Exists(filePath))
            {
                // Try in the parent directory (for when running from bin/Debug/netX.0)
                string parentDirectory = Directory.GetParent(baseDirectory)?.Parent?.Parent?.FullName ?? "";
                filePath = Path.Combine(parentDirectory, fileName);
                
                if (!File.Exists(filePath))
                {
                    throw new FileNotFoundException($"Input file '{fileName}' not found in current directory, application directory, or project directory.");
                }
            }
        }

        var jsonContent = File.ReadAllText(filePath);
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var customers = JsonSerializer.Deserialize<List<Customer>>(jsonContent, options);

        return customers ?? new List<Customer>();
    }

    /// <summary>
    /// Generates credit reports for all customers.
    /// </summary>
    /// <param name="customers">List of customers to process.</param>
    /// <returns>List of customer reports.</returns>
    private static List<CustomerReport> GenerateReports(List<Customer> customers)
    {
        var reports = new List<CustomerReport>();

        foreach (var customer in customers)
        {
            var creditScore = CreditScoreCalculator.CalculateCreditScore(customer);
            var riskStatus = RiskAssessment.GetRiskStatus(creditScore);

            reports.Add(new CustomerReport
            {
                Name = customer.Name,
                CreditScore = creditScore,
                RiskStatus = riskStatus
            });
        }

        return reports;
    }

    /// <summary>
    /// Displays the credit report to the console in a formatted table.
    /// </summary>
    /// <param name="reports">List of customer reports to display.</param>
    private static void DisplayReportToConsole(List<CustomerReport> reports)
    {
        Console.WriteLine("\n=== Credit Risk Management Report ===\n");
        Console.WriteLine($"{"Name",-20} {"Credit Score",-15} {"Risk Status",-15}");
        Console.WriteLine(new string('-', 50));

        foreach (var report in reports)
        {
            Console.WriteLine($"{report.Name,-20} {report.CreditScore,-15} {report.RiskStatus,-15}");
        }

        Console.WriteLine(new string('-', 50));
        Console.WriteLine($"Total Customers: {reports.Count}");
        Console.WriteLine($"High Risk Customers: {reports.Count(r => r.RiskStatus == "High Risk")}");
        Console.WriteLine($"Low Risk Customers: {reports.Count(r => r.RiskStatus == "Low Risk")}");
    }

    /// <summary>
    /// Saves the credit report to a JSON file.
    /// </summary>
    /// <param name="reports">List of customer reports to save.</param>
    /// <param name="fileName">The name of the output JSON file.</param>
    private static void SaveReportToFile(List<CustomerReport> reports, string fileName)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        var jsonContent = JsonSerializer.Serialize(reports, options);
        File.WriteAllText(fileName, jsonContent);
    }
}
