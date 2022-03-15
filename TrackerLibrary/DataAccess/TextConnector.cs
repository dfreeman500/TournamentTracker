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
        private const string PrizesFile = "PrizeModels.csv"; //because private const string notation ...is uppercase

        public PrizeModel CreatePrize(PrizeModel model)
        {
            // load the text file and convert the text to list<PrizeModel>
            List<PrizeModel> prizes = PrizesFile.FullFilePath().LoadFile().ConvertToPrizeModels();

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
            prizes.SaveToPrizeFile(PrizesFile);
            return model;

        }

    }
}
