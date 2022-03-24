﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary.Models;

namespace TrackerLibrary
{
    public static class TournamentLogic
    {
        // Order lists randomly of teams
        // check if it is big enough - if not add in Byes
        // create first riybd if matchups
        // create every round after that. 8 - 4 - 2 - 1
        // 

        public static void CreateRounds(TournamentModel model) 
        {
            List<TeamModel> randomizedTeams = RandomizeTeamOrder(model.EnteredTeams);
            int rounds = findNumberOfRounds(randomizedTeams.Count);
            int byes = numberOfByes(rounds, randomizedTeams.Count); ;

            model.Rounds.Add(CreateFirstRound(byes, randomizedTeams));
            CreateOtherRounds(model, rounds);
        }

        private static void CreateOtherRounds(TournamentModel model, int rounds) //TournamentModel model gets populated with all of the sets of rounds
        {
            int round = 2; //already created the first round
            List<MatchupModel> previousRound = model.Rounds[0];
            List<MatchupModel> currRound = new List<MatchupModel>();
            MatchupModel currMatchup = new MatchupModel();



            while (round <= rounds) //round - current round, rounds - total number of rounds
            {
                foreach (MatchupModel match in previousRound)
                {
                    currMatchup.Entries.Add(new MatchupEntryModel { ParentMatchup = match }); // added new matchup entry where the parents match
                    if (currMatchup.Entries.Count > 1)
                    {
                        currMatchup.MatchupRound = round;
                        currRound.Add(currMatchup);
                        currMatchup = new MatchupModel();
                    }
                
                }


                model.Rounds.Add(currRound);
                previousRound = currRound; 
                currRound = new List<MatchupModel>();
                round += 1; 




            }

        }

        private static List<MatchupModel> CreateFirstRound(int byes, List<TeamModel> teams)
        {
            List<MatchupModel> output = new List<MatchupModel>();
            MatchupModel curr = new MatchupModel();
            
            foreach (TeamModel team in teams)
            {
                curr.Entries.Add(new MatchupEntryModel { TeamCompeting = team });

                if (byes > 0 || curr.Entries.Count > 1)
                {
                    curr.MatchupRound = 1; // creating first round
                    output.Add(curr);
                    curr = new MatchupModel();

                    if (byes > 0) //if a bye has been used take a bye away
                    {
                        byes -= 1;
                    }
                }

            }
            return output;
        } 

        private static int numberOfByes(int rounds, int numberOfTeams)
        {
            //Math.Pow(2, rounds);

            int output = 0;

            int totalTeams = 0;

            for (int i = 1; i <= rounds; i++)
            {
                totalTeams *= 2;
            }

            output = totalTeams - numberOfTeams;

            return output;
        }
        private static int findNumberOfRounds(int teamCount) 
        {
            int output = 1;
            int val = 2;

            while (val < teamCount)
            {
                output += 1;
                val *= 2; //val = val *2
            }

            return output;
        }

        private static List<TeamModel> RandomizeTeamOrder(List<TeamModel> teams)
        {
            return teams.OrderBy(x => Guid.NewGuid()).ToList(); //sorts by Guid to get somewhat random list
        }
    }
}