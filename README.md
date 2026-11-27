# nin-kode-api
Gå til url for å bli redirigert til swagger-dokumentasjonen.

## [NiN3 Wiki](https://github.com/Artsdatabanken/nin-kode-api/wiki)

## Drift
https://nin-kode-api.artsdatabanken.no/

## Test
https://nin-kode-api.test.artsdatabanken.no/

## Development

Populate local database: [Console app readme](./NiN.Console/README.md)

### API version compatibility

The deprecated `2.1b` version remains available in URL paths and the `api-version` query parameter. After the API versioning library migration, malformed version queries return HTTP 400 instead of using the default version. Unsupported URL versions return HTTP 404 rather than 400, and versioning error response details can differ from the previous library.

The 3.0 controllers also accept the `v3.0-v2026` route and expose it in Swagger. Both 3.0 route versions currently use the same `3.0` data; the new route does not select a separate 2026 dataset.

### Docker

cd NinKode.WebApi

docker build -f ./Dockerfile --force-rm -t artsdatabanken/nin-kode-api ..

docker run -d -p 5001:8000 artsdatabanken/nin-kode-api

docker push artsdatabanken/nin-kode-api
