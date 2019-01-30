# PerfTests
This repository contains some performance tests on c#


### Test01: Int array to byte stream
```
BenchmarkDotNet=v0.11.3, OS=Windows 10.0.17134.523 (1803/April2018Update/Redstone4)
Intel Core i7-4770R CPU 3.20GHz (Haswell), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=2.2.101
  [Host] : .NET Core 2.2.0 (CoreCLR 4.6.27110.04, CoreFX 4.6.27110.04), 64bit RyuJIT
  Core   : .NET Core 2.2.0 (CoreCLR 4.6.27110.04, CoreFX 4.6.27110.04), 64bit RyuJIT

Job=Core  Runtime=Core
```
|                          Method |      Mean |     Error |    StdDev |       Min |       Max | Ratio | Rank | Gen 0/1k Op | Gen 1/1k Op | Gen 2/1k Op | Allocated Memory/Op |
|-------------------------------- |----------:|----------:|----------:|----------:|----------:|------:|-----:|------------:|------------:|------------:|--------------------:|
|            SimpleImplementation | 2.2194 ns | 0.0294 ns | 0.0246 ns | 2.1952 ns | 2.2820 ns | 1.000 |    5 |      0.0005 |           - |           - |                 3 B |
|  SimpleImplementationWithBuffer | 1.8745 ns | 0.0196 ns | 0.0183 ns | 1.8472 ns | 1.9082 ns | 0.844 |    4 |           - |           - |           - |                   - |
|   StackllocBufferImplementation | 1.7642 ns | 0.0173 ns | 0.0162 ns | 1.7343 ns | 1.7930 ns | 0.795 |    3 |           - |           - |           - |                   - |
| **MemoryMarshallingImplementation** | 0.0099 ns | 0.0000 ns | 0.0000 ns | 0.0099 ns | 0.0100 ns | **0.004** |    2 |           - |           - |           - |                   - |
|           **PointerImplementation** | 0.0087 ns | 0.0001 ns | 0.0001 ns | 0.0085 ns | 0.0088 ns | **0.004** |    1 |           - |           - |           - |                   - |


### Test02: Struct factory over Delegate factory
```
BenchmarkDotNet=v0.11.3, OS=Windows 10.0.17134.523 (1803/April2018Update/Redstone4)
Intel Core i7-4770R CPU 3.20GHz (Haswell), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=2.2.101
  [Host] : .NET Core 2.2.0 (CoreCLR 4.6.27110.04, CoreFX 4.6.27110.04), 64bit RyuJIT
  Core   : .NET Core 2.2.0 (CoreCLR 4.6.27110.04, CoreFX 4.6.27110.04), 64bit RyuJIT

Job=Core  Runtime=Core
```
|                       Method |     Mean |     Error |    StdDev |      Min |      Max | Ratio | RatioSD | Rank |
|----------------------------- |---------:|----------:|----------:|---------:|---------:|------:|--------:|-----:|
|       NormalClassWithFactory | 5.590 ns | 0.1841 ns | 0.1722 ns | 5.283 ns | 5.900 ns |  1.00 |    0.00 |    2 |
| **NormalClassWithStructFactory** | 3.261 ns | 0.1339 ns | 0.1252 ns | 3.044 ns | 3.464 ns |  **0.58** |    0.03 |    1 |

### Test03: Method calls
```
BenchmarkDotNet=v0.11.3, OS=Windows 10.0.17134.523 (1803/April2018Update/Redstone4)
Intel Core i7-4770R CPU 3.20GHz (Haswell), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=2.2.101
  [Host] : .NET Core 2.2.0 (CoreCLR 4.6.27110.04, CoreFX 4.6.27110.04), 64bit RyuJIT
  Core   : .NET Core 2.2.0 (CoreCLR 4.6.27110.04, CoreFX 4.6.27110.04), 64bit RyuJIT

Job=Core  Runtime=Core
```
|               Method |      Mean |     Error |    StdDev |       Min |       Max | Ratio | RatioSD | Rank |
|--------------------- |----------:|----------:|----------:|----------:|----------:|------:|--------:|-----:|
|      **NormalClassCall** | 0.2891 ns | 0.0027 ns | 0.0022 ns | 0.2847 ns | 0.2926 ns |  **1.00** |    0.00 |    1 |
|      **SealedClassCall** | 0.2892 ns | 0.0024 ns | 0.0020 ns | 0.2853 ns | 0.2915 ns |  **1.00** |    0.01 |    1 |
| ClassWithVirtualCall | 2.0147 ns | 0.0214 ns | 0.0190 ns | 1.9771 ns | 2.0527 ns |  6.97 |    0.08 |    2 |
|        InterfaceCall | 2.6957 ns | 0.0266 ns | 0.0249 ns | 2.6645 ns | 2.7381 ns |  9.33 |    0.13 |    4 |
|         AbstractCall | 2.3063 ns | 0.0295 ns | 0.0276 ns | 2.2660 ns | 2.3503 ns |  7.97 |    0.11 |    3 |

### Test04: Matrix Row Access over Column Access (Data locality)
```
BenchmarkDotNet=v0.11.3, OS=Windows 10.0.17134.523 (1803/April2018Update/Redstone4)
Intel Core i7-4770R CPU 3.20GHz (Haswell), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=2.2.101
  [Host]     : .NET Core 2.2.0 (CoreCLR 4.6.27110.04, CoreFX 4.6.27110.04), 64bit RyuJIT
  DefaultJob : .NET Core 2.2.0 (CoreCLR 4.6.27110.04, CoreFX 4.6.27110.04), 64bit RyuJIT
```

|       Method |       Mean |    Error |   StdDev | Ratio | RatioSD |
|------------- |-----------:|---------:|---------:|------:|--------:|
|    **RowAccess** |   118.8 ms | 11.99 ms | 16.41 ms |  **1.00** |    0.00 |
| ColumnAccess | 2,197.1 ms | 14.75 ms | 13.80 ms | 18.54 |    2.07 |
