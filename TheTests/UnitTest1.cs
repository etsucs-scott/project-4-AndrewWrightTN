namespace TheTests;
using Project4Web.Services;
using Project4Web.Models;


    public class UnitTest1
    {
        
        /// <summary>
        /// Unit tests for the FinanceManager service.
        /// </summary>
        public class FinanceManagerTests
        {
            [Fact]
            public void AddTransaction_AddsToCorrectDate()
            {
                // Arrange
                FinanceManager manager = new FinanceManager();
                DateTime date = new DateTime(2024, 1, 1);
                Transaction transaction = new Transaction { Description = "Test", Amount = 100, IsIncome = true };

                // Act
                manager.AddTransaction(date, transaction);

                // Assert
                Assert.True(manager.TransactionsByDate.ContainsKey(date));
            }

            [Fact]
            public void AddTransaction_TransactionIsStored()
            {
                // Arrange
                FinanceManager manager = new FinanceManager();
                DateTime date = new DateTime(2024, 1, 1);
                Transaction transaction = new Transaction { Description = "Salary", Amount = 500, IsIncome = true };

                // Act
                manager.AddTransaction(date, transaction);

                // Assert
                Assert.Contains(transaction, manager.TransactionsByDate[date]);
            }

            [Fact]
            public void AddTransaction_NegativeAmount_ThrowsException()
            {
                // Arrange
                FinanceManager manager = new FinanceManager();
                Transaction transaction = new Transaction { Description = "Bad", Amount = -50, IsIncome = true };

                // Act & Assert
                Assert.Throws<ArgumentException>(() => manager.AddTransaction(DateTime.Today, transaction));
            }

            [Fact]
            public void AddTransaction_MultipleOnSameDate_AllStored()
            {
                // Arrange
                FinanceManager manager = new FinanceManager();
                DateTime date = new DateTime(2024, 1, 1);

                // Act
                manager.AddTransaction(date, new Transaction { Description = "Job", Amount = 200, IsIncome = true });
                manager.AddTransaction(date, new Transaction { Description = "Groceries", Amount = 50, IsIncome = false });

                // Assert
                Assert.Equal(2, manager.TransactionsByDate[date].Count);
            }

            [Fact]
            public void GetTotalIncome_ReturnsCorrectSum()
            {
                // Arrange
                FinanceManager manager = new FinanceManager();
                manager.AddTransaction(DateTime.Today, new Transaction { Amount = 300, IsIncome = true });
                manager.AddTransaction(DateTime.Today, new Transaction { Amount = 200, IsIncome = true });

                // Act
                decimal total = manager.GetTotalIncome();

                // Assert
                Assert.Equal(500, total);
            }

            [Fact]
            public void GetTotalIncome_DoesNotCountExpenses()
            {
                // Arrange
                FinanceManager manager = new FinanceManager();
                manager.AddTransaction(DateTime.Today, new Transaction { Amount = 300, IsIncome = true });
                manager.AddTransaction(DateTime.Today, new Transaction { Amount = 100, IsIncome = false });

                // Act
                decimal total = manager.GetTotalIncome();

                // Assert
                Assert.Equal(300, total);
            }

            [Fact]
            public void GetTotalExpenses_ReturnsCorrectSum()
            {
                // Arrange
                FinanceManager manager = new FinanceManager();
                manager.AddTransaction(DateTime.Today, new Transaction { Amount = 80, IsIncome = false });
                manager.AddTransaction(DateTime.Today, new Transaction { Amount = 20, IsIncome = false });

                // Act
                decimal total = manager.GetTotalExpenses();

                // Assert
                Assert.Equal(100, total);
            }

            [Fact]
            public void GetTotalExpenses_DoesNotCountIncome()
            {
                // Arrange
                FinanceManager manager = new FinanceManager();
                manager.AddTransaction(DateTime.Today, new Transaction { Amount = 500, IsIncome = true });
                manager.AddTransaction(DateTime.Today, new Transaction { Amount = 75, IsIncome = false });

                // Act
                decimal total = manager.GetTotalExpenses();

                // Assert
                Assert.Equal(75, total);
            }

            [Fact]
            public void GetNetSavings_ReturnsIncomeMinusExpenses()
            {
                // Arrange
                FinanceManager manager = new FinanceManager();
                manager.AddTransaction(DateTime.Today, new Transaction { Amount = 500, IsIncome = true });
                manager.AddTransaction(DateTime.Today, new Transaction { Amount = 200, IsIncome = false });

                // Act
                decimal savings = manager.GetNetSavings();

                // Assert
                Assert.Equal(300, savings);
            }

            [Fact]
            public void GetNetSavings_NegativeWhenExpensesExceedIncome()
            {
                // Arrange
                FinanceManager manager = new FinanceManager();
                manager.AddTransaction(DateTime.Today, new Transaction { Amount = 100, IsIncome = true });
                manager.AddTransaction(DateTime.Today, new Transaction { Amount = 300, IsIncome = false });

                // Act
                decimal savings = manager.GetNetSavings();

                // Assert
                Assert.Equal(-200, savings);
            }

            [Fact]
            public void GetTotalIncome_NoTransactions_ReturnsZero()
            {
                // Arrange
                FinanceManager manager = new FinanceManager();

                // Act
                decimal total = manager.GetTotalIncome();

                // Assert
                Assert.Equal(0, total);
            }

            [Fact]
            public void SaveAndLoad_PreservesTransactions()
            {
                // Arrange
                FinanceManager manager = new FinanceManager();
                DateTime date = new DateTime(2024, 6, 15);
                manager.AddTransaction(date, new Transaction { Description = "Salary", Amount = 250, IsIncome = true });
                string tempFile = Path.GetTempFileName();

                // Act
                manager.SaveTransactionsToFile(tempFile);
                FinanceManager loaded = new FinanceManager();
                loaded.LoadTransactionsFromFile(tempFile);

                // Assert
                Assert.Equal(250, loaded.GetTotalIncome());

                // Cleanup
                File.Delete(tempFile);
            }

            [Fact]
            public void LoadTransactions_FileNotFound_DoesNotThrow()
            {
                // Arrange
                FinanceManager manager = new FinanceManager();

                // Act
                Exception ex = Record.Exception(() => manager.LoadTransactionsFromFile("fake_file.json"));

                // Assert
                Assert.Null(ex);
            }
        }
    }
