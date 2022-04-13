using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrackerLibrary;
using TrackerLibrary.Models;

namespace TrackerUI
{
    public partial class TournamentViewerForm : Form
    {
        private TournamentModel tournament; //stored at the form level - anything on the form has access
        BindingList<int> rounds = new BindingList<int>();
        BindingList<MatchupModel> selectedMatchups = new BindingList<MatchupModel>();
        
        
        public TournamentViewerForm(TournamentModel tournamentModel)
        {
            InitializeComponent();


            tournament = tournamentModel;

            tournament.OnTournamentComplete += Tournament_OnTournamentComplete;

            WireUpLists();

            LoadFormData();
            LoadRounds();
            //MessageBox.Show("Hello");
        }

        private void Tournament_OnTournamentComplete(object sender, DateTime e)
        {
            this.Close();
            
         }

        private void LoadFormData()
        {
            tournamentName.Text = tournament.TournamentName; //prints the tournament name on the form itself depending on tournament
        }

        private void WireUpLists()
        {
            //roundDropDown.DataSource = null; //wipes out if value already there
            roundDropDown.DataSource = rounds;
            matchupListBox.DataSource = selectedMatchups;
            matchupListBox.DisplayMember = "DisplayName";
        }

        private void LoadRounds()
        {
            //rounds = new BindingList<int>(); // initializes the rounds each time to avoid duplicate rounds upon re-running
            rounds.Clear(); //resets the elements

            rounds.Add(1);
            int currRound = 1;
            foreach (List<MatchupModel> matchups in tournament.Rounds)
            {
                if (matchups.First().MatchupRound > currRound)
                {
                    currRound = matchups.First().MatchupRound;
                    rounds.Add(currRound);
                    
                }
            }

            LoadMatchups(1); // round 1 is always the first on the list
        }




        private void TournamentViewerForm_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void LoadMatchup(MatchupModel m)
        {
            if (m != null) // only runs if != null - can't count null
            {
                for (int i = 0; i < m.Entries.Count; i++)
                {
                    if (i == 0)
                    {
                        if (m.Entries[0].TeamCompeting != null)
                        {
                            teamOneName.Text = m.Entries[0].TeamCompeting.TeamName;
                            teamOneScoreValue.Text = m.Entries[0].Score.ToString();

                            teamTwoName.Text = "<bye>";
                            teamTwoScoreValue.Text = "0";

                        }
                        else
                        {
                            teamOneName.Text = "Not Yet Set";
                            teamOneScoreValue.Text = "";
                        }
                    }
                    if (i == 1)
                    {
                        if (m.Entries[1].TeamCompeting != null)
                        {
                            teamTwoName.Text = m.Entries[1].TeamCompeting.TeamName;
                            teamTwoScoreValue.Text = m.Entries[1].Score.ToString();
                        }
                        else
                        {
                            teamTwoName.Text = "Not Yet Set";
                            teamTwoScoreValue.Text = "";
                        }
                    }
                } 
            }
        }
        private void matchupListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadMatchup((MatchupModel)matchupListBox.SelectedItem);
        }

        private void roundDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadMatchups((int)roundDropDown.SelectedItem); //casts to int);
        }

        private void LoadMatchups(int round)
        {

            foreach (List<MatchupModel> matchups in tournament.Rounds)
            {
                if (matchups.First().MatchupRound == round)
                {
                    //clears out, then adds items in one by one instead of using bindinglist
                    selectedMatchups.Clear();
                    foreach(MatchupModel m in matchups)
                    {
                        if (m.Winner == null || !unplayedOnlyCheckbox.Checked) //if no winner or if unplayedOnly is unchecked then add (m)
                        {
                            selectedMatchups.Add(m);

                        }                    
                    }
                    //selectedMatchups = new BindingList<MatchupModel>(matchups);
                }
            }

            if (selectedMatchups.Count>0)
            {
                LoadMatchup(selectedMatchups.First());//first matchup in the list

            }
            DisplayMatchupInfo();

        }

        private void DisplayMatchupInfo()
        {
            bool isVisible = (selectedMatchups.Count > 0);  //evaluation is stored into a bool instead of if statement

            teamOneName.Visible = isVisible;
            teamOneScoreLabel.Visible = isVisible;
            teamOneScoreValue.Visible = isVisible;

            teamTwoName.Visible = isVisible;
            teamTwoScoreLabel.Visible = isVisible;
            teamTwoScoreValue.Visible = isVisible;

            versusLabel.Visible = isVisible;
            scoreButton.Visible = isVisible;



        }

        private void unplayedOnlyCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            LoadMatchups((int)roundDropDown.SelectedItem); //casts to int);

        }

        private string ValidateData()
        {
            string output = "";


            double teamOneScore = 0;
            double teamTwoScore = 0;

            bool scoreOneValid = double.TryParse(teamOneScoreValue.Text, out teamOneScore);
            bool scoreTwoValid = double.TryParse(teamTwoScoreValue.Text, out teamTwoScore);
        
            if (!scoreOneValid)
            {
                output = "The Score one Value is not a valid number.";
            }

            else if (!scoreTwoValid)
            {
                output = "The Score two value si not a valid number";
            }
        
            else if(teamOneScore == 0 && teamTwoScore == 0)
            {
                output = "You did not enter a score for either team";
            }

            else if (teamOneScore == teamTwoScore)
            {
                output = "We do not allow ties in this application.";

                }
            return output;
        }

        private void scoreButton_Click(object sender, EventArgs e)
        {

            string errorMessage = ValidateData();
            if (errorMessage.Length > 0)
            {
                MessageBox.Show($"Input Error { errorMessage }");
                return;
            }



            MatchupModel m = (MatchupModel)matchupListBox.SelectedItem;
            double teamOneScore = 0;
            double teamTwoScore = 0;

            for(int i = 0; i < m.Entries.Count; i++)
            {
                if (i == 0)
                {
                    if (m.Entries[0].TeamCompeting != null)
                    {

                        bool scoreValid = double.TryParse(teamOneScoreValue.Text, out teamOneScore);
                        if (scoreValid)
                        {
                            m.Entries[0].Score = teamOneScore;

                        }
                        else
                        {
                            MessageBox.Show("Please Enter a valid score for team 1");
                            return; //exits out of the entire method at time of error
                        }
                    }
   
                }
                if (i == 1)
                {
                    if (m.Entries[1].TeamCompeting != null)
                    {
                        bool scoreValid = double.TryParse(teamTwoScoreValue.Text, out teamTwoScore);
                        if (scoreValid)
                        {
                            m.Entries[1].Score = teamTwoScore;

                        }
                        else
                        {
                            MessageBox.Show("Please Enter a valid score for team 2");
                            return; //exits out of the entire method at time of error
                        }
                    }
                }
            }

            try
            {
                TournamentLogic.UpdateTournamentResults(tournament); // passing in entire model because working on entire object

            }
            catch (Exception ex)
            {

                MessageBox.Show($"The application had the followingerror: { ex.Message}");
                return;
            }
            LoadMatchups((int)roundDropDown.SelectedItem);

        }
    }
}
