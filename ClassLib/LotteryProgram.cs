using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ClassLib
{
    public class LotteryProgram
    {
        public LotteryPeriod p = new LotteryPeriod(40_000_000);//Start at $40M
        public LotteryVendor lv;


        public LotteryProgram()
        {
            lv = new LotteryVendor(p); //starting with one Vendor 
        }
        public bool ClosePeriodSales()
        {
            if (p.SalesState == TicketSales.OK)
            {
                p.SalesState = TicketSales.CLOSED;
                return true;
            }
            else
                return false;
        }
        public bool ResetPeriod()
        {
            if (p.SalesState == TicketSales.CLOSED)
            {
                //Write stats to DB
                var ls = new LotteryStatistics();
                //ls.WriteStatsToDB(p);
                ls.WriteStatsBulk(p);

                //get rid of old period, setup new period
                //TODO:  increment GrandPrize Amt by max($10M or 10%)
                p = new LotteryPeriod(p.GrandPrizeAmount += Math.Max(10_000_000, p.GrandPrizeAmount / 10));

                //Ready to allow sales again
                p.SalesState = TicketSales.OK;

                return true;
            }
            else throw new Exception("Cannot reset Lottery Period while sales are ongoing, Please process drawing first");
        }
    }
}
