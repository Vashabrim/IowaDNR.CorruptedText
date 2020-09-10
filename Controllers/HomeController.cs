using IowaDNR.CorruptedText.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
            return View("Results", count);
        }

        public async Task<ResultsViewModel> SearchTheText(string results, string textToFind)
        {
            string pattern = "[^GCAT]";
            int cleanResults = CleanText(results, textToFind);
            var cleanText = Regex.Replace(results, pattern, string.Empty);
            IEnumerable<string> chunkText = ChunkText(cleanText, 4);
            List<Result> returnText = new List<Result>();
            ResultsViewModel model = new ResultsViewModel()
            {
                SearchText = chunkText.ToString(),
                TextToFind = textToFind,
            };
            foreach (var chunk in chunkText)
            {
                returnText.Add(new Result
                {
                    Chunk = chunk
                });
            }
            model.ReturnText = returnText;
            model.ResultsCount = cleanResults;
            return model;
        }

        private int CleanText(string txtToSearch, string txtToFind)
        {
            //Are lowercase 'gcat' characters valid?
            //Are the combination in any pattern, or broken up in the 4 character chunks only?
            int count = 0;
            foreach (Match match in Regex.Matches(txtToSearch, txtToFind.ToUpper()))
            {
                count++;
            };
            return count;
        }

        public IEnumerable<string> ChunkText(string str, int chunkSize)
        {
            return Enumerable.Range(0, str.Length / chunkSize).Select(i => str.Substring(i * chunkSize, chunkSize));
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
