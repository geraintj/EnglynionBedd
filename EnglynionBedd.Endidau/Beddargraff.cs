using System;
using System.Collections.Generic;
using System.Text;

namespace EnglynionBedd.Endidau
{
    public class Beddargraff
    {
        public string EnwBedd { get; set; }
        public string Mynwent { get; set; }
        public string Dyddiad { get; set; }
        public List<string> Llinellau { get; set; }
        public List<Englyn> Englynion { get; set; }
    }
}
