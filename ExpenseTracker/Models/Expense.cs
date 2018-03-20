using System;

namespace ExpenseTracker.Models
{
    public class Expense : BaseModel
    {
        public string Description { get; set; }
        public double Amount { get; set; }
        public DateTime DateTime { get; set; }
    }
}