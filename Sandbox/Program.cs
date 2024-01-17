using BenchmarkDotNet.Running;
using Sandbox.Features.Obfuscation;

Console.WriteLine("Hello, World!");

BenchmarkRunner.Run<AesFastBenchmarks>();