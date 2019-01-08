using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using EnglynionBedd.Endidau;
using EnglynionBedd.Gwasanaethau.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace EnglynionBedd.Gwasanaethau
{
    public class GwasanaethauGwybodol : IGwasanaethauGwybodol
    {
        private readonly IOptions<Gosodiadau> _gosodiadau;

        public GwasanaethauGwybodol(IOptions<Gosodiadau> gosodiadau)
        {
            _gosodiadau = gosodiadau;
        }

        public async Task<GwybodaethDelwedd> DadansoddiTestun(byte[] delwedd, bool argraffedig)
        {
            try
            {
                var gwybodaeth = new GwybodaethDelwedd();
                
                HttpClient client = new HttpClient();

                // Request headers.
                client.DefaultRequestHeaders.Add(
                    "Ocp-Apim-Subscription-Key", _gosodiadau.Value.AllweddTanysgrifiad);

                // Request parameters. 
                // The language parameter doesn't specify a language, so the 
                // method detects it automatically.
                // The detectOrientation parameter is set to true, so the method detects and
                // and corrects text orientation before detecting text.
                string requestParameters = argraffedig ? "mode=Printed" : "Mode=Handwritten";

                // Assemble the URI for the REST API method.
                string uri = _gosodiadau.Value.CyfeiriadGwasanaeth + "?" + requestParameters;

                HttpResponseMessage response;

               
                // Add the byte array as an octet stream to the request body.
                using (ByteArrayContent content = new ByteArrayContent(delwedd))
                {
                    // This example uses the "application/octet-stream" content type.
                    // The other content types you can use are "application/json"
                    // and "multipart/form-data".
                    content.Headers.ContentType =
                        new MediaTypeHeaderValue("application/octet-stream");

                    // Asynchronously call the REST API method.
                    response = await client.PostAsync(uri, content);
                }

                var operationLocation = "";
                if (response.IsSuccessStatusCode)
                    operationLocation =
                        response.Headers.GetValues("Operation-Location").FirstOrDefault();
                else
                {
                    // Display the JSON error data.
                    string errorString = await response.Content.ReadAsStringAsync();
                    //Console.WriteLine("\n\nResponse:\n{0}\n",
                    //    JToken.Parse(errorString).ToString());
                    Console.WriteLine(errorString);
                }

                // Asynchronously get the JSON response.
                string contentString;
                int i = 0;
                do
                {
                    System.Threading.Thread.Sleep(1000);
                    var contentResponse = await client.GetAsync(operationLocation);
                    contentString = await contentResponse.Content.ReadAsStringAsync();
                    ++i;
                }
                while (i < 100 && contentString.IndexOf("\"status\":\"Succeeded\"") == -1);

                if (i == 100 && contentString.IndexOf("\"status\":\"Succeeded\"") == -1)
                {
                    Console.WriteLine("\nTimeout error.\n");
                    //eturn "Error";
                }

                gwybodaeth = JsonConvert.DeserializeObject<GwybodaethDelwedd>(contentString);
                return gwybodaeth;
            }
            catch (Exception e)
            {
                Console.WriteLine("\n" + e.Message);
            }
            return new GwybodaethDelwedd();
        }
    }
}