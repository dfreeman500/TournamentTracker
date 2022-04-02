using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrackerLibrary.Models;

namespace TrackerUI
{
    public partial class TournamentViewerForm : Form
    {
        private TournamentModel tournament; //stored at the form level - anything on the form has access
        List<int> rounds = new List<int>();
        List<MatchupModel> selectedMatchups = new List<MatchupModel>();
        
        
        public TournamentViewerForm(TournamentModel tournamentModel)
        {
            InitializeComponent();

            tournament = tournamentModel;
            LoadFormData();
            LoadRounds();
        }


        private void LoadFormData()
        {
            tournamentName.Text = tournament.TournamentName; //prints the tournament name on the form itself depending on tournament
        }

        private void WireUpRoundsLists()
        {
            roundDropDown.DataSource = null; //wipes out if value already there
            roundDropDown.DataSource = rounds;

        }

        private void WireUpMatchupsLists()
        {

            matchupListBox.DataSource = null;
            matchupListBox.DataSource = selectedMatchups;
            matchupListBox.DisplayMember = "DisplayName";
        }

        private void LoadRounds()
        {
            rounds = new List<int>(); // initializes the rounds each time to avoid duplicate rounds upon re-running


            rounds.Add(1);
            int currRound = 1;
            foreach (List<MatchupModel> matchups in tournament.Rounds)
            {
                if (matchups.First().MatchupRound> currRound)
                {
                    currRound = matchups.First().MatchupRound;
                    rounds.Add(currRound);
                    
                }
            }

            WireUpRoundsLists();
        }




        private void TournamentViewerForm_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void mastchupListBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void roundDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadMatchups();
        }

        private void LoadMatchups()
        {
            int round = (int)roundDropDown.SelectedItem; //casts to int

            foreach (List<MatchupModel> matchups in tournament.Rounds)
            {
                if (matchups.First().MatchupRound == round)
                {
                    selectedMatchups = matchups; 

                }
            }
            WireUpMatchupsLists();
        }
    }
}
