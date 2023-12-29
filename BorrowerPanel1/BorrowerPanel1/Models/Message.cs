using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace BorrowerPanel1.Models
{
    public partial class Message
    {
        public int MessageId { get; set; }
        public int BurrowerId { get; set; }
        public string Message1 { get; set; }

        public virtual BorrowerTb Burrower { get; set; }
    }
}
