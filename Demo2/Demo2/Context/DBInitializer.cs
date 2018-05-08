using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Demo2.Context
{
    public class DBInitializer : DropCreateDatabaseIfModelChanges<DatabaseContext>
    {

    }
}