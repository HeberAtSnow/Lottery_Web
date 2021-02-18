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
        public LotteryPeriod p = new LotteryPeriod();
        static int vTicketsToSell = 1000000;
        static int vThreadCount = 3; //VENDOR Thread Count
        public LotteryVendor lv;


        public LotteryProgram()
        {
            ////Stopwatch to measure how long N threads take to sell _____ tickets
            //System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
            //timer.Start();
            ////Stopwatch stop
            //timer.Stop();
            //Console.WriteLine("Elapsed time for {0} threads is: {1}", vThreadCount, timer.Elapsed);
            //Console.WriteLine("Array has {0:n0} tickets sold.", p.soldTickets.Count);

            ////Phase 3 - check for winners
            //Thread thr4 = new Thread(tStatsCompilers);
            //thr4.Start();
            //Thread thr5 = new Thread(tStatsCompilers);
            //thr5.Start();
            //thr4.Join(); thr5.Join();
            //Console.WriteLine("Winners are:  GRAND={0:n0} 1st={1:n0}  2nd={2:n0}  3rd={3:n0} 4th={4:n0}" +
            //    " 5th={5:n0}  6th={6:n0}  7th={7:n0}  8th={8:n0}", p.numGrandPrizeWinners, p.numAlt1Winners, p.numAlt2Winners,
            //     p.numAlt3Winners, p.numAlt4Winners, p.numAlt5Winners, p.numAlt6Winners, p.numAlt7Winners, p.numAlt8Winners);

            p.GrandPrizeAmount = 40000000;//$40M
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
        public void ResetPeriod()
        {
            if (p.SalesState==TicketSales.CLOSED)
            {
                //TODO:  Save stats
                //TODO:  Write stats to DB
                //TODO:  increment GrandPrize Amt 
                //Ready to allow sales again
                p.SalesState = TicketSales.OK;

            }
            //p = new LotteryPeriod();
        }
        

        //static void tStatsCompilers()
        //{
        //    LotteryStatistics ls = new LotteryStatistics();
        //    ls.compileStats(p);
        //    Console.WriteLine("Thread finished compiling stats");
        //}
    }

}
