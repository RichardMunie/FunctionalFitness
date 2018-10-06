using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;


namespace FunctionalFitness.Models
{
    public class Video
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Url { get; set; }
        public string Thumbnail { get; set; }
        public int ReleaseDays { get; set; }
        public int ID { get; set; }

        public string EmbedUrl()
        {
            int i = this.Url.Length;
            List<char> urlChars = this.Url.ToList();
            StringBuilder myStringBuilder = new StringBuilder();
            for (int j = 31; j < i; j++)
            {
                myStringBuilder.Append(urlChars[j]);
            }
            string newUrl = "https://www.youtube.com/embed/" + myStringBuilder;

            return newUrl;
        }
    }
}
