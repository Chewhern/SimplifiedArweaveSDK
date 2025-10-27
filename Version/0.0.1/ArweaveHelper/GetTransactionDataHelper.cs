using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SimplifiedArweaveSDK.ArweaveHelper
{
    public static class GetTransactionDataHelper
    {
        public static String GetTransactionData(String id) 
        {
            String RetrievedData = "";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://arweave.net/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                var response = client.GetAsync($"tx/{id}/data");
                response.Wait();
                var result = response.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    readTask.Wait();

                    var Result = readTask.Result;

                    RetrievedData = Result;
                }
                else
                {
                    RetrievedData = "Invalid address.";
                }
            }
            return RetrievedData;
        }
    }
}
