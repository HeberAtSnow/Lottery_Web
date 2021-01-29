using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLib
{
    public class LotteryStatistics
    {
        public int numAlt1Winners = 0; public int numAlt2Winners = 0; public int numAlt3Winners = 0;
        public int numAlt4Winners = 0; public int numAlt5Winners = 0; public int numAlt6Winners = 0;
        public int numAlt7Winners = 0; public int numAlt8Winners = 0;

        //public void compileStats(LotteryPeriod p)
        //{
        //    while (p.soldTickets.Count > 0)
        //    {
        //        LotteryTicket lt = new LotteryTicket();
        //        lock (p.soldTickets)
        //        {
        //            try
        //            {
        //                lt = p.soldTickets.Pop();
        //            }
        //            catch (System.InvalidOperationException) { break; }//leave the while
        //        }
        //        if (p.IsWinnerGrandPrize(lt) > 0)
        //        { //GRAND PRIZE 
        //            lock (p.flagGrandPrizeWinners)
        //            {
        //                p.numGrandPrizeWinners++;
        //            }
        //        }
        //        else if (p.IsWinner1Mill(lt) > 0)
        //        {//1st Prize
        //            lock (p.flagWin1)
        //            {
        //                p.numAlt1Winners++;
        //            }
        //        }
        //        else if (p.isWin2(lt) > 0)
        //        {
        //            lock (p.flagWin2)
        //            {
        //                p.numAlt2Winners++;
        //            }
        //        }
        //        else if (p.IsWinner50k(lt) > 0)
        //        {// Prize
        //            lock (p.flagWin3)
        //            {
        //                p.numAlt3Winners++;
        //            }
        //        }
        //        else if (p.IsWinner100(lt) > 0)
        //        {
        //            lock (p.flagWin4)
        //            {
        //                p.numAlt4Winners++;
        //            }
        //        }
        //        else if (p.IsWinner7(lt) > 0)
        //        {// Prize
        //            lock (p.flagWin5)
        //            {
        //                p.numAlt5Winners++;
        //            }
        //        }
        //        else if (p.IsWinner4(lt) > 0)
        //        {
        //            lock (p.flagWin6)
        //            {
        //                p.numAlt6Winners++;
        //            }
        //        }
        //        else if (p.isWin7(lt) > 0)
        //        {//Prize
        //            lock (p.flagWin7)
        //            {
        //                p.numAlt7Winners++;
        //            }
        //        }
        //        else if (p.isWin8(lt) > 0)
        //        {
        //            lock (p.flagWin8)
        //            {
        //                p.numAlt8Winners++;
        //            }
        //        }//otherwise, it is a losing ticket
        //    }//end while
        //}//end CompileStats()
    }

}
