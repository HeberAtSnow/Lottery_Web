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
        public string testConn()
        {
            var cs = "Host=localhost;Username=postgres;Password=mysecretpassword;Database=postgres";
            using var con = new NpgsqlConnection(cs);
            con.Open();
            var sql = "SELECT version()";
            using var cmd = new NpgsqlCommand(sql, con);
            var version = cmd.ExecuteScalar().ToString();
            return $"PostgreSQL version: {version}";
        }





    }

}
