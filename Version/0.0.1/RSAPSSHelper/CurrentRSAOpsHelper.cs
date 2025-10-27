using ASodium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SimplifiedArweaveSDK.RSAPSSHelper
{
    public static class CurrentRSAOpsHelper
    {
        public static Byte[] SignData(Byte[] Data, Byte[] ModulusBytes, Byte[] ExponentBytes, Byte[] DBytes, Byte[] PBytes, Byte[] QBytes, Byte[] DPBytes, Byte[] DQBytes, Byte[] InverseQBytes) 
        {
            var rsaParams = new RSAParameters();

            //Example Initialization
            //rsaParams.Exponent = new Byte[] {0x01,0x00,0x01};
            //This method of hardcoding treats the file system of any device as malicious
            //If the file system of any device can be trusted, then loading the private key parameters from file in binary format
            //can be used.
            //This way, it ensures all operations are mutable inside the memory or computer.
            //It'll only be possible to clear the parameters securely via proper libraries
            //as the default language's library clearing functions won't work for cryptography related hygiene. 
            rsaParams.Modulus = ModulusBytes;
            rsaParams.Exponent = ExponentBytes;
            rsaParams.D = DBytes;
            rsaParams.P = PBytes;
            rsaParams.Q = QBytes;
            rsaParams.DP = DPBytes;
            rsaParams.DQ = DQBytes;
            rsaParams.InverseQ = InverseQBytes;

            using var rsa = RSA.Create();
            rsa.ImportParameters(rsaParams);

            var signature = rsa.SignData(Data, HashAlgorithmName.SHA256, RSASignaturePadding.Pss);

            SodiumSecureMemory.SecureClearBytes(rsaParams.Modulus);
            SodiumSecureMemory.SecureClearBytes(rsaParams.Exponent);
            SodiumSecureMemory.SecureClearBytes(rsaParams.D);
            SodiumSecureMemory.SecureClearBytes(rsaParams.P);
            SodiumSecureMemory.SecureClearBytes(rsaParams.Q);
            SodiumSecureMemory.SecureClearBytes(rsaParams.DP);
            SodiumSecureMemory.SecureClearBytes(rsaParams.DQ);
            SodiumSecureMemory.SecureClearBytes(rsaParams.InverseQ);

            return signature;
        }

        
    }
}
