using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace LenderPanel.Models
{
    public partial class LenderTb
    {
        public LenderTb()
        {
            Instruments = new HashSet<Instruments>();
        }

        public int LenderId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailId { get; set; }
        public int? ContactNumber { get; set; }
        public string Address { get; set; }
        public string Password { get; set; }

        public virtual ICollection<Instruments> Instruments { get; set; }
    }
}
