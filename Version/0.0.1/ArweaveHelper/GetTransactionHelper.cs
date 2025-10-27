using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using SimplifiedArweaveSDK.ArweaveModel;

namespace SimplifiedArweaveSDK.ArweaveHelper
{
    public static class GetTransactionHelper
    {
        public static GetTransactionModel GetTransaction(String id)
        {
            GetTransactionModel MyModel = new GetTransactionModel();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://arweave.net/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                var response = client.GetAsync($"tx/{id}");
                response.Wait();
                var result = response.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    readTask.Wait();

                    var Result = readTask.Result;

                    MyModel = JsonConvert.DeserializeObject<GetTransactionModel>(Result);
                }
            }
            return MyModel;
        }
    }
}
