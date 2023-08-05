using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace Atown10_CMobile.Services
{
    public interface IDatabaseConnection
    {
        SQLiteAsyncConnection DbConnection();
    }
}
