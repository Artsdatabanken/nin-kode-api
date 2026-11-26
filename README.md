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

### Docker

cd NinKode.WebApi

docker build -f ./Dockerfile --force-rm -t artsdatabanken/nin-kode-api ..

docker run -d -p 5001:8000 artsdatabanken/nin-kode-api

docker push artsdatabanken/nin-kode-api
