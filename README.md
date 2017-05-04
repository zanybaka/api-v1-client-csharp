# Blockchain API library (C#, .NET Core, v1)

An official C# (.NET Core) library for interacting with the Blockchain.info API.

## Latest changes

This library is a migration of the original .NET library to .NET Core. Some models and namespaces have been modified. Most notably:

* Receive Payments v2 API functionality has been introduced
* Additional chart functionality has been added to the `StatisticsExplorer` class
* All documentation has been brought up to date
* All models have been moved to `Info.Blockchain.API.Models`
* The client, exception and helper classes under the root namespace have been moved to `Info.Blockchain.API.Client`
* `CreateWallet.cs`, which previously contained the CreateWallet response and request models, was split into two separate models and moved to the `Data` namespace
* The `Info.Blockchain.API.CreateWallet` namespace was removed, the `WalletCreator.cs` class moved to `Info.Blockchain.API.Wallet`

## Getting started

The library requires .NET Core 1.1. It is possible to use this library with the .NET Framework by editing
`Info.Blockchain.Api.csproj`

and changing

`<TargetFramework>netcoreapp1.1</TargetFramework>`

to your project's .NET version, e.g.

`<TargetFramework>net46</TargetFramework>`

The recommended way to install and use the library is via NuGet:
```
PM> Install-Package BlockchainAPI
```

The library consists of the following namespaces:

* `Info.Blockchain.API.Client` ([docs](docs/blockchainhttpclient.md))
* `Info.Blockchain.API.BlockExplorer` ([docs](docs/blockexplorer.md)) ([api/blockchain_api][api1])
* `Info.Blockchain.API.ExchangeRates` ([docs](docs/exchangerates.md)) ([api/exchange\_rates\_api][api3])
* `Info.Blockchain.API.PushTx` ([docs](docs/pushtx.md)) ([pushtx][api6])
* `Info.Blockchain.API.Statistics` ([docs](docs/statistics.md)) ([api/charts_api][api4])
* `Info.Blockchain.API.Wallet` ([docs](docs/wallet.md)) ([api/blockchain\_wallet\_api][api5]) and ([api/blockchain/create_wallet][api2])
* `Info.Blockchain.API.Receive` ([docs](docs/receive.md)) ([api/receive\_payments\_v2\_api][api7])

The following namespacse contain helpers and models used by the above:

* `Info.Blockchain.API.Client`
* `Info.Blockchain.API.Data`
* `Info.Blockchain.API.Json`

In order to use the Wallet and CreateWallet functionality, you must provide a URL to an instance of [service-my-wallet-v3](https://github.com/blockchain/service-my-wallet-v3) as first parameter to BlockchainApiHelper.
If you don't intend to use these functionalities, this parameter can be null.

## Error handling

All methods may throw exceptions caused by incorrectly passed parameters or other problems. If a call is rejected server-side, the `APIException` exception will be thrown. Other exceptions may also be thrown by the environment (e.g. no internet connection etc).

## Connection timeouts

It is possible to set arbitrary connection timeouts.

```csharp
// time out after 5 seconds
Info.Blockchain.API.HttpClient.TimeoutMs = 5000;
```

## Request limits and API keys

In order to prevent abuse some API methods require an API key approved with some basic contact information and a description of its intended use. Please request an API key [here](https://blockchain.info/api/api_create_code). The API key can be passed to the constructor of the `BlockchainHttpClient` class, which can be passed to the constructor of all other classes in this library.

The same API key can be used to bypass the request limiter.

[api1]: https://blockchain.info/api/blockchain_api
[api2]: https://blockchain.info/api/create_wallet
[api3]: https://blockchain.info/api/exchange_rates_api
[api4]: https://blockchain.info/api/charts_api
[api5]: https://blockchain.info/api/blockchain_wallet_api
[api6]: https://blockchain.info/pushtx
[api7]: https://blockchain.info/api/api_receive