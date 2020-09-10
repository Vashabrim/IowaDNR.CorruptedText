using IowaDNR.CorruptedText.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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
            Microsoft.Extensions.Primitives.StringValues txtToSearch = HttpContext.Request.Form["TextToSearch"];
            Microsoft.Extensions.Primitives.StringValues txtToFind = HttpContext.Request.Form["SearchCombo"];
            ResultsViewModel count = await SearchTheText(txtToSearch, txtToFind);
            return Content("Hello, the combination " + txtToFind + " was found " + count.ResultsCount.ToString() + " times");
        }

        public async Task<ResultsViewModel> SearchTheText(string results, string textToFind)
        {
            int cleanResults = CleanText(results, textToFind);
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
            int count = 0;
            string cleanText = Regex.Replace(textToClean, pattern, string.Empty);
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
