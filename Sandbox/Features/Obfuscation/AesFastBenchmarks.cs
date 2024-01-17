using BenchmarkDotNet.Attributes;

namespace Sandbox.Features.Obfuscation;

[MemoryDiagnoser]
public class AesFastBenchmarks
{
    private AesFast _aesFast;
    private Base64Encoder _base64Encoder;
    private byte[] _data;

    [GlobalSetup]
    public void Setup()
    {
        var key = "some test key";
        _aesFast = new AesFast(key);
        _data = new byte[1024]; // adjust size as needed
        Random.Shared.NextBytes(_data);

        _base64Encoder = new Base64Encoder();
    }

    [Benchmark]
    public string Aes()
    {
        var hash = _aesFast.Encrypt(_data);
        // Convert the hash to base64 so it's apples-to-apples with the base64 encoder.
        return Convert.ToBase64String(hash);
    }

    [Benchmark]
    public string Base64() => _base64Encoder.Encode(_data);
}
