using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SimplifiedArweaveSDK.ArweaveHelper
{
    public static class GetBlockByHashHelper
    {
        //Raw JSON Data
        //Without proper Model format unable to deserialize it properly
        public static String GetBlockByHash(String HashID) 
        {
            String RetrievedData = "";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://arweave.net/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                var response = client.GetAsync($"block/hash/{HashID}");
                response.Wait();
                var result = response.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    readTask.Wait();

                    var Result = readTask.Result;

                    RetrievedData = Result;
                    
                }
            }
            return RetrievedData;
        }

        public static DateTime GetTransactionDateTime(String JSONData) 
        {
            var document = JsonDocument.Parse(JSONData);
            long UnixSeconds = document.RootElement
                                     .GetProperty("timestamp")
                                     .GetInt64();
            DateTime TransactionDateTime = DateTimeOffset.FromUnixTimeSeconds(UnixSeconds).UtcDateTime.AddHours(8);

            return TransactionDateTime;
        }
    }
}
