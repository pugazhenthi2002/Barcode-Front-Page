using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportFrontPage
{
    public static class DatabaseManager
    {
        public static List<string> PrefixCollection;    //For ComboBox DataBinding

        private static MyDBContext dbContext;
        
        static DatabaseManager()
        {
            dbContext = new MyDBContext();
            dbContext.Database.EnsureCreated();

            PrefixCollection = new List<string>();
            PrefixCollection = dbContext.ReportsProfile.Select(r1 => r1.Prefix).Distinct().ToList();
        }

        public static void AddOrUpdateReport(ReportProfile report)
        {
            ReportProfile result = dbContext.ReportsProfile.Find(report.ID);
            if(result == null)
            {
                dbContext.ReportsProfile.Add(report);

                if(PrefixCollection.Find(r1 => r1 == report.Prefix) != "")  // If Newly added Prefix is not presented in the Collection
                    PrefixCollection.Add(report.Prefix);
            }
            else
            {
                result.ReportName = report.ReportName;
                result.Prefix = report.Prefix;
                result.OperatorName = report.OperatorName;
                result.IsVerified = report.IsVerified;
            }

            dbContext.SaveChanges();
        }

        public static void DeleteReport(ReportProfile report)
        {
            ReportProfile result = dbContext.ReportsProfile.Find(report.ID);
            if (result != null)
            {
                if(dbContext.ReportsProfile.Where(r1=>r1.Prefix == report.Prefix).Count() == 1) //If Prefix Count is equals 1
                {
                    PrefixCollection.Remove(report.Prefix);
                }

                dbContext.ReportsProfile.Remove(report);
            }

            dbContext.SaveChanges();
        }

        public static void FetchReport()
        {

        }
    }
}
