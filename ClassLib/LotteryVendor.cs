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
            Heberlogger.Init();
            Thread thr = new Thread(TLotteryVendor); thr.Start();
            Thread thr2 = new Thread(TLotteryVendor); thr2.Start();
            Thread thr3 = new Thread(TLotteryVendor); thr3.Start();
            Heberlogger.Write("All 3 threads have started");
            thr.Join(); Heberlogger.Write("Thr1 is done"); 
            thr2.Join(); Heberlogger.Write("Thr2 is done");
            thr3.Join(); Heberlogger.Write("Thr3 is done");
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
                Heberlogger.Write("I am on {0} out of {1}", i, sellLimit);
                LotteryTicket t1 = new LotteryTicket();
                ProcessSale(t1);
            }
            Heberlogger.Write("Done selling {0} tickets.", sellLimit);
        }

        public LotteryVendor(LotteryPeriod p)
        {
            this.p = p ?? throw new ArgumentNullException(nameof(p));
        }

        public LotteryTicket SellTicket(string playerName, int [] sixnums)
        {
            var lt = new LotteryTicket(playerName, sixnums);
            ProcessSale(lt);
            return lt;
        }
        public LotteryTicket SellTicket(string playerName)
        {
            var lt = new LotteryTicket(playerName);
            //better concurrency option
            ProcessSale(lt);
            return lt;
        }

        private void ProcessSale(LotteryTicket lt)
        {

            //to make this threadsafe, the rule will be:
            // 1) always take lock of p.soldTickets
            // 2) check if TickeSales.OK
            //   - same when changing TicketSales - must always have lock of p.soldTickets first
            //Heberlogger.Write("ProcessSale asking for lock");
            //lock (p.soldTickets)
            //{
            //    Heberlogger.Write("ProcessSale has lock");
            //    if (p.SalesState == TicketSales.OK)
            //    {
            //        p.soldTickets.Push(lt);
            //        Heberlogger.Write("ProcessSale released lock");
            //        return ;
            //    }
            //    else
            //    {
            //        Heberlogger.Write("ProcessSale Lock failed to have ticketsalesOK");
            //        throw new TicketSalesClosedException();
            //    }

            //}

           // p.soldTicketsLock.EnterReadLock();
            //try
            //{
                if (p.SalesState == TicketSales.OK)
                {
                    p.soldTickets.Push(lt);
                }
                else
                    throw new TicketSalesClosedException();
            //}
            //finally
            //{
            //    p.soldTicketsLock.ExitReadLock();
            //}
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
