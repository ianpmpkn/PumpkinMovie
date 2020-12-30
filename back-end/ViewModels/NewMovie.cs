using System.Collections.Generic;

namespace Pumpkinmovies.ViewModels
{
    public class NewMovie
    {
        public string MovieName { get; set; }
        public string ReleaseTime { get; set; }
        public int MovieLength { get; set; }
        public string MovieArea { get; set; }
        public string MovieSummary { get; set; }
        public string MovieIMDb { get; set; }
    }
}