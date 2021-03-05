using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private readonly ILogger<LotteryProgram> _logger;

        public LotteryProgram(ILogger<LotteryProgram> logger)
        {
            _logger = logger;
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
            var elapsedTime = new Stopwatch();
            elapsedTime.Start();
            _logger.LogInformation("Writing stats to database.");

            if (p.SalesState == TicketSales.CLOSED)
            {
                //Write stats to DB
                var ls = new LotteryStatistics();
                try
                {
                    ls.WriteStatsToDB(p);
                }
                catch(Exception ex)
                {
                    _logger.LogError($"Failed to write to the database. Exception: {ex}");
                }
               

                //get rid of old period, setup new period
                //TODO:  increment GrandPrize Amt by max($10M or 10%)
                p = new LotteryPeriod(p.GrandPrizeAmount += Math.Max(10_000_000, p.GrandPrizeAmount / 10));

                //Ready to allow sales again
                p.SalesState = TicketSales.OK;

                return true;
            }
            else throw new Exception("You can't ResetPeriod() when ticketSales are still ongoing.  ClosePeriodSales first!");
        }
    }
}
