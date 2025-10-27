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
    public static class GetTransactionOffsetHelper
    {
        public static GetTransactionOffsetModel GetTransactionOffset(String id)
        {
            GetTransactionOffsetModel RetrievedData = new GetTransactionOffsetModel();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://arweave.net/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                var response = client.GetAsync($"tx/{id}/offset");
                response.Wait();
                var result = response.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    readTask.Wait();

                    var Result = readTask.Result;

                    RetrievedData = JsonConvert.DeserializeObject<GetTransactionOffsetModel>(Result);
                }
            }
            return RetrievedData;
        }
    }
}
