using System;
using System.Collections.Generic;
using System.Text;

namespace Atown10_CMobile.Models
{
    public class Term : Entity
    {
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

}
