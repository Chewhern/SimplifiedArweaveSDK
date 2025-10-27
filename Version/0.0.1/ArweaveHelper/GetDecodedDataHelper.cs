using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SimplifiedArweaveSDK.ArweaveHelper
{
    //Need to test
    public static class GetDecodedDataHelper
    {
        public static async Task<string> GetDecodedData(string id,string filename,string fileextension)
        {
            string StatusString = "";
            string directory = "";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                directory = AppContext.BaseDirectory + "\\Directory\\";
            }
            else
            {
                directory = AppContext.BaseDirectory + "/Directory/";
            }
            string downloadUrl = "https://arweave.net/" + $"tx/{id}";
            var fileSavePath = directory + filename + fileextension;

            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(downloadUrl);

            if (response.IsSuccessStatusCode)
            {
                await using var stream = await response.Content.ReadAsStreamAsync();
                await using var fileStream = File.Create(fileSavePath);
                await stream.CopyToAsync(fileStream);
                StatusString = $"File saved to: {fileSavePath}";
            }
            else
            {
                StatusString = "Download failed: " + await response.Content.ReadAsStringAsync();
            }

            return StatusString;
        }

        public static async Task<string> GetDecodedDataWithFileExtension(string id, string filename, string fileextension)
        {
            string StatusString = "";
            string directory = "";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                directory = AppContext.BaseDirectory + "\\Directory\\";
            }
            else
            {
                directory = AppContext.BaseDirectory + "/Directory/";
            }
            string downloadUrl = "https://arweave.net/" + $"tx/{id}/data.{fileextension}";
            var fileSavePath = directory + filename + fileextension;

            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(downloadUrl);

            if (response.IsSuccessStatusCode)
            {
                await using var stream = await response.Content.ReadAsStreamAsync();
                await using var fileStream = File.Create(fileSavePath);
                await stream.CopyToAsync(fileStream);
                StatusString = $"File saved to: {fileSavePath}";
            }
            else
            {
                StatusString = "Download failed: " + await response.Content.ReadAsStringAsync();
            }

            return StatusString;
        }
    }
}
