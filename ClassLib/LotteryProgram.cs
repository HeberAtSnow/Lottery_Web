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


        public LotteryProgram()
        {
            lv = new LotteryVendor(p); //starting with one Vendor 
        }
        public bool ClosePeriodSales()
        {
            if (p.SalesState == TicketSales.OK)
            {
                p.SalesState = TicketSales.CLOSED;
                //logger.LogInformation("Period was closed succesfully");
                return true;
            }
            else
                return false;
        }
        public bool ResetPeriod()
        {
            if (p.SalesState == TicketSales.CLOSED)
            {
                var stopwatch = new Stopwatch();
                var ls = new LotteryStatistics();

                //logger.LogInformation("Writing current stats to database");
                stopwatch.Start();

                try
                {
                    ls.WriteStatsToDB(p);
                }
                catch (Exception ex)
                {
                    //logger.LogError("Failed to write current stats to the database");
                    throw new Exception("Failed to write to database");
                }
                stopwatch.Stop();
                //logger.LogInformation($"Done writing to the database. Time elapsed: {stopwatch.ElapsedMilliseconds}");

                //get rid of old period, setup new period
                //TODO:  increment GrandPrize Amt by max($10M or 10%)
                p = new LotteryPeriod(p.GrandPrizeAmount += Math.Max(10_000_000, p.GrandPrizeAmount / 10));

                //Ready to allow sales again
                p.SalesState = TicketSales.OK;
                //logger.LogInformation("New period is now open.");

                return true;
            }
            else throw new Exception("You can't ResetPeriod() when ticketSales are still ongoing.  ClosePeriodSales first!");
        }
    }
}
