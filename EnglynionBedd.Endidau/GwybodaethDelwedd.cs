using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EnglynionBedd.Endidau
{
    public class GwybodaethDelwedd
    {
        public string CyfeiriadDelwedd { get; set; }
        [Display(Name = "Enw Bedd")]
        public string EnwBedd { get; set; }
        [Display(Name = "Mynwent")]
        public string Mynwent { get; set; }
        [Display(Name = "Dyddiad")]
        public string Dyddiad { get; set; }
        public List<string> Llinellau { get; set; }
        public RecognitionResult recognitionResult {get;set;}
    }
}
