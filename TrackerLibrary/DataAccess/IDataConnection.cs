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

    }
}
