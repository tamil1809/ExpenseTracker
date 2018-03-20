using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace ExpenseTracker.Models
{
    public abstract class BaseModel
    {
        [AutoIncrement]
        public long Id { get; set; }
    }
}