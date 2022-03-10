using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary
{
    public class PrizeModel
    {
        //unique identifier for prize
        public int Id { get; set; }
        //Numeric ID for the place (ex: 2 for second)
        public int PlaceNumber { get; set; }
        //common name for place (ex: second)
        public string PlaceName { get; set; }
        //fixed amount this place earns or 0 if not used
        public decimal PrizeAmount { get; set; }
        //percentage of prize amount
        public double PrizePercentage { get; set; }

    }
}
