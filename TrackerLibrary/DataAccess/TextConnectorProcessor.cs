﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary.Models;

namespace TrackerLibrary.DataAccess.TextHelpers
{
    public static class TextConnectorProcessor
    {
        public static string FullFilePath(this string fileName)  //becomes an extension method
        {
            //C:\data\TournamentTracker\PrizeModels.csv
            var p = $"{ConfigurationManager.AppSettings["filePath"] }\\{ fileName }";
            return p;

        }
        public static List<string> LoadFile(this string file) // Takes in full file path, and reads all of the lines in the file
        {
            bool doesFileExist = File.Exists(file);
            if (File.Exists(file) == false) //if file doesn't exist...
            {
                return new List<string>();
            }
            return File.ReadAllLines(file).ToList();
        }
        public static List<PrizeModel> ConvertToPrizeModels(this List<string> lines)
        {
            List<PrizeModel> output = new List<PrizeModel>();

            foreach (string line in lines)
            {
                string[] cols = line.Split(',');

                PrizeModel p = new PrizeModel();
                p.Id = int.Parse(cols[0]);
                p.PlaceNumber = int.Parse(cols[1]);
                p.PlaceName = cols[2];
                p.PrizeAmount = decimal.Parse(cols[3]);
                p.PrizePercentage = double.Parse(cols[4]);
                output.Add(p);
            }
            return output;

        }

        public static List<PersonModel> ConvertToPersonModels(this List<string> lines)
        {
            List<PersonModel> output = new List<PersonModel>();
            foreach (string line in lines)
            {
                string[] cols = line.Split(',');
                PersonModel p = new PersonModel();
                p.Id = int.Parse(cols[0]);
                p.FirstName = cols[1];
                p.LastName = cols[2];
                p.EmailAddress = cols[3];
                p.CellphoneNumber = cols[4];
                output.Add(p);
            }
            return output;
        }

        public static List<TeamModel> ConvertToTeamModels(this List<string> lines, string peopleFileName)
        {
            //id, team name, list of person, list of ids separated by the pipe
            // 3, Tim's team, 1 | 3|5
            List<TeamModel> output = new List<TeamModel>();
            List<PersonModel> people = peopleFileName.FullFilePath().LoadFile().ConvertToPersonModels(); //reads all of the people out of the csv file

            foreach (string line in lines)
            {
                string[] cols = line.Split(',');
                TeamModel t = new TeamModel();
                t.Id = int.Parse(cols[0]);
                t.TeamName = cols[1];

                string[] personIds = cols[2].Split('|'); //takes 3rd column, splits it by pipe and puts in array

                foreach (string id in personIds)
                {
                    t.TeamMembers.Add(people.Where(x => x.Id == int.Parse(id)).First()); //takes list of all people in text file and searches for where id of person in the list equals id of a specific person

                }
                output.Add(t);

            }
            return output;
        }
        /// <summary>
        /// 
        ///     id=0
        ///     TournamentName = 1
        ///     EntryFee = 2
        ///     EnteredTeams= 3
        ///     Prizes = 4
        ///     Rounds = 5
        ///     
        /// </summary>
        /// <param name="lines"></param>
        /// <returns></returns>
        public static List<TournamentModel> ConvertToTournamentModels(
            this List<string> lines, 
            string teamFileName, 
            string peopleFileName,
            string prizesFileName)
        {
            //id,TournamentName, EntryFee, (id|id|id - Entered Teams),(Rounds - id^id^id|id^id^id|id^id^id)

            List<TournamentModel> output = new List<TournamentModel>();
            List<TeamModel> teams = teamFileName.FullFilePath().LoadFile().ConvertToTeamModels(peopleFileName);
            List<PrizeModel> prizes = prizesFileName.FullFilePath().LoadFile().ConvertToPrizeModels();

            foreach (string line in lines)
            {
                string[] cols = line.Split(',');
                TournamentModel tm = new TournamentModel();
                tm.Id = int.Parse(cols[0]);
                tm.TournamentName = cols[1];
                tm.EntryFee = decimal.Parse(cols[2]);

                string[] teamIds = cols[3].Split('|');

                foreach (string id in teamIds)
                {
                    //Looks through list of teams and finds the team with the id that matches, if found takes the first one puts that into EnteredTeams
                    tm.EnteredTeams.Add(teams.Where(x => x.Id == int.Parse(id)).First());

                }

                string[] prizeIds = cols[4].Split('|');

                foreach (string id in prizeIds)
                {
                    //Looks through list of Prizes and finds the prize with the id that matches, if found takes the first one puts that into Prizes

                    tm.Prizes.Add(prizes.Where(x => x.Id == int.Parse(id)).First());
                }

                //TODO -capture Rounds information

                output.Add(tm);

            }
            return output;
        }


        public static void SaveToPrizeFile(this List<PrizeModel> models, string fileName)
        {
            List<string> lines = new List<string>();
            foreach (PrizeModel p in models)
            {
                lines.Add($"{p.Id}, {p.PlaceNumber}, {p.PlaceName}, {p.PrizeAmount}, {p.PrizePercentage}");

            }
            File.WriteAllLines(fileName.FullFilePath(), lines); //creates file -- but does not create path
        }

