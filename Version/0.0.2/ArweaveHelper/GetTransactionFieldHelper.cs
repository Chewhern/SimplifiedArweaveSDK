using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using SimplifiedArweaveSDK.ArweaveModel;

namespace SimplifiedArweaveSDK.ArweaveHelper
{
    public static class GetTransactionFieldHelper
    {
        public static String GetTransactionField(String id,String field)
        {
            String RetrievedData = "";
            if(field!=null && field.Contains("id") || field.Contains("last_tx") || field.Contains("owner")
                || field.Contains("target") || field.Contains("quantity") || field.Contains("data")
                || field.Contains("data_root") || field.Contains("data_size") || field.Contains("reward")
                || field.Contains("signature")) 
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://arweave.net/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json"));
                    var response = client.GetAsync($"tx/{id}/{field}");
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
            }
            return RetrievedData;
        }

        public static Object[] GetTransactionTags(String id, String field)
        {
            Object[] RetrievedData = new Object[] { };
            if (field != null && field.Contains("tags"))
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://arweave.net/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json"));
                    var response = client.GetAsync($"tx/{id}/{field}");
                    response.Wait();
                    var result = response.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsStringAsync();
                        readTask.Wait();

                        var Result = readTask.Result;

                        RetrievedData = JsonConvert.DeserializeObject<Object[]>(Result);
                    }
                }
            }
            return RetrievedData;
        }
    }
}
