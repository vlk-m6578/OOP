using FinancialTracker.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTracker.Entities
{
    public class Report : IReportService
    {
        private static int _lastId = 0;
        public int Id { get;}
        public DateTime StartDate { get; }
        public DateTime EndDate { get; }
        public Dictionary<int, CategoryTotal> CategoryTotals { get; } = new Dictionary<int, CategoryTotal>();
        public Report(DateTime startDate, DateTime endDate) 
        {
            Id = ++_lastId;
            StartDate = startDate;
            EndDate = endDate;
        }
        public void AddCategoryData(int categoryId, decimal income, decimal expense)
        {
            if (!CategoryTotals.ContainsKey(categoryId))
            {
                CategoryTotals[categoryId] = new CategoryTotal();
            }
            CategoryTotals[categoryId].TotalIncome += income;
            CategoryTotals[categoryId].TotalExpense += expense;
        }
        public Report GenerateFinancialReport(DateTime startDate, DateTime endDate)
        {
            return this;
        }

        public void Generate()
        {
            Console.WriteLine("================================================================ REPORT ==============================================================");
            Console.WriteLine($"Period: {StartDate:d} - {EndDate:d}");

            decimal totalIncome = 0;
            decimal totalExpense = 0;

            foreach(var entry in CategoryTotals)
            {
                totalIncome += entry.Value.TotalIncome;
                totalExpense += entry.Value.TotalExpense;
                Console.WriteLine($"{GetCategoryName(entry.Key)}: " +
                                  $"Income {entry.Value.TotalIncome:C}, " +
                                  $"Expense {entry.Value.TotalExpense:C}");
            }
            Console.WriteLine("\nSUMMARY: ");
            Console.WriteLine($"Total Income: {totalIncome:C}");
            Console.WriteLine($"Total Expenses: {totalExpense:C}");
            Console.WriteLine($"Net Balance: {totalIncome - totalExpense:C}");
        }
        private string GetCategoryName(int categoryId)
        {
            return "Category name";
        }

        public class CategoryTotal
        {
            public decimal TotalIncome { get; set; }
            public decimal TotalExpense { get; set; }
        }
    }
}
