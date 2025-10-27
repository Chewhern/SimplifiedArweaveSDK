using ASodium;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using SimplifiedArweaveSDK.ArweaveHelper;
using SimplifiedArweaveSDK.ArweaveModel;
using SimplifiedArweaveSDK.ArweaveSubHelper;
using SimplifiedArweaveSDK.RSAPSSHelper;


namespace SimplifiedArweaveSDK.ArweaveLocalHelper
{
    public static class ArweaveSecureCreateAndPostDataHelper
    {
        public static async Task<String> UploadData(Object Data,String OwnerRSAModulusString, Byte[] ModulusBytes, Byte[] ExponentBytes, Byte[] DBytes, Byte[] PBytes, Byte[] QBytes, Byte[] DPBytes, Byte[] DQBytes, Byte[] InverseQBytes) 
        {
            Byte[] DataBytes = new Byte[] { };
            if (Data is String TestString) 
            {
                DataBytes = Encoding.UTF8.GetBytes((String)Data);
                //12 MiB
                if(DataBytes.Length> 12582912) 
                {
                    throw new ArgumentException("Error: Data size exceeded 12 MB/MiB");
                }
            }
            else if(Data is Byte[] TestBytes) 
            {
                DataBytes = TestBytes;
                if (DataBytes.Length > 12582912)
                {
                    throw new ArgumentException("Error: Data size exceeded 12 MB/MiB");
                }
            }
            else 
            {
                throw new ArgumentException("Error: Data must either be in string or Byte[]");
            }
            TransactionModel MyTransaction = new TransactionModel();
            MyTransaction.format = 2;
            MyTransaction.last_tx = GetTransactionAnchorHelper.GetTransactionAnchor();
            MyTransaction.owner = OwnerRSAModulusString;
            MyTransaction.tags = new TagsModel[] { };
            MyTransaction.target = "";
            MyTransaction.quantity = "0";
            MyTransaction.data_root = Base64URLEncodeDecodeHelper.Encode(SodiumHelper.HexToBinary(ArweaveDataRootHelper.ComputeDataRoot(DataBytes)));
            MyTransaction.data_size = DataBytes.Length.ToString();
            MyTransaction.data = Base64URLEncodeDecodeHelper.Encode(DataBytes);
            MyTransaction.reward = GetTransactionPriceHelper.GetTransactionPrice(MyTransaction.data_size, "");
            var tagList = new List<List<byte[]>>();
            var items = new List<Object>
            {
                Encoding.UTF8.GetBytes(MyTransaction.format.ToString()),
                Base64URLEncodeDecodeHelper.Decode(MyTransaction.owner),
                Encoding.UTF8.GetBytes(MyTransaction.target),
                Encoding.UTF8.GetBytes(MyTransaction.quantity),
                Encoding.UTF8.GetBytes(MyTransaction.reward),
                Base64URLEncodeDecodeHelper.Decode(MyTransaction.last_tx),
                tagList,
                Encoding.UTF8.GetBytes(MyTransaction.data_size),
                Base64URLEncodeDecodeHelper.Decode(MyTransaction.data_root)
            };
            Byte[] DeepHashResult = await ArweaveDeepHashHelper.ComputeAsync(items);
            Byte[] SignatureData = CurrentRSAOpsHelper.SignData(DeepHashResult, ModulusBytes, ExponentBytes, DBytes, PBytes, QBytes, DPBytes, DQBytes, InverseQBytes);
            MyTransaction.signature = Base64URLEncodeDecodeHelper.Encode(SignatureData);
            Byte[] HashedSignatureData = SHA256.HashData(SignatureData);
            MyTransaction.id = Base64URLEncodeDecodeHelper.Encode(HashedSignatureData);
            return await UploadDataHelper.UploadData(JsonConvert.SerializeObject(MyTransaction));
        }

