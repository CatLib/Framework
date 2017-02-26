using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;


public class HMacAes256 {

    public string Encrypt(string toEncrypt , string key)
    {
        
        RijndaelManaged aes = new RijndaelManaged();
        aes.KeySize = 256;
        aes.BlockSize = 256;
        aes.Padding = PaddingMode.PKCS7;
        aes.Mode = CipherMode.CBC;
        aes.Key = Encoding.UTF8.GetBytes(key);
        aes.GenerateIV();

        ICryptoTransform aesEncrypt = aes.CreateEncryptor(aes.Key, aes.IV);
        byte[] buffer = Encoding.UTF8.GetBytes(toEncrypt);
        buffer = aesEncrypt.TransformFinalBlock(buffer, 0, buffer.Length);

        string aesBufferString = Convert.ToBase64String(buffer);
        string ivString = Convert.ToBase64String(aes.IV);
        string aesIVString = Convert.ToBase64String(Encoding.UTF8.GetBytes(aesBufferString + "$" + ivString));

        return aesIVString + "$" + HMac(aesIVString, key);
        
    }

    public string Decrypt(string toDecrypt, string key)
    {
        var hmac = toDecrypt.Split('$');
        if (hmac.Length < 2) { throw new Exception("mac is invalid"); }
        if (HMac(hmac[0], key) != hmac[1]) { throw new Exception("mac is invalid"); }

        string aesIVString = Encoding.UTF8.GetString(Convert.FromBase64String(hmac[0]));
        var aesIVStringArr = aesIVString.Split('$');
        if (aesIVStringArr.Length < 2) { throw new Exception("mac is invalid"); }

        byte[] aesBuffer = Convert.FromBase64String(aesIVStringArr[0]);
        byte[] ivBuffer = Convert.FromBase64String(aesIVStringArr[1]);

        RijndaelManaged aes = new RijndaelManaged();
        aes.KeySize = 256;
        aes.BlockSize = 256;
        aes.Padding = PaddingMode.PKCS7;
        aes.Mode = CipherMode.CBC;
        aes.Key = Encoding.UTF8.GetBytes(key);
        aes.IV = ivBuffer;

        ICryptoTransform aesDecrypt = aes.CreateDecryptor(aes.Key, aes.IV);
        aesBuffer = aesDecrypt.TransformFinalBlock(aesBuffer, 0, aesBuffer.Length);

        return Encoding.UTF8.GetString(aesBuffer);
    }

    private string HMac(string toEncrypt , string key)
    {
        HMACSHA256 hmacSha256 = new HMACSHA256(Encoding.UTF8.GetBytes(key));
        return HashAlgorithmBase(hmacSha256, toEncrypt, Encoding.UTF8);
    }

    private string Bytes2Str(IEnumerable<byte> source, string formatStr = "{0:X2}")
    {
        StringBuilder pwd = new StringBuilder();
        foreach (byte btStr in source) { pwd.AppendFormat(formatStr, btStr); }
        return pwd.ToString();
    }

    private string HashAlgorithmBase(HashAlgorithm hashAlgorithmObj, string source, Encoding encoding)
    {
        byte[] btStr = encoding.GetBytes(source);
        byte[] hashStr = hashAlgorithmObj.ComputeHash(btStr);
        return Bytes2Str(hashStr);
    }

}
