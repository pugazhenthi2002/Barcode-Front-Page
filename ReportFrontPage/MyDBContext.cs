using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using System.Xml;

namespace ReportFrontPage
{
    public class MyDBContext : DbContext
    {
        public MyDBContext() : base() { }
        public DbSet<ReportProfile> ReportsProfile { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            MySqlConnection conn;
            conn = new MySqlConnection();
            conn.ConnectionString = "server=localhost;uid=root;pwd=Irpt@1110;database=mydatabase";

            optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
            optionsBuilder.UseMySQL(conn);
        }

        
    }
}
