using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using IowaDNR.CorruptedText.Models;
using System.Text.RegularExpressions;

namespace IowaDNR.CorruptedText.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SearchText()
        {
            var txtToSearch = HttpContext.Request.Form["TextToSearch"];
            var txtToFind = HttpContext.Request.Form["SearchCombo"];
            var count = await SearchTheText(txtToSearch, txtToFind);
            return Content("Hello, the combination " + txtToFind + " was found " + count.ResultsCount.ToString() + " times");
        }

        public async Task<ResultsViewModel> SearchTheText(string results, string textToFind)
        {
            var cleanResults = CleanText(results, textToFind);
            ResultsViewModel model = new ResultsViewModel()
            {
                SearchText = results,
                TextToFind = textToFind,
            };
            model.ResultsCount = cleanResults;
            return model;
        }

        private int CleanText(string textToClean, string txtToFind)
        {
            string pattern = "[^GCAT]";
            //Are lowercase 'gcat' characters valid?
            var count = 0;
            var cleanText = Regex.Replace(textToClean, pattern, string.Empty);
            foreach (Match match in Regex.Matches(cleanText, txtToFind.ToUpper()))
            {
                count++;
            };
            return count;
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
