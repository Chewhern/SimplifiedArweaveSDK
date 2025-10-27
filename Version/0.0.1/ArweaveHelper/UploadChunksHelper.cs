using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SimplifiedArweaveSDK.ArweaveHelper
{
    public static class UploadChunksHelper
    {
        //Need to test..
        //Can only 25KB
        public static async Task<string> UploadChunk(string Path)
        {
            string StatusString = "";

            var httpClient = new HttpClient();
            var fileBytes = File.ReadAllBytes(Path);
            String ConvertedfileBytes = Convert.ToBase64String(fileBytes);
            var content = new StringContent(ConvertedfileBytes);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await httpClient.PostAsync("https://arweave.net/chunk", content);

            StatusString = await response.Content.ReadAsStringAsync();

            return StatusString;
        }
    }
}
