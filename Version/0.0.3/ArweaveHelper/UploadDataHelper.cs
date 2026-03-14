using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

            HttpStatusCode MyStatusCode = response.StatusCode;

            if (MyStatusCode == HttpStatusCode.OK)
            {
                StatusString = "OK";
            }
            else if (MyStatusCode == HttpStatusCode.AlreadyReported)
            {
                StatusString = "Transaction already processed.";
            }
            else if (MyStatusCode == HttpStatusCode.BadRequest)
            {
                StatusString = "Transaction verification failed.";
            }
            else if (MyStatusCode == HttpStatusCode.TooManyRequests)
            {
                StatusString = "Too Many Requests";
            }
            else
            {
                StatusString = "Transaction verification failed.";
            }

            return StatusString;
        }
    }
}
