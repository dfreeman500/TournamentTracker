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
    public partial class CreateTournamentForm : Form, IPrizeRequester, ITeamRequester  // Inherits from (1) parent -- Form. Implements/fulfills contracts for IPrizeRequester,ITeamRequester, allows for loose coupling. An interface doesn't actually bring in code.
    {
        List<TeamModel> availableTeams = GlobalConfig.Connection.GetTeam_All(); //gets all of the teams and puts them in this list of teamModel
        List<TeamModel> selectedTeams = new List<TeamModel>();
        List<PrizeModel> selectedPrizes = new List<PrizeModel>();
        
        public CreateTournamentForm()
        {
            InitializeComponent();
            WireUpLists();

        }
        /// <summary>
        /// updates lists
        /// </summary>
        private void WireUpLists() 
        {
            //wires up the listboxes

            selectTeamDropDown.DataSource = null; //allows to reset list and allow to rebind
            selectTeamDropDown.DataSource = availableTeams;
            selectTeamDropDown.DisplayMember = "TeamName";

            tournamentTeamsListBox.DataSource = null; //allows to reset list and allow to rebind
            tournamentTeamsListBox.DataSource = selectedTeams;
            tournamentTeamsListBox.DisplayMember = "TeamName";


            prizesListBox.DataSource = null; //allows to reset list and allow to rebind
            prizesListBox.DataSource = selectedPrizes;
            prizesListBox.DisplayMember = "PlaceName";

        }


        private void addTeamButton_Click(object sender, EventArgs e)
        {

            // takes selected team in the drop down and converts it to the TeamModel type
            TeamModel t = (TeamModel)selectTeamDropDown.SelectedItem; //


            //if t is not null then remove it from the availableTeams list and add it to the selectedTeams list 
            if(t != null)
            {
                availableTeams.Remove(t);
                selectedTeams.Add(t);

                WireUpLists();
            }
        }
        /// <summary>
        /// Calls create prize form
        /// gets back from the form a prizemodel
        /// take the prizemodel and put it into our list of selected prizes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void createPrizeButton_Click(object sender, EventArgs e)
        {
            //this keyword represents this specific instance
            CreatePrizeForm frm = new CreatePrizeForm(this); //instantiates a new prizeform
            frm.Show(); //shows the form
        }

        
        /// <summary>
        /// 1.) Get back from the form a PrizeModel
        /// 2.) Take the PrizeModel and put it into our list of selected prizes
        /// </summary>
        /// <param name="model"></param>
        public void PrizeComplete(PrizeModel model)
        {
            selectedPrizes.Add(model);
            WireUpLists();
        }

        public void TeamComplete(TeamModel model)
        {
            selectedTeams.Add(model);
            WireUpLists();
        }

        private void createNewTeamLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            CreateTeamForm frm = new CreateTeamForm(this);
            frm.Show();
        }

        private void removeSelectedPlayerButton_Click(object sender, EventArgs e)
        {

            TeamModel t = (TeamModel)tournamentTeamsListBox.SelectedItem; //taking selected item and casting it to a TeamModel

            if (t != null)
            {
                selectedTeams.Remove(t);
                availableTeams.Add(t);
                WireUpLists();
            }
        }

        private void removeSelectedPrizeButton_Click(object sender, EventArgs e)
        {
            PrizeModel p = (PrizeModel)prizesListBox.SelectedItem;
            if (p != null)
            {
                selectedPrizes.Remove(p);
                //prizes don't get reused for different tournaments
            }
        }

        /// <summary>
        /// Validate Data
        /// Create Tournament Model
        /// Create Tournament Entry
        /// Create all of the prizes entries
        /// Create all of the team entries
        /// Create Matchups
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void createTournamentButton_Click(object sender, EventArgs e)
        {
            //validate data
            decimal fee = 0;
            bool feeAcceptable = decimal.TryParse(entryFeeValue.Text, out fee); // attempts to convert to decimal if can - sends value out to fee variable, if can't sends 0 out to fee and will send false as a result of this method

            if (!feeAcceptable)
            {
                MessageBox.Show("You need to enter a valid entry fee",
                    "Invalid Fee",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }



            TournamentModel tm = new TournamentModel();
            tm.TournamentName = tournamentNameValue.Text;
            tm.EntryFee = fee;

            tm.Prizes = selectedPrizes;
            tm.EnteredTeams = selectedTeams;

            //TODO wire up matchups
            TournamentLogic.CreateRounds(tm); //does all the work of creating the rounds




            // Order lists randomly of teams
            // check if it is big enough - if not add in Byes
            // create first riybd if matchups
            // create every round after that. 8 - 4 - 2 - 1
            // 



            GlobalConfig.Connection.CreateTournament(tm);
          

            

        }
    }
}

