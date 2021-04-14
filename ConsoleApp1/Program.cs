using System;
using ClassLib;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var program = new LotteryProgram();
            var vendor = program.Vendor;
            Console.WriteLine("Time is {0}", DateTime.Now);
            vendor.StartSimulatedTicketSales(10_000);
            Console.WriteLine("Tickets sold: {0}", program.Period.soldTickets.Count);
            Console.WriteLine("Time is {0}", DateTime.Now);

            vendor.StartSimulatedTicketSales(10_000);
            Console.WriteLine("Tickets sold: {0}", program.Period.soldTickets.Count);
            Console.WriteLine("Time is {0}", DateTime.Now);

            vendor.StartSimulatedTicketSales(10_000);
            Console.WriteLine("Tickets sold: {0}", program.Period.soldTickets.Count);
            Console.WriteLine("Time is {0}", DateTime.Now);

            Console.WriteLine("Now I will Process the drawing {0}",DateTime.Now);
            program.ClosePeriodSales();
            program.Period.DrawWinningTicket();
            program.Period.ComputeWinners();

            //replicating Tanner
            var y = program.Period.ResultsByPlayer("threadSpawned");
            var x = program.Period.ResultsByWinLevel();

            Console.WriteLine("Now I will close the period and save everything to DB {0}", DateTime.Now);
            program.ResetPeriod();

            Console.WriteLine("DB has been saved.  New period started. {0}", DateTime.Now);

            LotteryStatistics ls = new LotteryStatistics();
            ls.DBStatsAllPeriods();
            Console.WriteLine("stats all periods done");

            //Note: The OS is using almost 10G of memory for 65M tickets.
            //  on my laptop with 16G, I stop at 63M tickets and it can complete in 50 seconds
            //  If, I chose 90M tickets, I don't finish because I force the OS to swap memory fiercely
            //      PREVENT MEMORY HOG - DON'T SELL TO MANY TICKETS for your memory
        }
    }
}
