using System.Collections.Generic;

namespace EnglynionBedd.Endidau
{
    public class Line
    {
        public List<int> boundingBox { get; set; }
        public string text { get; set; }
        public List<Word> words { get; set; }
    }
}