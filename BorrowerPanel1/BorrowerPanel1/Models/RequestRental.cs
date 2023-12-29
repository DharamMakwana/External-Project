using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace BorrowerPanel1.Models
{
    public partial class RequestRental
    {
        public int ReqId { get; set; }
        public int BorrowerId { get; set; }
        public int InstrumentId { get; set; }
        public DateTime? RequestDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Experience { get; set; }
        public string Status { get; set; }
        public int? TotalAmount { get; set; }

        public virtual Instruments Instrument { get; set; }
        public virtual BorrowerTb Borrower { get; internal set; }
    }
}
