namespace Sandbox.Features.Obfuscation;

public sealed class Base64Encoder
{
    public string Encode(byte[] input)
    {
        if (input == null || input.Length == 0)
        {
            throw new ArgumentException("Input cannot be null or empty.", nameof(input));
        }

        return Convert.ToBase64String(input);
    }

    public byte[] Decode(string base64String)
    {
        if (string.IsNullOrEmpty(base64String))
        {
            throw new ArgumentException("Base64 string cannot be null or empty.", nameof(base64String));
        }

        return Convert.FromBase64String(base64String);
    }
}