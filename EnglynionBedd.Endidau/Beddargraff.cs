using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace EnglynionBedd.Endidau
{
    public class Beddargraff
    {
        [JsonProperty(propertyName:"id")]
        public string Id { get; set; }
        [JsonProperty(propertyName:"enwBedd")]
        public string EnwBedd { get; set; }
        [JsonProperty(propertyName:"mynwent")]
        public string Mynwent { get; set; }
        [JsonProperty(propertyName:"dyddiad")]
        public string Dyddiad { get; set; }
        [JsonProperty(propertyName:"cyfeiriadDelwedd")]
        public string CyfeiriadDelwedd { get; set; }
        [JsonProperty(propertyName:"llinellau")]
        public List<string> Llinellau { get; set; }
        [JsonProperty(propertyName:"englynion")]
        public List<Englyn> Englynion { get; set; }
    }
}
