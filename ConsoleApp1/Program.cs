using System;
using ClassLib;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var program = new LotteryProgram();
            var vendor = program.lv;
            Console.WriteLine("Time is {0}", DateTime.Now);
            vendor.StartSimulatedTicketSales(10_000_000);
            Console.WriteLine("Tickets sold: {0}", program.p.soldTickets.Count);
            Console.WriteLine("Time is {0}", DateTime.Now);

            vendor.StartSimulatedTicketSales(10_000_000);
            Console.WriteLine("Tickets sold: {0}", program.p.soldTickets.Count);
            Console.WriteLine("Time is {0}", DateTime.Now);

            vendor.StartSimulatedTicketSales(1_000_000);
            Console.WriteLine("Tickets sold: {0}", program.p.soldTickets.Count);
            Console.WriteLine("Time is {0}", DateTime.Now);

            //Note: The OS is using almost 10G of memory for 65M tickets.
            //  on my laptop with 16G, I stop at 63M tickets and it can complete in 50 seconds
            //  If, I chose 90M tickets, I don't finish because I force the OS to swap memory fiercely
            //      PREVENT MEMORY HOG - DON'T SELL TO MANY TICKETS for your memory
        }
    }
}
