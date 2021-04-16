using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLib
{
    public class LotteryTicket
    {
        public int[] balls = new int[5];
        public int powerBall;
        public String Player;
        public bool isGraded = false;
        public int winLevel; //only use if isGraded is set to true
        public decimal winAmtDollars; //only use if isGraded is set to true
        public string Type { get; set; }

        [ThreadStatic] static Random rnd;

        public LotteryTicket() : this("Player Name Anonymous")
        {
        }
        public LotteryTicket(String PlayerName)
        { //RULE: first 5 balls are 1-69 inclusive, no duplicates in balls 1-5
          //RULE: 6th ball is 1-26 inclusive
            if (rnd == null)
            {
                rnd = new Random();
                Console.WriteLine("Thread# {0} set Rnd", System.Threading.Thread.CurrentThread.ManagedThreadId);
            }
            bool duplicate = false;
            for (int i = 0; i < 5; i++)
            {
                int x = rnd.Next(1, 70);//1-69 inclusive
                //is this tryBall already in my ticket?  (no dups allowed)
                for (int j = 0; j < i; j++)
                {
                    if (balls[j] == x)
                    {
                        duplicate = true;
                    }
                }
                if (duplicate == false)
                {
                    balls[i] = x;
                }
                else
                {
                    //force myself through loop again
                    i--;
                    duplicate = false;
                }
            }//end of for() first 5 balls
            System.Array.Sort(balls);
            //get powerball
            powerBall = rnd.Next(1, 27);//1-26 inclusive
            Player = PlayerName;
        }
        public LotteryTicket(String PlayerName, int[] sixBalls)
        {
            if (sixBalls.Length != 6)
            {
                throw new ArgumentException("The 'sixBalls' parameter must include 5 numbers + Powerball (total of 6 numbers", "sixBalls");
            }
            Player = PlayerName;
            for(int i = 0; i < 5; i++)
            {
                balls[i] = sixBalls[i];
            }
            powerBall = sixBalls[5];

            if (! isValidTicket(out string message))
            {
                throw new ArgumentException(message);
            }
        }
        private  bool isValidTicket(out string message)
        {
            //test 1:  six numbers
            if (this.balls.Length != 5) { message = "6 balls needed";  return false; }
            //test 2:  no duplicates
            for (int i = 0; i < 4; i++)
            {
                for (int j = i + 1; j < 5; j++)
                {
                    if (this.balls[i] == this.balls[j])
                    {
                        message = $"The ball {i} is duplicated";
                        return false; // l.balls[i] is duplicated"
                    }
                }
            }
            //test 3:  first 5 are 1-69 inclusive,  last one is 1-26 inclusive
            for (int i = 0; i < 5; i++)
            {
                if (this.balls[i] < 1 || this.balls[i] > 69) { message = "White balls are valid 1-69"; return false; }
            }
            if (this.powerBall < 1 || this.powerBall > 26) { message = "Powerball is valid 1-26"; return false; }

            message = null;
            return true; //all others pass!
        }

    }//end LotteryTicket
}
