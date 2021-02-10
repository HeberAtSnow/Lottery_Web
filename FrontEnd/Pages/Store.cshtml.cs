using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;

namespace FrontEnd.Pages
{
    public class StoreModel : PageModel
    {
        private IMemoryCache _cache;
        private string cacheKey = "Selection";
        private string cacheValue;
        public string Selection => cacheValue ?? "";

        public StoreModel(IMemoryCache cache)
        {
            _cache = cache;
        }

        public void OnGet()
        {
            _cache.TryGetValue(cacheKey, out cacheValue);
        }

        public IActionResult OnPostQuickPick()
        {
            cacheValue = "QuickPick";
            _cache.Set(cacheKey, cacheValue, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(10)));
            return RedirectToPage();
        }
        public IActionResult OnPostNumberPick()
        {
            cacheValue = "NumberPick";
            _cache.Set(cacheKey, cacheValue, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(10)));
            return RedirectToPage();
        }

    }
}
