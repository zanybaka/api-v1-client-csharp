#Blockchain API library (C#, v1)

An official .NET (C#) library for interacting with the Blockchain.info API.

###Getting started

The library requires at least .NET 4.0.

The library consists of the following namespaces:

* `Info.Blockchain.Api.Blockexplorer` ([docs](docs/blockexplorer.md)) ([api/blockchain_api][api1])
* `Info.Blockchain.Api.Createwallet` ([docs](docs/createwallet.md)) ([api/create_wallet][api2])
* `Info.Blockchain.Api.Exchangerates` ([docs](docs/exchangerates.md)) ([api/exchange\_rates\_api][api3])
* `Info.Blockchain.Api.Receive` ([docs](docs/receive.md)) ([api/api_receive][api4])
* `Info.Blockchain.Api.Statistics` ([docs](docs/statistics.md)) ([api/charts_api][api5])
* `Info.Blockchain.Api.Wallet` ([docs](docs/wallet.md)) ([api/blockchain\_wallet\_api][api6])

###Error handling

All methods may throw exceptions caused by incorrectly passed parameters or other problems. If a call is rejected server-side, the `APIException` exception will be thrown. Other exceptions may also be thrown by the environment (e.g. no internet connection etc).

###Request limits and API keys

In order to prevent abuse some API methods require an API key approved with some basic contact information and a description of its intended use. Please request an API key [here](https://blockchain.info/api/api_create_code).

The same API key can be used to bypass the request limiter.

[api1]: https://blockchain.info/api/blockchain_api
[api2]: https://blockchain.info/api/create_wallet
[api3]: https://blockchain.info/api/exchange_rates_api
[api4]: https://blockchain.info/api/api_receive
[api5]: https://blockchain.info/api/charts_api
[api6]: https://blockchain.info/api/blockchain_wallet_api