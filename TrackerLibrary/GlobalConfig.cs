using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary.DataAccess;

namespace TrackerLibrary
{
    //static public classes always visible and can't instantiate
    //Putting data here because we actually want global data
    
    public static class GlobalConfig
    {
        //List<IDataConnection> allows to save to both text file and dB
        public static IDataConnection Connection { get; private set; }  //anyone can read but not everyone can set
        
        public static void InitializeConnections(DatabaseType db)
        {
            if (db == DatabaseType.Sql)
            {
                SqlConnector sql = new SqlConnector();
                Connection = sql;
                
            }

            else if (db == DatabaseType.TextFile)
            {
                TextConnector text = new TextConnector();
                Connection = text;
                
                
            }

        }
        public static string CnnString (string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }
    
    }
}
