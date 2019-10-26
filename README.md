# FAMS3



## Project Structure

    .
    ├── app                     # Application Source Files.
    |   ├──                     # Search API
    |   └──                     # Dynamic Adapter
    ├── .gitignore              # Git ignore.
    └── README.md               # This file.

## SearchApi

FAMS-search-api is a dotnet core rest service to execute people search accross multiple data providers.

### Tracing

The Search Api uses [opentracing](https://opentracing.io/) instrumentation for distributed tracing.

It uses [Jaeger](https://www.jaegertracing.io/) implementation to monitor and troubleshoot transactions and reference the [jeager-client-csharp](https://github.com/jaegertracing/jaeger-client-csharp).

The csharp client is set up to use [configuration via environment](https://github.com/jaegertracing/jaeger-client-csharp#configuration-via-environment).

*Notes*

> Set `JAEGER_SERVICE_NAME` if you want the tracer to ship tracing logs.  
> Set `JAEGER_SAMPLER_TYPE=const` if you want to sample all your traces.

## Run on Docker

download and install [Docker](https://www.docker.com)

Run

```shell
cd app/SearchApi
docker-compose up
```

Application health can be checked [here](http://localhost:5050/health).

## Run

download and install [dotnet core 3.0](https://dotnet.microsoft.com/download/dotnet-core/3.0)

Optionaly you can configure `jaeger` client to send traces:

![asp-config](docs/aspnet.configuration.env.png)

Run

```shell
cd app/SearchApi/SearchApi.Web
dotnet run
```

Application health can be checked [here](http://localhost:5000/health).

FAMS-search-api is a dotnet core rest service to execute people search accross multiple data providers.

## DynamicAdapter

FAMS-dynamic-adapter is a dotnet core rest service that polls dynamics to retrieve ready to search request and execute search using the Search API.

### Tracing

The Dynamic Adapter uses [opentracing](https://opentracing.io/) instrumentation for distributed tracing.

It uses [Jaeger](https://www.jaegertracing.io/) implementation to monitor and troubleshoot transactions and reference the [jeager-client-csharp](https://github.com/jaegertracing/jaeger-client-csharp).

The csharp client is set up to use [configuration via environment](https://github.com/jaegertracing/jaeger-client-csharp#configuration-via-environment).

*Notes*

> Set `JAEGER_SERVICE_NAME` if you want the tracer to ship tracing logs.  
> Set `JAEGER_SAMPLER_TYPE=const` if you want to sample all your traces.

## Run on Docker

download and install [Docker](https://www.docker.com)

Run

```shell
cd app/DynamicAdapter
docker-compose up
```

Application health can be checked [here](http://localhost:5060/health).

## Run

download and install [dotnet core 3.0](https://dotnet.microsoft.com/download/dotnet-core/3.0)

Optionaly you can configure `jaeger` client to send traces:

![asp-config](docs/aspnet.configuration.env.png)

Run

```shell
cd app/DynamicAdapter/DynamicAdapter.Web
dotnet run
```
