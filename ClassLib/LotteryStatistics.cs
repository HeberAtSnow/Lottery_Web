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
        public string cs = "Host=kubernetes.docker.internal;Username=postgres;Password=mysecretpassword;Database=postgres";

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

        public int WriteStatsToDB(LotteryPeriod lp)
        {
            using var con = new NpgsqlConnection(cs);
            con.Open();
            int periodID;
            var cmd = new NpgsqlCommand("insert into period (grandprizeamt,startts,endts)" +
                "VALUES (:gpAmt, :bTS,:eTS) returning id", con);
            cmd.Parameters.Add(new NpgsqlParameter("gpAmt", lp.GrandPrizeAmount));
            cmd.Parameters.Add(new NpgsqlParameter("bTS", lp.PeriodBeginTS));
            cmd.Parameters.Add(new NpgsqlParameter("eTS", DateTime.Now));
            periodID = (int)cmd.ExecuteScalar(); //PERIOD table 1-record

            var unionResult = lp.losingTicketsL.Union(lp.winningTicketsL).ToList();

            while (unionResult.Any())
            {
                var sqlstr = new StringBuilder("insert into ticketsale " +
                    "(period_id,ballstring,ball1,ball2,ball3,ball4,ball5,powerball,winlevel,winamount,type,player) values ");
                using var cmd2 = new NpgsqlCommand();

                int batchSize = Math.Min(5000, unionResult.Count);
                foreach (var i in Enumerable.Range(0, batchSize))
                {
                    var l = unionResult[i];
                    sqlstr.Append($"(@pid{i},@bs{1},@b1{i},@b2{i},@b3{i},@b4{i},@b5{i},@bpower{i},@winL{i},@wina{i},@t{i},@pn{i})\n");
                    cmd2.Parameters.Add(new NpgsqlParameter($"pid{i}", periodID));
                    cmd2.Parameters.Add(new NpgsqlParameter($"bs{i}",
                        l.balls[0].ToString("00") + l.balls[1].ToString("00") +
                        l.balls[2].ToString("00") + l.balls[3].ToString("00") +
                        l.balls[4].ToString("00") + l.powerBall.ToString("00")));
                    cmd2.Parameters.Add(new NpgsqlParameter($"b1{i}", l.balls[0]));
                    cmd2.Parameters.Add(new NpgsqlParameter($"b2{i}", l.balls[1]));
                    cmd2.Parameters.Add(new NpgsqlParameter($"b3{i}", l.balls[2]));
                    cmd2.Parameters.Add(new NpgsqlParameter($"b4{i}", l.balls[3]));
                    cmd2.Parameters.Add(new NpgsqlParameter($"b5{i}", l.balls[4]));
                    cmd2.Parameters.Add(new NpgsqlParameter($"bpower{i}", l.powerBall));
                    cmd2.Parameters.Add(new NpgsqlParameter($"winL{i}", l.winLevel));
                    cmd2.Parameters.Add(new NpgsqlParameter($"wina{i}", l.winAmtDollars));
                    cmd2.Parameters.Add(new NpgsqlParameter($"t{i}", l.Type));
                    cmd2.Parameters.Add(new NpgsqlParameter($"pn{i}", l.Player));

                    if (i < batchSize - 1)
                        sqlstr.Append(",");
                }
                cmd2.CommandText = sqlstr.ToString();
                cmd2.Connection = con;
                cmd2.Prepare();
                cmd2.ExecuteNonQuery();
                unionResult.RemoveRange(0, batchSize);
            }
            con.Close();
            return periodID;//returns period.id as assigned by identity column in DB
        }
        public int DBLoosingTicketCountInPeriod(int pID)
        {
            var con = new NpgsqlConnection(cs);
            con.Open();
            var cmd = new NpgsqlCommand("select count(*) from ticketsale where winlevel=0 and period_id=:periodID", con);
            cmd.Parameters.Add(new NpgsqlParameter("periodID", pID));
            int r = Convert.ToInt32(cmd.ExecuteScalar()); //PERIOD count of loosing tickets
            con.Close();
            return r;
        }

        public int DBWinningTicketCountInPeriod(int pID)
        {
            var con = new NpgsqlConnection(cs);
            con.Open();
            var cmd = new NpgsqlCommand("select count(*) from ticketsale where winlevel>0 and period_id=:periodID", con);
            cmd.Parameters.Add(new NpgsqlParameter("periodID", pID));
            int r = Convert.ToInt32(cmd.ExecuteScalar()); //PERIOD count of loosing tickets
            con.Close();
            return r;
        }

        public IEnumerable<(int periodid, DateTime started)> DBPeriodsInHistory()
        {
            var con = new NpgsqlConnection(cs);
            con.Open();
            var cmd = new NpgsqlCommand("select id,startts from period order by 1,2", con);
            var res = cmd.ExecuteReader();
            List<(int periodID, DateTime started)> pers = new List<(int periodID, DateTime started)>();

            while (res.Read())
                pers.Add(((int)res[0], (DateTime)res[1]));

            con.Close();
            return pers;
        }

        public IEnumerable<TicketSale> DBStatsOnePeriod(int requestedPeriod)
        {
            var con = new NpgsqlConnection(cs);
            con.Open();
            var cmd = new NpgsqlCommand(
                @"with winlevel0 as (	select period_id,winlevel, count(*) num from ticketsale where winlevel=0 group by period_ID,winlevel),
winlevel1 as (select period_id, winlevel, count(*) num from ticketsale where winlevel = 1 group by period_ID, winlevel),
winlevel2 as (select period_id, winlevel, count(*) num from ticketsale where winlevel = 2 group by period_ID,winlevel),
winlevel3 as (select period_id, winlevel, count(*) num from ticketsale where winlevel = 3 group by period_ID,winlevel),
winlevel4 as (select period_id, winlevel, count(*) num from ticketsale where winlevel = 4 group by period_ID,winlevel),
winlevel5 as (select period_id, winlevel, count(*) num from ticketsale where winlevel = 5 group by period_ID,winlevel),
winlevel6 as (select period_id, winlevel, count(*) num from ticketsale where winlevel = 6 group by period_ID,winlevel),
winlevel7 as (select period_id, winlevel, count(*) num from ticketsale where winlevel = 7 group by period_ID,winlevel),
winlevel8 as (select period_id, winlevel, count(*) num from ticketsale where winlevel = 8 group by period_ID,winlevel),
winlevel9 as (select period_id, winlevel, count(*) num from ticketsale where winlevel = 9 group by period_ID,winlevel)
select
    id,
    grandprizeamt,
    startts,
    endts,
    coalesce(winlevel0.num,0) level0,
    coalesce(winlevel1.num,0) level1,
    coalesce(winlevel2.num,0) level2,
    coalesce(winlevel3.num,0) level3,
    coalesce(winlevel4.num,0) level4,
    coalesce(winlevel5.num,0) level5,
    coalesce(winlevel6.num,0) level6,
    coalesce(winlevel7.num,0) level7,
    coalesce(winlevel8.num,0) level8,
    coalesce(winlevel9.num,0) level9
from
    period
        left outer
join winlevel0 on (period.id = winlevel0.period_id)
left outer join winlevel1 on(period.id= winlevel1.period_id)
        left outer join winlevel3 on(period.id = winlevel3.period_id)
        left outer join winlevel4 on(period.id = winlevel4.period_id)
        left outer join winlevel2 on(period.id = winlevel2.period_id)
        left outer join winlevel5 on(period.id = winlevel5.period_id)
        left outer join winlevel6 on(period.id = winlevel6.period_id)
        left outer join winlevel7 on(period.id = winlevel7.period_id)
        left outer join winlevel8 on(period.id = winlevel8.period_id)
        left outer join winlevel9 on(period.id = winlevel9.period_id)
where period.id=:req_id
order by id", con);
            cmd.Parameters.Add(new NpgsqlParameter("req_id", requestedPeriod));
            var res = cmd.ExecuteReader();
            List<TicketSale> result = new List<TicketSale>();

            while (res.Read())
                result.Add(new TicketSale((int)res[0], (decimal)res[1], (DateTime)res[2], (DateTime)res[3],
                    (long)res[4], (long)res[5], (long)res[6], (long)res[7], (long)res[8],
                    (long)res[9], (long)res[10], (long)res[11], (long)res[12], (long)res[13]));

            con.Close();
            return result;
        }

        public IEnumerable<TicketSale> DBStatsAllPeriods()
        {
            var con = new NpgsqlConnection(cs);
            con.Open();
            var cmd = new NpgsqlCommand(
                @"with winlevel0 as (	select period_id,winlevel, count(*) num from ticketsale where winlevel=0 group by period_ID,winlevel),
winlevel1 as (select period_id, winlevel, count(*) num from ticketsale where winlevel = 1 group by period_ID, winlevel),
winlevel2 as (select period_id, winlevel, count(*) num from ticketsale where winlevel = 2 group by period_ID,winlevel),
winlevel3 as (select period_id, winlevel, count(*) num from ticketsale where winlevel = 3 group by period_ID,winlevel),
winlevel4 as (select period_id, winlevel, count(*) num from ticketsale where winlevel = 4 group by period_ID,winlevel),
winlevel5 as (select period_id, winlevel, count(*) num from ticketsale where winlevel = 5 group by period_ID,winlevel),
winlevel6 as (select period_id, winlevel, count(*) num from ticketsale where winlevel = 6 group by period_ID,winlevel),
winlevel7 as (select period_id, winlevel, count(*) num from ticketsale where winlevel = 7 group by period_ID,winlevel),
winlevel8 as (select period_id, winlevel, count(*) num from ticketsale where winlevel = 8 group by period_ID,winlevel),
winlevel9 as (select period_id, winlevel, count(*) num from ticketsale where winlevel = 9 group by period_ID,winlevel)
select
    id,
    grandprizeamt,
    startts,
    endts,
    coalesce(winlevel0.num,0) level0,
    coalesce(winlevel1.num,0) level1,
    coalesce(winlevel2.num,0) level2,
    coalesce(winlevel3.num,0) level3,
    coalesce(winlevel4.num,0) level4,
    coalesce(winlevel5.num,0) level5,
    coalesce(winlevel6.num,0) level6,
    coalesce(winlevel7.num,0) level7,
    coalesce(winlevel8.num,0) level8,
    coalesce(winlevel9.num,0) level9
from
    period
        left outer
join winlevel0 on (period.id = winlevel0.period_id)
left outer join winlevel1 on(period.id= winlevel1.period_id)
        left outer join winlevel3 on(period.id = winlevel3.period_id)
        left outer join winlevel4 on(period.id = winlevel4.period_id)
        left outer join winlevel2 on(period.id = winlevel2.period_id)
        left outer join winlevel5 on(period.id = winlevel5.period_id)
        left outer join winlevel6 on(period.id = winlevel6.period_id)
        left outer join winlevel7 on(period.id = winlevel7.period_id)
        left outer join winlevel8 on(period.id = winlevel8.period_id)
        left outer join winlevel9 on(period.id = winlevel9.period_id)
order by id", con);
            var res = cmd.ExecuteReader();
            List<TicketSale> result = new List<TicketSale>();

            while (res.Read())
                result.Add(new TicketSale((int)res[0], (decimal)res[1], (DateTime)res[2], (DateTime)res[3],
                    (long)res[4], (long)res[5], (long)res[6], (long)res[7], (long)res[8],
                    (long)res[9], (long)res[10], (long)res[11], (long)res[12], (long)res[13]));

            con.Close();
            return result;
        }
    }
}
