using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FunctionalFitness.Models.VideoViewModels
{
    public class VideoSearchViewModel
    {
        public List<Video> List { get; set; }
        public string SearchText { get; set; }
    }
}
