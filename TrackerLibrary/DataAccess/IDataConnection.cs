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
        PrizeModel CreatePrize(PrizeModel model);
        PersonModel CreatePerson(PersonModel model);
        TeamModel CreateTeam(TeamModel model);

        TournamentModel CreateTournament(TournamentModel model);
        List<TeamModel> GetTeam_All(); //gets all teams and puts them in a list
        List<PersonModel> GetPerson_ALL(); //gets all of the persons

    }
}
