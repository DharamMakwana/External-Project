using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace FINAL.Models
{
    public partial class Transactions
    {
        public int TransactionId { get; set; }
        public int ReqId { get; set; }
        public int LenderId { get; set; }
        public int BorrowerId { get; set; }
        public DateTime? TransactionDate { get; set; }
        public int? Amount { get; set; }

        public virtual BorrowerTb Borrower { get; set; }
    }
}
