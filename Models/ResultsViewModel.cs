using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IowaDNR.CorruptedText.Models
{
    public class ResultsViewModel
    {
        public string SearchText { get; set; }
        public string TextToFind { get; set; }
        public int ResultsCount { get; set; }
    }
}
