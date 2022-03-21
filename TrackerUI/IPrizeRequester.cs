using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary.Models;

namespace TrackerUI
{
    /// <summary>
    /// Interface allows whoever implements this contract will have one 
    /// method called prizecomplete that returns nothing but takes a prizeModel
    /// 
    /// </summary>
    public interface IPrizeRequester
    {
        void PrizeComplete(PrizeModel model); 
    }
}
