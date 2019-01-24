using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace EnglynionBedd.Endidau
{
    public class Englyn
    {
        [JsonProperty(propertyName: "id")]
        public string Id { get; set; }
        [JsonProperty(propertyName: "bedd")]
        public string Bedd { get; set; }
        [JsonProperty(propertyName: "mynwent")]
        public string Mynwent { get; set; }
        [JsonProperty(propertyName: "dyddiad")]
        public string Dyddiad { get; set; }
        [JsonProperty(propertyName: "cyfeiriadDelwedd")]
        public string CyfeiriadDelwedd { get; set; }
        [JsonProperty(propertyName: "llinell1")]
        public string Llinell1 { get; set; }
        [JsonProperty(propertyName: "llinell2")]
        public string Llinell2 { get; set; }
        [JsonProperty(propertyName: "llinell3")]
        public string Llinell3 { get; set; }
        [JsonProperty(propertyName: "llinell4")]
        public string Llinell4 { get; set; }
        [JsonProperty(propertyName: "bardd")]
        public string Bardd { get; set; }

        public static explicit operator Englyn(Microsoft.Azure.Documents.Document v)
        {
            return new Englyn()
            {
                Id = v.ResourceId,
                Bedd = v.GetPropertyValue<string>("bedd"),
                Mynwent = v.GetPropertyValue<string>("mynwent"),
                Dyddiad = v.GetPropertyValue<string>("dyddiad"),
                CyfeiriadDelwedd = v.GetPropertyValue<string>("cyfeiriadDelwedd"),
                Llinell1 = v.GetPropertyValue<string>("llinell1"),
                Llinell2 = v.GetPropertyValue<string>("llinell2"),
                Llinell3 = v.GetPropertyValue<string>("llinell3"),
                Llinell4 = v.GetPropertyValue<string>("llinell4"),
                Bardd = v.GetPropertyValue<string>("bardd")
            };
        }
    }
}
