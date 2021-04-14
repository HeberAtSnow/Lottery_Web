using Microsoft.Extensions.Logging;
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
        private readonly ILogger<LotteryProgram> logger;

        public LotteryPeriod Period = new LotteryPeriod(40_000_000);//Start at $40M
        public LotteryVendor Vendor;

        public LotteryProgram(ILogger<LotteryProgram> logger)
        {
            Vendor = new LotteryVendor(Period); //starting with one Vendor
            this.logger = logger;
        }

        public bool ClosePeriodSales()
        {
            if (Period.SalesState == TicketSales.OK)
            {
                Period.SalesState = TicketSales.CLOSED;
                return true;
            }
            else
                return false;
        }
        public bool ResetPeriod()
        {
            if (Period.SalesState == TicketSales.CLOSED)
            {
                //Write stats to DB
                var ls = new LotteryStatistics();
                ls.WriteStatsToDB(Period);

                //get rid of old period, setup new period
                //TODO:  increment GrandPrize Amt by max($10M or 10%)
                Period = new LotteryPeriod(Period.GrandPrizeAmount += Math.Max(10_000_000, Period.GrandPrizeAmount / 10));

                //Ready to allow sales again
                Period.SalesState = TicketSales.OK;

                return true;
            }
            else throw new Exception("You can't ResetPeriod() when ticketSales are still ongoing.  ClosePeriodSales first!");
        }
    }
}
