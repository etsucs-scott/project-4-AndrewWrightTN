using Project4Web.Models;
using System.Text.Json;

namespace Project4Web.Services
{
    /// <summary>
    /// this stores the finance data that the user inputs
    /// </summary>
    public class FinanceManager
    {
        /// store transactions by date
        public Dictionary<DateTime, List<Transaction>> TransactionsByDate { get; set; } = new();

        /// <summary>
        /// add a transaction
        /// </summary>
        /// <param name="date">time of input</param>
        /// <param name="transaction">creates the transaction</param>
        /// <exception cref="ArgumentException">throws execption if negative transaction</exception>
        public void AddTransaction(DateTime date, Transaction transaction)
        {
            if (transaction.Amount < 0)
            {
                throw new ArgumentException("Transaction amount cannot be negative.");
            }

            if (!TransactionsByDate.ContainsKey(date))
            {
                TransactionsByDate[date] = new List<Transaction>();
            }
            TransactionsByDate[date].Add(transaction);
        }

        /// <summary>
        /// total income
        /// </summary>
        public decimal GetTotalIncome()
        {
            return TransactionsByDate.Values
                .SelectMany(t => t)
                .Where(t => t.IsIncome)
                .Sum(t => t.Amount);
        }

        /// <summary>
        /// total expenses
        /// </summary>
        public decimal GetTotalExpenses()
        {
            return TransactionsByDate.Values
                .SelectMany(t => t)
                .Where(t => !t.IsIncome)
                .Sum(t => t.Amount);
        }

        /// <summary>
        /// net savings
        /// </summary>
        public decimal GetNetSavings()
        {
            return GetTotalIncome() - GetTotalExpenses();
        }

        /// <summary>
        /// save transactions to file
        /// </summary>
        public void SaveTransactionsToFile(string filePath)
        {
            try
            {
                string json = JsonSerializer.Serialize(TransactionsByDate);
                File.WriteAllText(filePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving to file: {ex.Message}");
            }
        }

        /// <summary>
        /// load transactions from file
        /// </summary>
        public void LoadTransactionsFromFile(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    string json = File.ReadAllText(filePath);
                    TransactionsByDate = JsonSerializer.Deserialize<Dictionary<DateTime, List<Transaction>>>(json)
                                         ?? new Dictionary<DateTime, List<Transaction>>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading from file: {ex.Message}");
            }
        }
    }
}
