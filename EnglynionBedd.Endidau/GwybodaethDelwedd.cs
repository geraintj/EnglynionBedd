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
        [Display(Name = "Llinellau'r englyn")]
        public string Llinell1 { get; set; }
        public string Llinell2 { get; set; }
        public string Llinell3 { get; set; }
        public string Llinell4 { get; set; }
        [Display(Name = "Bardd")]
        public string Bardd { get; set; }
        public RecognitionResult recognitionResult {get;set;}
    }
}
