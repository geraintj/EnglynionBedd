namespace EnglynionBedd.Models
{
    public class GwybodaethDelwedd
    {
        public byte[] Delwedd { get; set; }
        public string status { get; set; }
        public bool succeeded { get; set; }
        public bool failed { get; set; }
        public bool finished { get;set; }
        public RecognitionResult recognitionResult {get;set;}
    }
}
