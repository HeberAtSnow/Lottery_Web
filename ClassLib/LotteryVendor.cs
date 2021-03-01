using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ClassLib
{
    public class LotteryVendor
    {
        private readonly LotteryPeriod p;
        private int simulated_tickets_per_thread;

        public void StartSimulatedTicketSales(int numberToSell)
        {
            simulated_tickets_per_thread = numberToSell;
            Thread thr = new Thread(TLotteryVendor); thr.Start();
            Thread thr2 = new Thread(TLotteryVendor); thr2.Start();
            Thread thr3 = new Thread(TLotteryVendor); thr3.Start();
            thr.Join(); 
            thr2.Join();
            thr3.Join();
        }

        
        private void TLotteryVendor()
        {
            PurchaseTickets(p, simulated_tickets_per_thread);
        }

        public void PurchaseTickets(LotteryPeriod period, int sellLimit)
        {
            if (sellLimit < 1) { throw new System.ArgumentException("Parameter cannot be <1", "sellLimit"); }
            for (int i = 0; i < sellLimit; i++)
            {
                SellTicket("threadSpawned");
            }
        }

        public LotteryVendor(LotteryPeriod p)
        {
            this.p = p ?? throw new ArgumentNullException(nameof(p));
        }

        public LotteryTicket SellTicket(string playerName, int [] sixnums)
        {
            var lt = new LotteryTicket(playerName, sixnums);
            lt.Type = "custom";
            ProcessSale(lt);
            return lt;
        }
        public LotteryTicket SellTicket(string playerName)
        {
            var lt = new LotteryTicket(playerName);
            lt.Type = "quick";
            //better concurrency option
            ProcessSale(lt);
            return lt;
        }

        private void ProcessSale(LotteryTicket lt)
        {

            //to make this threadsafe, the rule will be:
            // 1) always take lock of p.soldTickets
            // 2) check if TickeSales.OK

            if (p.SalesState == TicketSales.OK)
            {
                p.soldTickets.Push(lt);
            }
            else
                throw new TicketSalesClosedException();
        }

        public bool SellQuickTickets(string playerName, int qty)
        {
            for(int i = 0; i < qty; i++)
            {
                SellTicket(playerName);
            }
            return true;
        }

    }
}
