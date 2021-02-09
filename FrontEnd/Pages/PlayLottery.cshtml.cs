using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrontEnd.Pages
{
    public class PlayLotteryModel : PageModel
    {

        public void OnGet()
        {
        }
        public IActionResult OnPostGetData(String name, int num1, int num2, int num3, int num4, int num5, int num6, int powerball )
        {

            /* Response.Cookies["userInfo"]["userName"] = "patrick"; //userInfo is the cookie, userName is the subkey
             Response.Cookies["userInfo"]["lastVisit"] = DateTime.Now.ToString(); //now lastVisit is the subkey
             Response.Cookies["userInfo"].Expires = DateTime.Now.AddDays(1);

             HttpCookie aCookie = new HttpCookie("userInfo");
             aCookie.Values["userName"] = "patrick";
             aCookie.Values["lastVisit"] = DateTime.Now.ToString();
             aCookie.Expires = DateTime.Now.AddDays(1);
             Response.Cookies.Add(aCookie);*/





            return RedirectToPage();
        }
    }
}
