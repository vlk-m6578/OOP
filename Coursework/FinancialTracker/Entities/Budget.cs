using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTracker.Entities
{
    public class Budget
    {
        public int CategoryId { get; }
        public decimal CurrentSpending { get; private set; }
        public DateTime Month {  get; }
        public decimal Limit { get; }
        public Budget(int categoryId, decimal limit) 
        {
            CategoryId = categoryId;
            Limit = limit;
            Month = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        }
        public void UpdateSpending(decimal amount) => CurrentSpending += amount;
        public bool isLimitReached() => CurrentSpending >= Limit;
        public bool IsWarningThresholdReached() => CurrentSpending >= Limit * 0.8m;
    }
}
