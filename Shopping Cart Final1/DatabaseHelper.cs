using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shopping_Cart_Final1
{
    internal class DatabaseHelper
    {
        // *** កែ Server Name, Database Name ឲ្យត្រូវជាមួយ SQL Server របស់អ្នក ***
        private static readonly string ConnectionString =
            "Server=DESKTOP-RP8M6HR\\SQLEXPRESS;Database=ShoppingCartDB;Trusted_Connection=True;TrustServerCertificate=True;";

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }
    }
}
