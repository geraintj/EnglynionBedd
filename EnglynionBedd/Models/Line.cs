using System.Collections.Generic;

namespace EnglynionBedd.Models
{
    public class Line
    {
        public List<int> boundingBox { get; set; }
        public string text { get; set; }
        public List<Word> words { get; set; }
    }
}