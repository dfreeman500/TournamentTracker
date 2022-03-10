using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary
{
    //static public classes always visible and can't instantiate
    //Putting data here because we actually want global data
    
    public static class GlobalConfig
    {
        //List<IDataConnection> allows to save to both text file and dB
        public static List<IDataConnection> Connections  { get; private set; } //anyone can read but not everyone can set
        
        public static void InitializeConnections(bool database, bool textFiles)
        {
            if (database)
            {
                //TODO - create SQL connection
            }

            if (textFiles)
            {
                //TODO Create the text connection
            }


            //TODO - 4:37:43, 5:14:32
            //TODO currently at 3:43:37

        }
    
    }
}
