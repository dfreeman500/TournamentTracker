using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary.Models
{
    public class MatchupModel
    {
        public int Id { get; set; }

        public List<MatchupEntryModel> Entries { get; set; } = new List<MatchupEntryModel>();
        /// <summary>
        /// ID from the database that will be used to identify the winner
        /// </summary>
        public int WinnerId { get; set; }

        public TeamModel Winner { get; set; }
        public int MatchupRound { get; set; }

        public string DisplayName //property - get with no set
        {
            get //figures out what the name should be
            {
                string output = "";

                foreach (MatchupEntryModel me in Entries)
                {
                    
                        if (me.TeamCompeting != null)
                        {
                            if (output.Length == 0) //checks to see if this is the first person being entered if it is - just put name
                            {
                                output = me.TeamCompeting.TeamName;
                            }
                            else
                            {
                                output += $" vs. { me.TeamCompeting.TeamName }";
                            }
                        }
                        else
                        {
                            output = "Matchup Not Yet Determined";
                            break; 
                        }
                   
                }
                return output;
            }
        }
        
    }

}
