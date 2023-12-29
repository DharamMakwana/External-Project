using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace BorrowerPanel1.Models
{
    public partial class Instruments
    {
        public Instruments()
        {
            RequestRental = new HashSet<RequestRental>();
        }

        public int InstrumentId { get; set; }
        public string InstrumentName { get; set; }
        public int LenderId { get; set; }
        public string Description { get; set; }
        public string AvailabilityStatus { get; set; }
        public string Image { get; set; }
        public int? RentAmount { get; set; }

        public virtual LenderTb Lender { get; set; }
        public virtual ICollection<RequestRental> RequestRental { get; set; }
    }
}
