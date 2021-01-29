using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLib
{
    public class LotteryPeriod
    {
        int[] winTicket = { 1, 2, 3, 4, 5, 6 }; //Winning Ticket Values
        public Stack<LotteryTicket> soldTickets = new Stack<LotteryTicket>();
        public Stack<LotteryTicket> winningTickets = new Stack<LotteryTicket>();
        public Stack<LotteryTicket> loosingTickets = new Stack<LotteryTicket>();
        public int numGrandPrizeWinners = 0;
        public object flagGrandPrizeWinners = new object();
        public object flagWin1 = new object();
        public object flagWin2 = new object();
        public object flagWin3 = new object();
        public object flagWin4 = new object();
        public object flagWin5 = new object();
        public object flagWin6 = new object();
        public object flagWin7 = new object();
        public object flagWin8 = new object();

        //TODO:  remove the following stats - put in LotteryStatistics
        public int numAlt1Winners = 0; public int numAlt2Winners = 0; public int numAlt3Winners = 0;
        public int numAlt4Winners = 0; public int numAlt5Winners = 0; public int numAlt6Winners = 0;
        public int numAlt7Winners = 0; public int numAlt8Winners = 0;

        public decimal GrandPrizeAmount { get; set; }

        public LotteryTicket WinningTicket { get; set; }

        public void DrawWinningTicket()
        {
            //TODO: stop any other 'purchases' from happening
            //      by changing the state

            WinningTicket = new LotteryTicket("WinningTicket");
        }

        public void ComputeWinners()
        {
            //TODO: ensure the state is set to ONLY let this work if drawing has started/ended
            LotteryTicket lt;
            while (soldTickets.Count > 0)
            {
                try
                {
                    lt = soldTickets.Pop();
                    CheckWinningTicket(lt);
                    if (lt.winLevel > 0)
                        winningTickets.Push(lt);
                    else
                        loosingTickets.Push(lt);
                    //each ticket is moved from soldTickets to ( winningTickets or loosingTickets )
                }
                catch (System.InvalidOperationException) { break; }//leave the while 
            }
        }

        public int NumberMatchingWhiteBalls(LotteryTicket lt)
        {
            int numMatches = 0;
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (lt.balls[i] == WinningTicket.balls[j])
                    {
                        numMatches++;
                    }
                }//end for
            }
            return numMatches;
        }
        public void CheckWinningTicket(LotteryTicket lt)
        {
            int whiteMatches = NumberMatchingWhiteBalls(lt);
            if (lt.powerBall == WinningTicket.powerBall && whiteMatches == 5)
            {
                lt.winLevel = 1;
                lt.winAmtDollars = GrandPrizeAmount;
            }
            else if (whiteMatches == 5)
            {
                lt.winLevel = 2;
                lt.winAmtDollars = 1000000;//$1M
            }
            else if (lt.powerBall == WinningTicket.powerBall && whiteMatches == 4)
            {
                lt.winLevel = 3;
                lt.winAmtDollars = 50000;//$50k
            }
            else if (whiteMatches == 4)
            {
                lt.winLevel = 4;
                lt.winAmtDollars = 100;//$100
            }
            else if (lt.powerBall == WinningTicket.powerBall && whiteMatches == 3)
            {
                lt.winLevel = 5;
                lt.winAmtDollars = 100;//$100
            }
            else if (whiteMatches == 3)
            {
                lt.winLevel = 6;
                lt.winAmtDollars = 7;//$7
            }
            else if (lt.powerBall == WinningTicket.powerBall && whiteMatches == 2)
            {
                lt.winLevel = 7;
                lt.winAmtDollars = 7;//$7
            }
            else if (lt.powerBall == WinningTicket.powerBall && whiteMatches == 1)
            {
                lt.winLevel = 8;
                lt.winAmtDollars = 4;//$4
            }
            else if (lt.powerBall == WinningTicket.powerBall && whiteMatches == 0)
            {
                lt.winLevel = 9;
                lt.winAmtDollars = 4;//$4
            }
            else
            {
                lt.winLevel = 0;
                lt.winAmtDollars = 0;
            }

            lt.isGraded = true;

        }
    }
}
