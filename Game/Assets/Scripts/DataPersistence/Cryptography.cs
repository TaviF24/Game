using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Security.Cryptography;
using System.IO;

public class Cryptography
{
    public byte[] Encode(byte[] bytes, byte[] key, byte[] vector)
    {
        Aes aes = Aes.Create();
        ICryptoTransform encryptor = aes.CreateEncryptor(key, vector);
        MemoryStream memoryStream = new MemoryStream();
        CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
        cryptoStream.Write(bytes, 0, bytes.Length);
        cryptoStream.Close();
        return memoryStream.ToArray();
    }

    public byte[] Decode(byte[] bytes, byte[] key, byte[] vector)
    {
        Aes aes = Aes.Create();
        ICryptoTransform decryptor = aes.CreateDecryptor(key, vector);
        MemoryStream memoryStream = new MemoryStream();
        CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Write);
        cryptoStream.Write(bytes, 0, bytes.Length);
        cryptoStream.Close();
        return memoryStream.ToArray();
    }
}