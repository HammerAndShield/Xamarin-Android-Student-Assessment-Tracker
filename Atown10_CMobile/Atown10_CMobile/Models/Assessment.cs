using System;
using System.Collections.Generic;
using System.Text;

namespace Atown10_CMobile.Models
{
    public class Assessment : Entity
    {
        public string Name { get; set; }
        public DateTime DueDate { get; set; }
        public string Type { get; set; }
        public int CourseId { get; set; }
    }
}
