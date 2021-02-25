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
            vendor.StartSimulatedTicketSales(10_000_000);
            Console.WriteLine("Tickets sold: {0}", program.p.soldTickets.Count);
        }
    }
}
