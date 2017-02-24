using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Events.Data_Access
{
    public class DataFactory
    {
        public static string AccessMode = "Firebase";
        public static IDataAccess DataAccessFactory()
        {
            if (AccessMode == "Firebase")
            {
                return new FirebaseDataAccess();
            }
            else
            {
                return new OfflineDataAccess();
            }
        }
    }
}
