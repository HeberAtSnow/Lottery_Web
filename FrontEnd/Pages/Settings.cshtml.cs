using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassLib;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace FrontEnd.Pages
{

    public class SettingsModel : PageModel
    {
        LotteryProgram lp;

        public string ResetErrorMessage;
        public string ResetSuccessMessage = "";

        private readonly LotteryStatistics lotteryStatistics;
        public IEnumerable<LotteryTicket> WinLotteryTickets;
        public IEnumerable<TicketSale> CurrPeriodWinTickets;

        public IEnumerable<TicketSale> ticketSales;
        public IEnumerable<(int periodid, DateTime started)> LotteryPeriods;

        public ILogger<SettingsModel> logger { get; }

        //lotteryvendor.lotteryprogram
        //lotteryprogram has a lottery period
        public SettingsModel(LotteryProgram lotteryProgram, LotteryStatistics lotteryStatistics, ILogger<SettingsModel> logger)
        {
            lp = lotteryProgram;
            this.lotteryStatistics = lotteryStatistics;
            this.logger = logger;
        }
        public void OnGet()
        {

            logger.LogInformation("Somebody went to Administration page {time}", DateTime.Now);
        }

        public void OnPostResetLottery()
        {
            logger.LogInformation("Somebody clicked on ResetLottery {time}", DateTime.Now);

            try
            {
                if (lp.ResetPeriod())
                {
                    ResetSuccessMessage = "Successfully resetted the Lottery";
                    logger.LogInformation(ResetSuccessMessage);
                }
            }
            catch
            {
                ResetErrorMessage = "cant reset period when sales are not closed";
                logger.LogError(ResetErrorMessage);
            }


        }
        //draw winning numbers
        public void OnPostDrawWinningTickets()
        {
            var startTime = DateTime.Now;

            logger.LogInformation("Somebody clicked DrawLottery");

            lp.ClosePeriodSales();

            logger.LogInformation("ClosePeriodSales took {time} to process", DateTime.Now - startTime);

            lp.p.DrawWinningTicket();
            logger.LogInformation("DrawWinningTickets took {time} to process", DateTime.Now - startTime);

            lp.p.ComputeWinners();
            logger.LogInformation("ComputeWinners took {time} to process", DateTime.Now - startTime);

            logger.LogInformation("To finish DrawWinningTickets in total took {time} to process", DateTime.Now - startTime);
        }

    }

}
