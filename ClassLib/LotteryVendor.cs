using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLib
{
    public class LotteryVendor
    {
        private readonly LotteryPeriod p;

        public LotteryVendor(LotteryPeriod p)
        {
            this.p = p ?? throw new ArgumentNullException(nameof(p));
        }

        public LotteryTicket SellTicket(string playerName, int [] sixnums)
        {
            var lt = new LotteryTicket(playerName, sixnums);
            return processSale(lt);

        }
        public LotteryTicket SellTicket(string playerName)
        {
            var lt = new LotteryTicket(playerName);
            //better concurrency option
            return processSale(lt);
        }

        private LotteryTicket processSale(LotteryTicket lt)
        {
            p.soldTicketsLock.EnterReadLock();
            try
            {
                if (p.SalesState == TicketSales.OK)
                {
                    p.soldTickets.Push(lt);
                    return lt;
                }
                throw new TicketSalesClosedException();
            }
            finally
            {
                p.soldTicketsLock.ExitReadLock();
            }
        }

        public bool SellQuickTickets(string playerName, int qty)
        {
            for(int i = 0; i < qty; i++)
            {
                SellTicket(playerName);
            }
            return true;
        }
        public void purchaseTickets(LotteryPeriod period, int sellLimit)
        {
            if (sellLimit < 1) { throw new System.ArgumentException("Parameter cannot be <1", "sellLimit"); }
            for (int i = 0; i < sellLimit; i++)
            {
                //need to buy/sell at least one more ticket
                LotteryTicket t1 = new LotteryTicket();
                processSale(t1);
            }
            System.Console.WriteLine("Done selling {0} tickets.", sellLimit);
        }
    }
}
