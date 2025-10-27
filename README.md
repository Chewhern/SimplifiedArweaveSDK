Simplified, minimalist and cryptographically secure Arweave SDK for C#

**This is still a relatively new SDK, there may be bugs. I hope that this C# implementation can make the cryptographic related security more secure and more zero trust.**

# Credentials Handling (Loading RSA key parameter from file)
If the C# application runs on VPS hosting's environment, if the hosting's environment can be trusted, then the RSA key can be loaded into the library in **binary** manner by using **File.ReadAllBytes()**.

However, such a method most probably need to create 8 files all stored different binary data from RSA key parameters.

# Credentials Handling (Hardcoding RSA key paramater)
Since hardcoding method will be used, String like data is the easiest for RSA key parameter importing but String by itself is immutable data type. This means cryptographic hygiene can't be guaranteed.

The better and secure method will be
```
Byte[] Exponent = new Byte[] {};
Exponent = new Byte[] {0x01,0x00,0x01}; //<---- This way of initializing guarantees mutable data type throughout the process.
SodiumSecureMemory.SecureClearBytes(Exponent);
```

Personally, I prefer the second method but if one advances in application security (one sub branch of information security), how to properly **hardcode credentials** or something similar is something that operates
in a private and trusted zone manner **(security with obscurity such as anti-cheat,cheat,DRM,malware,anti-malware)** and might no longer operate in a **zero trust manner** (I may be wrong). 
