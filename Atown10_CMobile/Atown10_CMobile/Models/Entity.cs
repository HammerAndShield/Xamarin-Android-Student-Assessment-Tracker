using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace Atown10_CMobile.Models
{
    class Entity
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
    }
}
