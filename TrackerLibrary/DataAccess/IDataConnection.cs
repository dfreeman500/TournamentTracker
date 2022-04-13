using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary.Models;

namespace TrackerLibrary.DataAccess

{
    public interface IDataConnection

        //contract specifies what methods /properties need to be here
        // in a contract you only have public items - doesn't make sense to have hidden contract terms

    {
        void CreatePrize(PrizeModel model);
        void CreatePerson(PersonModel model);
        void CreateTeam(TeamModel model);

        void CreateTournament(TournamentModel model);

        void UpdateMatchup(MatchupModel model);

        void CompleteTournament(TournamentModel model);

        List<TeamModel> GetTeam_All(); //gets all teams and puts them in a list
        List<PersonModel> GetPerson_All(); //gets all of the persons
        List<TournamentModel> GetTournament_All();

    }
}
