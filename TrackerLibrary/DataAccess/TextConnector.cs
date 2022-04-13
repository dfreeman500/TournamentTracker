using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary.Models;
using TrackerLibrary.DataAccess.TextHelpers;

namespace TrackerLibrary.DataAccess
{
    public class TextConnector : IDataConnection
    {

        public void CreatePerson(PersonModel model)
        {
            List<PersonModel> people = GlobalConfig.PeopleFile.FullFilePath().LoadFile().ConvertToPersonModels(); //reads all of the people out of the csv file
            int currentId = 1;
            if (people.Count > 0)
            {
                currentId = people.OrderByDescending(x => x.Id).First().Id + 1; //finds max id and adds one to it
            }
            model.Id = currentId;
            people.Add(model);
            people.SaveToPeopleFile();
        }

        public void CreatePrize(PrizeModel model)
        {
            // load the text file and convert the text to list<PrizeModel>
            List<PrizeModel> prizes = GlobalConfig.PrizesFile.FullFilePath().LoadFile().ConvertToPrizeModels();

            //find the max ID
            int currentId = 1;
            if (prizes.Count > 0)
            {
                currentId = prizes.OrderByDescending(x => x.Id).First().Id + 1;
            }
            model.Id = currentId;
            //add the new record with the new id

            prizes.Add(model);


            // convert the prizes to a list<string>
            // save the list<string> to the text file
            prizes.SaveToPrizeFile();

        }

        public void CreateTeam(TeamModel model)
        {
            List<TeamModel> teams = GlobalConfig.TeamFile.FullFilePath().LoadFile().ConvertToTeamModels();

            //finds the max id
            int currentId = 1;
            if (teams.Count > 0)
            {
                currentId = teams.OrderByDescending(x => x.Id).First().Id + 1;

            }
            model.Id = currentId;
            teams.Add(model);
            teams.SaveToTeamFile();

        }

        public void CreateTournament(TournamentModel model)
        {
            List<TournamentModel> tournaments = GlobalConfig.TournamentFile
                .FullFilePath()
                .LoadFile()
                .ConvertToTournamentModels();

            int currentId = 1;

            if(tournaments.Count>0)
            {
                currentId = tournaments.OrderByDescending(x => x.Id).First().Id + 1;

            }
            model.Id = currentId;

            model.SaveRoundsToFile();

            tournaments.Add(model);

            tournaments.SaveToTournamentFile();

            TournamentLogic.UpdateTournamentResults(model); //updates the byes

        }

        public List<PersonModel> GetPerson_All()
        {
            return GlobalConfig.PeopleFile.FullFilePath().LoadFile().ConvertToPersonModels();
        }

        public List<TeamModel> GetTeam_All()
        {
            return GlobalConfig.TeamFile.FullFilePath().LoadFile().ConvertToTeamModels();
        }

        public List<TournamentModel> GetTournament_All()
        {
            return GlobalConfig.TournamentFile
                .FullFilePath()
                .LoadFile()
                .ConvertToTournamentModels();

        }

        public void UpdateMatchup(MatchupModel model)
        {
            model.UpdateMatchupToFile();
        }

        public void CompleteTournament(TournamentModel model)
        {
            //finds all tournament entries, removes one that matches the model,
            //saves the file (i.e. deletes that tournament

            List<TournamentModel> tournaments = GlobalConfig.TournamentFile
                .FullFilePath()
                .LoadFile()
                .ConvertToTournamentModels();

           

            tournaments.Remove(model); 

            tournaments.SaveToTournamentFile();

            TournamentLogic.UpdateTournamentResults(model); //updates the model
        }

    }
}