        /*
        public static async Task<String> SendAR(String OwnerRSAModulusString,String TargetUserAddress,String QuantityString) 
        {
            TransactionModel MyTransaction = new TransactionModel();
            MyTransaction.format = 2;
            MyTransaction.last_tx = GetTransactionAnchorHelper.GetTransactionAnchor();
            MyTransaction.owner = OwnerRSAModulusString;
            MyTransaction.tags = new TagsModel[] { };
            MyTransaction.target = TargetUserAddress;
            MyTransaction.quantity = QuantityString;
            MyTransaction.data_root = "";
            MyTransaction.data_size = "0";
            MyTransaction.data = "";
            MyTransaction.reward = GetTransactionPriceHelper.GetTransactionPrice(MyTransaction.data_size, TargetUserAddress);
            var tagList = new List<List<byte[]>>();
            var items = new List<Object>
            {
                Encoding.UTF8.GetBytes(MyTransaction.format.ToString()),
                Base64URLEncodeDecodeHelper.Decode(MyTransaction.owner),
                Encoding.UTF8.GetBytes(MyTransaction.target),
                Encoding.UTF8.GetBytes(MyTransaction.quantity),
                Encoding.UTF8.GetBytes(MyTransaction.reward),
                Base64URLEncodeDecodeHelper.Decode(MyTransaction.last_tx),
                tagList,
                Encoding.UTF8.GetBytes(MyTransaction.data_size),
                Base64URLEncodeDecodeHelper.Decode(MyTransaction.data_root)
            };
            Byte[] DeepHashResult = await ArweaveDeepHashHelper.ComputeAsync(items);
            Byte[] SignatureData = CurrentRSAOpsHelper.SignData(DeepHashResult);
            MyTransaction.signature = Base64URLEncodeDecodeHelper.Encode(SignatureData);
            Byte[] HashedSignatureData = SHA256.HashData(SignatureData);
            MyTransaction.id = Base64URLEncodeDecodeHelper.Encode(HashedSignatureData);
            return await UploadDataHelper.UploadData(JsonConvert.SerializeObject(MyTransaction));
        }

        public static async Task<String> SendARAndUploadData(Object Data, String OwnerRSAModulusString, String TargetUserAddress, String QuantityString)
        {
            Byte[] DataBytes = new Byte[] { };
            if (Data is String TestString)
            {
                DataBytes = Encoding.UTF8.GetBytes((String)Data);
                //12 MiB
                if (DataBytes.Length > 12582912)
                {
                    throw new ArgumentException("Error: Data size exceeded 12 MB/MiB");
                }
            }
            else if (Data is Byte[] TestBytes)
            {
                DataBytes = TestBytes;
                if (DataBytes.Length > 12582912)
                {
                    throw new ArgumentException("Error: Data size exceeded 12 MB/MiB");
                }
            }
            else
            {
                throw new ArgumentException("Error: Data must either be in string or Byte[]");
            }
            TransactionModel MyTransaction = new TransactionModel();
            MyTransaction.format = 2;
            MyTransaction.last_tx = GetTransactionAnchorHelper.GetTransactionAnchor();
            MyTransaction.owner = OwnerRSAModulusString;
            MyTransaction.tags = new TagsModel[] { };
            MyTransaction.target = TargetUserAddress;
            MyTransaction.quantity = QuantityString;
            MyTransaction.data_root = Base64URLEncodeDecodeHelper.Encode(SodiumHelper.HexToBinary(ArweaveDataRootHelper.ComputeDataRoot(DataBytes)));
            MyTransaction.data_size = DataBytes.Length.ToString();
            MyTransaction.data = Base64URLEncodeDecodeHelper.Encode(DataBytes);
            MyTransaction.reward = GetTransactionPriceHelper.GetTransactionPrice(MyTransaction.data_size, TargetUserAddress);
            var tagList = new List<List<byte[]>>();
            var items = new List<Object>
            {
                Encoding.UTF8.GetBytes(MyTransaction.format.ToString()),
                Base64URLEncodeDecodeHelper.Decode(MyTransaction.owner),
                Encoding.UTF8.GetBytes(MyTransaction.target),
                Encoding.UTF8.GetBytes(MyTransaction.quantity),
                Encoding.UTF8.GetBytes(MyTransaction.reward),
                Base64URLEncodeDecodeHelper.Decode(MyTransaction.last_tx),
                tagList,
                Encoding.UTF8.GetBytes(MyTransaction.data_size),
                Base64URLEncodeDecodeHelper.Decode(MyTransaction.data_root)
            };
            Byte[] DeepHashResult = await ArweaveDeepHashHelper.ComputeAsync(items);
            Byte[] SignatureData = CurrentRSAOpsHelper.SignData(DeepHashResult);
            MyTransaction.signature = Base64URLEncodeDecodeHelper.Encode(SignatureData);
            Byte[] HashedSignatureData = SHA256.HashData(SignatureData);
            MyTransaction.id = Base64URLEncodeDecodeHelper.Encode(HashedSignatureData);
            return await UploadDataHelper.UploadData(JsonConvert.SerializeObject(MyTransaction));
        }
        */
    }
}
