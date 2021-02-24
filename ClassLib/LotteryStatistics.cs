using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace ClassLib
{
    public class LotteryStatistics
    {
        public string cs = "Host=localhost;Username=postgres;Password=mysecretpassword;Database=postgres";

        public string TestConn()
        {
            var con = new NpgsqlConnection(cs);
            con.Open();
            var sql = "SELECT version()";
            using var cmd = new NpgsqlCommand(sql, con);
            var version = cmd.ExecuteScalar().ToString();
            con.Close();
            return $"PostgreSQL version: {version}";
        }

        public int WriteStatsToDB(LotteryPeriod lp) //returns periodID assigned in DB
        {
            var con = new NpgsqlConnection(cs);
            con.Open();
            int periodID;
            var cmd = new NpgsqlCommand("insert into period (grandprizeamt,startts,endts)" +
                "VALUES (:gpAmt, :bTS,:eTS) returning id", con);
            cmd.Parameters.Add(new NpgsqlParameter("gpAmt", lp.GrandPrizeAmount));
            cmd.Parameters.Add(new NpgsqlParameter("bTS", lp.PeriodBeginTS));
            cmd.Parameters.Add(new NpgsqlParameter("eTS", DateTime.Now));
            periodID = (int)cmd.ExecuteScalar(); //PERIOD table 1-record

            //TODO: use bulk copy
            var unionResult = lp.losingTicketsL.Union(lp.winningTicketsL);
            foreach (var l in unionResult)
            {
                var cmd2 = new NpgsqlCommand(
                    "insert into ticketsale " +
                    "(period_id,ballstring,ball1,ball2,ball3,ball4,ball5,powerball,winlevel,winamount,type) values (" +
                    " :p_id, :bs, :b1, :b2, :b3, :b4, :b5, :bpower, :winL, :wina, :t)", con);
                cmd2.Parameters.Add(new NpgsqlParameter("p_id", periodID));
                cmd2.Parameters.Add(new NpgsqlParameter("bs",
                    l.balls[0].ToString("00") + l.balls[1].ToString("00") +
                    l.balls[2].ToString("00") + l.balls[3].ToString("00") +
                    l.balls[4].ToString("00") + l.powerBall.ToString("00")));
                cmd2.Parameters.Add(new NpgsqlParameter("b1", l.balls[0]));
                cmd2.Parameters.Add(new NpgsqlParameter("b2", l.balls[1]));
                cmd2.Parameters.Add(new NpgsqlParameter("b3", l.balls[2]));
                cmd2.Parameters.Add(new NpgsqlParameter("b4", l.balls[3]));
                cmd2.Parameters.Add(new NpgsqlParameter("b5", l.balls[4]));
                cmd2.Parameters.Add(new NpgsqlParameter("bpower", l.powerBall));
                cmd2.Parameters.Add(new NpgsqlParameter("winL", l.winLevel));
                cmd2.Parameters.Add(new NpgsqlParameter("wina", l.winAmtDollars));
                cmd2.Parameters.Add(new NpgsqlParameter("t", l.Type));
                cmd2.ExecuteScalar(); //TICKETSALE table - 1 row per ticket
            }
            con.Close();
            return periodID;//returns period.id as assigned by identity column in DB
        }





    }

}