        public static void SaveToPeopleFile(this List<PersonModel> models, string filename)
        {
            List<string> lines = new List<string>();
            foreach (PersonModel p in models)
            {
                lines.Add($"{p.Id},{p.FirstName},{p.LastName},{p.EmailAddress},{p.CellphoneNumber}");
            }
            File.WriteAllLines(filename.FullFilePath(), lines);
        }

        public static void SaveRoundsToFile(this TournamentModel model, string matchupFile, string matchupEntryFile)
        {
            // loop through each round
            // loop through each matchup
            // save the id for the new matchup and save the record
            // loop through each entry, get the id, and save it

            foreach (List<MatchupModel> round in model.Rounds)
            {
                foreach (MatchupModel matchup in round)
                {
                    //Load all of the matchups from file
                    //get top id and add one
                    //store id
                    //save the matchup record
                    matchup.SaveMatchupToFile(matchupFile, matchupEntryFile);

                }
            }

        }

        private static List<MatchupEntryModel> ConvertStringToMatchupEntryModels(string input)
        {
            throw new NotImplementedException();
        }

        private static TeamModel LookupTeamById(int Id, string teamFile, string peopleFile)
        {
            List<TeamModel> teams = teamFile.FullFilePath().LoadFile().ConvertToTeamModels(peopleFile);
            throw new NotImplementedException();

        }

        public static List<MatchupModel> ConvertToMatchupModels(this List<string> lines)
        {
            //id=0
            //entries=1 (pipe delimited)
            // winner = 2
            //matchupRound=3


            List<MatchupModel> output = new List<MatchupModel>();

            foreach (string line in lines)
            {
                string[] cols = line.Split(',');

                MatchupModel p = new MatchupModel();
                p.Id = int.Parse(cols[0]);
                p.Entries = ConvertStringToMatchupEntryModels(cols[1]);
                p.Winner = LookupTeamById(int.Parse(cols[2]));
                p.MatchupRound = int.Parse(cols[3]);
                output.Add(p);
            }
            return output;

        }





        public static void SaveMatchupToFile(this MatchupModel matchup, string matchupFile, string matchupEntryFile)
        {


            foreach (MatchupEntryModel entry in matchup.Entries)
            {
                entry.SaveEntryToFile(matchupEntryFile);
            }
        }

        public static void SaveEntryToFile(this MatchupEntryModel entry, string matchupEntryFile)
        {

        }

        public static void SaveToTeamFile(this List<TeamModel> models, string fileName)
        {
            List<string> lines = new List<string>();
            foreach (TeamModel t in models)
            {
                lines.Add($"{t.Id},{t.TeamName},{ConvertPeopleListToString(t.TeamMembers)}");
            }
            File.WriteAllLines(fileName.FullFilePath(), lines);

        }


        //     id=0
        //     TournamentName = 1
        //     EntryFee = 2
        //     EnteredTeams= 3
        //     Prizes = 4
        //     Rounds = 5
        //     (Rounds - id^id^id|id^id^id|id^id^id)

        public static void SaveToTournamentFile(this List<TournamentModel> models, string fileName )
        {
            List<string> lines = new List<string>();

            foreach (TournamentModel tm in models)
            {
                lines.Add($@"{ tm.Id }, 
                    { tm.TournamentName }, 
                    { tm.EntryFee }, 
                    { ConvertTeamListToString(tm.EnteredTeams) }, 
                    { ConvertPrizeListToString(tm.Prizes) },
                    { ConvertRoundListToString(tm.Rounds) }"); 
            }
            File.WriteAllLines(fileName.FullFilePath(), lines);

        }


        private static string ConvertRoundListToString(List<List<MatchupModel>> rounds)
        {
            //     (Rounds - id^id^id|id^id^id|id^id^id)

            string output = "";

            if (rounds.Count == 0)
            {
                return "";
            }
            foreach (List<MatchupModel> r in rounds)
            {
                output += $"{ConvertMatchupListToString(r)}|";
            }
            output = output.Substring(0, output.Length - 1); // removes last pipe from string
            return output;



        }

        private static string ConvertMatchupListToString(List<MatchupModel> matchups)
        {
            string output = "";

            if (matchups.Count == 0)
            {
                return "";
            }
            foreach (MatchupModel m in matchups)
            {
                output += $"{m.Id}^";
            }
            output = output.Substring(0, output.Length - 1); // removes last pipe from string
            return output;

        }



        private static string ConvertPrizeListToString(List<PrizeModel> prizes)
        {
            string output = "";

            if (prizes.Count == 0)
            {
                return "";
            }
            foreach (PrizeModel p in prizes)
            {
                output += $"{p.Id}|";
            }
            output = output.Substring(0, output.Length - 1); // removes last pipe from string
            return output;



        }

        private static string ConvertTeamListToString(List<TeamModel> teams)
        {
            string output = "";

            if (teams.Count == 0)
            {
                return "";
            }
            foreach (TeamModel t in teams)
            {
                output += $"{t.Id}|";
            }
            output = output.Substring(0, output.Length - 1); // removes last pipe from string
            return output;



        }

        private static string ConvertPeopleListToString(List<PersonModel> people)
        {
            string output = "";

            if (people.Count == 0)
            {
                return "";
            }
            foreach (PersonModel p in people)
            {
                output += $"{p.Id}|";
            }
            output = output.Substring(0, output.Length - 1); // removes last pipe from string
            return output;

        }
    }
}
