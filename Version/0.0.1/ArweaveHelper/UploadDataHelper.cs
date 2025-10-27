using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SimplifiedArweaveSDK.ArweaveHelper
{
    public static class UploadDataHelper
    {
        public static async Task<string> UploadData(string Data)
        {
            string StatusString = "";

            var httpClient = new HttpClient();
            var content = new StringContent(Data);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await httpClient.PostAsync("https://arweave.net/tx", content);

            StatusString = await response.Content.ReadAsStringAsync();

            return StatusString;
        }
    }
}
