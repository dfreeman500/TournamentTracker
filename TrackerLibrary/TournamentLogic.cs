using System;
using System.Collections.Generic;
using System.Configuration;
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
            int byes = numberOfByes(rounds, randomizedTeams.Count); 

            model.Rounds.Add(CreateFirstRound(byes, randomizedTeams));
            CreateOtherRounds(model, rounds);
            //UpdateTournamentResults(model); //makes sure any byes move to the next round -- running load file - causes exception/unecessary

        }

        public static void UpdateTournamentResults(TournamentModel model)
        {
            int startingRound = model.CheckCurrentRound();
            //updates the matches

            List<MatchupModel> toScore = new List<MatchupModel>(); // the matchups that need to be scored

            //loop through entire tournament - i.e. loop through rounds

            foreach (List<MatchupModel> round in model.Rounds)
            {
                foreach (MatchupModel rm in round)
                {
                    if (rm.Winner == null && (rm.Entries.Any(x=> x.Score != 0) || rm.Entries.Count == 1)) //avoids scoring the same winner over an over
                    {
                        toScore.Add(rm);
                    }                
                }
            }

            MarkWinnerInMatchup(toScore);

            AdvanceWinners(toScore, model);


            toScore.ForEach(x => GlobalConfig.Connection.UpdateMatchup(x)); //inline foreach function

            int endingRound = model.CheckCurrentRound();

            if (endingRound > startingRound) 
            {
                model.AlertUsersToNewRound();
            }


        }


        public static void AlertUsersToNewRound(this TournamentModel model) 
        {

            //finds current round, then gets all matchups in current round, then loops through every matchup,
            //every match up entry in each matchup, every person in each matchup, then
            //alerts that person with the info that they are in a new matchup



            //int currentRoundNumber = model.CheckCurrentRound();
            //List<MatchupModel> currentRound = model.Rounds.Where(x=> x.First().MatchupRound == currentRoundNumber).First();
        
            //foreach(MatchupModel matchup in currentRound)
            //{
            //    foreach(MatchupEntryModel me in matchup.Entries)
            //    {
            //        foreach(PersonModel p in me.TeamCompeting.TeamMembers)
            //        {
            //            AlertPersonToNewRound(p, me.TeamCompeting.TeamName, matchup.Entries.Where(x => x.TeamCompeting != me.TeamCompeting).FirstOrDefault());
            //        }
            //    }
            //}
        
        }

        private static void AlertPersonToNewRound(PersonModel p, string teamName, MatchupEntryModel competitor)
        {

            //if (p.EmailAddress.Length == 0)
            //{
            //    return;
            //}



            //string to = "";
            //string subject = "";

            //StringBuilder body = new StringBuilder();


            //if (competitor != null)
            //{
            //    subject = $"You have a new matchup with { competitor.TeamCompeting.TeamName }";

            //    body.AppendLine("<h1>You have a new matchup</h1>");
            //    body.Append("<strong>Competitor: ");
            //    body.Append(competitor.TeamCompeting.TeamName);
            //    body.AppendLine();
            //    body.AppendLine();
            //    body.AppendLine("Have a great time!");
            //    body.AppendLine("~Tournament Tracker");

            //}
            //else
            //{
            //    subject = "You have a bye week this round.";

            //    body.AppendLine("Enjoy your round off");
            //    body.AppendLine("~Tournament Tracker");


            //}

            //to = p.EmailAddress;

            //EmailLogic.sendEmail(to, subject, body.ToString()) ;


        }

        private static int CheckCurrentRound(this TournamentModel model)
        {
            int output = 1;
            foreach (List<MatchupModel> round in model.Rounds)
            {
                if (round.All(x => x.Winner != null)) //if every winner not null
                {
                    output += 1;

                }
                else
                {
                    return output; //
                }

            }
            //Tournament is complete

            CompleteTournament(model);
            return output -1; //this advances to the next round including the last round
        }

        private static void CompleteTournament(TournamentModel model)
        {
            GlobalConfig.Connection.CompleteTournament(model);

            //Rounds is a list of list of matchup. Finds the Last round (which is a list)
            ////and returns the first of that list
            TeamModel winners = model.Rounds.Last().First().Winner;

            //Finds the loser in the championship game
            TeamModel runnerUp = model.Rounds.Last().First().Entries.Where(x=>x.TeamCompeting != winners).First().TeamCompeting;

            decimal winnerPrize = 0;
            decimal runnerUpPrize = 0;

            //checks to see if any prizes are entered
            // takes total number of teams and multiplies by entry fee
            if(model.Prizes.Count > 0)
            {
                decimal totalIncome = model.EnteredTeams.Count * model.EntryFee;

                PrizeModel firstPlacePrize = model.Prizes.Where(x => x.PlaceNumber == 1).FirstOrDefault();
                PrizeModel secondPlacePrize = model.Prizes.Where(x => x.PlaceNumber == 2).FirstOrDefault();

                if (firstPlacePrize != null)
                {
                    winnerPrize = firstPlacePrize.CalculatePrizePayout(totalIncome);
                }
                if (secondPlacePrize != null)
                {
                    runnerUpPrize = secondPlacePrize.CalculatePrizePayout(totalIncome);
                }


            }
            //Send email to all tournament
            //string subject = "";
            //StringBuilder body = new StringBuilder();
            //subject = $"In { model.TournamentName }, { winners.TeamName } has won!";
            //body.AppendLine("<h1>We have a WINNER</h1>");
            //body.AppendLine("<p>Congratulations to our winner on a great tournament.</p>");
            //body.AppendLine("<br />");
            //if (winnerPrize >0)
            //{
            //    body.AppendLine($"<p> { winners.TeamName } will receive ${ winnerPrize }</p>");
            //}
            //if (runnerUpPrize > 0)
            //{
            //    body.AppendLine($"<p> { runnerUp.TeamName } will receive ${ runnerUpPrize }</p>");
            //}
            //body.AppendLine("<p>Thanks for a great tournament everyone!</p>");
            //body.AppendLine("~Tournament Tracker");


            //List<string> bcc = new List<string>();

            //foreach(TeamModel t in model.EnteredTeams)
            //{
            //    foreach(PersonModel p in t.TeamMembers)
            //    {
            //        if (p.EmailAddress.Length > 0)
            //        {
            //            bcc.Add(p.EmailAddress);
            //        }

            //    }                
            //}

            //EmailLogic.sendEmail(new List<string>(), bcc, subject, body.ToString());

            //Complete Tournament
            model.CompleteTournament();

        }

        private static decimal CalculatePrizePayout(this PrizeModel prize, decimal totalIncome)
        {
            decimal output = 0;
            if (prize.PrizeAmount > 0)
            {
                output = prize.PrizeAmount;
            }
            else
            {
                output = Decimal.Multiply(totalIncome, Convert.ToDecimal(prize.PrizePercentage / 100));
            }

            return output;
        }

        private static void AdvanceWinners(List<MatchupModel> models, TournamentModel tournament)
        {

            //For every Matchup, look through every round and every matchup in every round
            // and find every entry in every matchup - if the parentmatchup is not null check to 
            // to see if it matches the current model - if Id matches, that's where the winner advances to


            foreach (MatchupModel m in models) // only matchup models that are marked as needing to be scored
            {
                foreach (List<MatchupModel> round in tournament.Rounds)
                {
                    foreach (MatchupModel rm in round)
                    {
                        foreach (MatchupEntryModel me in rm.Entries) //the matchupentry holds the parent matchup
                        {
                            if (me.ParentMatchup != null) // first round doesn't have a parent matchup -allows to skip
                            {
                                if (me.ParentMatchup.Id == m.Id) //m.id is current matchup
                                {
                                    me.TeamCompeting = m.Winner;
                                    GlobalConfig.Connection.UpdateMatchup(rm); //save the matchup itself
                                }
                            }
                        }
                    }
                }
            }
        }

        private static void MarkWinnerInMatchup(List<MatchupModel> models)
        {
            //using System.Configuration; uses key to determine score direction from app.config
            string greaterWins = ConfigurationManager.AppSettings["greaterWins"];

            foreach (MatchupModel m in models)
            {
                //Checks for a bye entry
                if (m.Entries.Count == 1)
                {
                    m.Winner = m.Entries[0].TeamCompeting;
                    continue; //continues at the next iteration without exiting
                }
                //"0" means false, or lower score wins
                if (greaterWins == "0")
                {
                    if (m.Entries[0].Score < m.Entries[1].Score) //if first score is lower, then that team is the winner
                    {
                        m.Winner = m.Entries[0].TeamCompeting;
                    } 
                    else if (m.Entries[1].Score < m.Entries[0].Score)
                    {
                        m.Winner = m.Entries[1].TeamCompeting;

                    }
                    else
                    {
                        throw new Exception("Ties are not allowed in this application");
                    }

                }
                else
                {
                    //"1" or any other value means high score wins
                    if (m.Entries[0].Score > m.Entries[1].Score) //if first score is lower, then that team is the winner
                    {
                        m.Winner = m.Entries[0].TeamCompeting;
                    }
                    else if (m.Entries[1].Score > m.Entries[0].Score)
                    {
                        m.Winner = m.Entries[1].TeamCompeting;

                    }
                    else
                    {
                        throw new Exception("Ties are not allowed in this application");
                    }
                }
            }


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

            int totalTeams = 1;

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
