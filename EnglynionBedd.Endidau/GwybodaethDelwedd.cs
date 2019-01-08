using System.Collections.Generic;

namespace EnglynionBedd.Endidau
{
    public class GwybodaethDelwedd
    {
        public string CyfeiriadDelwedd { get; set; }
        public string EnwBedd { get; set; }
        public string Mynwent { get; set; }
        public string Dyddiad { get; set; }
        public List<string> Llinellau { get; set; }
        public RecognitionResult recognitionResult {get;set;}
    }
}
