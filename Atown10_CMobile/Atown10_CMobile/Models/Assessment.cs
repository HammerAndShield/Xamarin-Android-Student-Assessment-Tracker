using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace Atown10_CMobile.Models
{
    public class Assessment : Entity
    {
        public string Name { get; set; }
        public DateTime DueDate { get; set; }
        public string Type { get; set; } 
        public int CourseId { get; set; }
        public bool SetNotification { get; set; }
    }
}
