using System.Text;

public class Report
{
    public DateTime StartDate { get; }
    public DateTime EndDate { get; }
    public decimal TotalIncome { get; private set; }
    public decimal TotalExpense { get; private set; }

    // Изменяем тип ключа на string
    public Dictionary<string, CategoryReportData> CategoryData { get; }
        = new Dictionary<string, CategoryReportData>();

    public class CategoryReportData
    {
        public string CategoryName { get; set; }
        public decimal Income { get; set; }
        public decimal Expense { get; set; }
        public decimal BudgetLimit { get; set; }
        public decimal BudgetUsed { get; set; }
    }

    public Report(DateTime start, DateTime end)
    {
        StartDate = start;
        EndDate = end;
    }

    public void AddCategoryData(
        string categoryName,  // Принимаем string как ключ
        decimal income,
        decimal expense,
        decimal budgetLimit,
        decimal budgetUsed)
    {
        var key = categoryName.ToLowerInvariant();

        if (!CategoryData.ContainsKey(key))
        {
            CategoryData[key] = new CategoryReportData
            {
                CategoryName = categoryName,
                BudgetLimit = budgetLimit
            };
        }

        CategoryData[key].Income += income;
        CategoryData[key].Expense += expense;
        CategoryData[key].BudgetUsed += budgetUsed;

        TotalIncome += income;
        TotalExpense += expense;
    }

    public string GetFormattedReport()
    {
        var sb = new StringBuilder();
        sb.AppendLine($"===== REPORT: {StartDate:dd.MM.yyyy} - {EndDate:dd.MM.yyyy} =====");
        sb.AppendLine($"Incomes: {TotalIncome} BYN");
        sb.AppendLine($"Expenses: {TotalExpense} BYN");
        sb.AppendLine($"Balance: {TotalIncome - TotalExpense} BYN\n");

        sb.AppendLine("Details by category:");
        foreach (var entry in CategoryData.Values.OrderByDescending(x => x.Expense))
        {
            sb.AppendLine($"\n[{entry.CategoryName.ToUpper()}]");
            sb.AppendLine($"Incomes: {entry.Expense}");

            if (entry.BudgetLimit > 0)
            {
                decimal percent = entry.BudgetUsed / entry.BudgetLimit * 100;
                sb.AppendLine($"Budget used: {percent:0}% ({entry.BudgetUsed:C} / {entry.BudgetLimit:C})");
            }
        }

        return sb.ToString();
    }
}