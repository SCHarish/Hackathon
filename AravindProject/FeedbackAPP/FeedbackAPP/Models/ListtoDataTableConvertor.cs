using Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using FeedbackAPP.Models;
namespace FeedbackAPP.Models
{
    public class ListtoDataTableConverter
    {
        public DataTable ToDataTable<Entities>(List<Entities> items)
        {
            System.Data.DataTable dataTable = new System.Data.DataTable(typeof(Entities).Name);
          //  DataTable dataTable = new DataTable(typeof(T).Name);
            //Get all the properties
            PropertyInfo[] Props = typeof(Entities).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }

            foreach (Entities item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }

                dataTable.Rows.Add(values);

            }
            //put a breakpoint here and check datatable
            return (Excel.DataTable)dataTable;
        }
    }
}