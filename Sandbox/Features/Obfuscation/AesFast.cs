using System.Security.Cryptography;
using System.Text;

namespace Sandbox.Features.Obfuscation;

/// <summary>
/// Uses AES in ECB mode for non-cryptographic functions. This was to see what performance penalties would need to
/// be paid to use AES vs something simpler like base64.
/// </summary>
public sealed class AesFast : IDisposable
{
    private byte[] _key;
    private Aes _aes;
    private Lazy<ICryptoTransform> _encryptor;
    private Lazy<ICryptoTransform> _decryptor;
    private bool _disposed = false;

    public AesFast(string keyString, int keySize = 256)
    {
        if (!CreateKey(keyString, keySize))
        {
            throw new ArgumentException("Invalid key or key size.");
        }
        InitAes();
    }

    private bool CreateKey(string keyString, int keySize)
    {
        if (keySize != 128 && keySize != 192 && keySize != 256)
        {
            return false;
        }

        _key = new byte[keySize / 8];
        byte[] providedBytes = Encoding.UTF8.GetBytes(keyString);
        Array.Copy(providedBytes, _key, Math.Min(_key.Length, providedBytes.Length));
        return true;
    }

    private void InitAes()
    {
        _aes = Aes.Create();
        _aes.Mode = CipherMode.ECB;
        _aes.Key = _key;
        _aes.Padding = PaddingMode.PKCS7;
        
        _encryptor = new Lazy<ICryptoTransform>(() => _aes.CreateEncryptor(_aes.Key, _aes.IV));
        _decryptor = new Lazy<ICryptoTransform>(() => _aes.CreateDecryptor(_aes.Key, _aes.IV));
    }

    public byte[] Encrypt(Span<byte> plainBytes)
    {
        if (plainBytes.IsEmpty)
        {
            throw new ArgumentException("Plain bytes cannot be empty.", nameof(plainBytes));
        }

        using var ms = new MemoryStream();
        using var cs = new CryptoStream(ms, _encryptor.Value, CryptoStreamMode.Write);
        cs.Write(plainBytes);
        cs.FlushFinalBlock();
        return ms.GetBuffer();
    }

    public byte[] Decrypt(Span<byte> cipherBytes)
    {
        if (cipherBytes.IsEmpty)
        {
            throw new ArgumentException("Cipher bytes cannot be empty.", nameof(cipherBytes));
        }

        using var inputStream = new MemoryStream(cipherBytes.ToArray());
        using var cryptoStream = new CryptoStream(inputStream, _decryptor.Value, CryptoStreamMode.Read);
        using var resultStream = new MemoryStream();
        cryptoStream.CopyTo(resultStream);
        return resultStream.GetBuffer();
    }

    public void Dispose()
    {
        Dispose(true);
    }
    
    private void Dispose(bool disposing)
    {
        if (_disposed) return;
        
        if (disposing)
        {
            // Dispose managed state (managed objects).
            _aes.Dispose();
            _encryptor.Value.Dispose();
            _decryptor.Value.Dispose();
        }

        // Free unmanaged resources (unmanaged objects) and override a finalizer below.
        // Set large fields to null.

        _disposed = true;
    }
}