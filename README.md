# nin-kode-api
Gå til url for å bli redirigert til swagger-dokumentasjonen.

## [NiN3 Wiki](https://github.com/Artsdatabanken/nin-kode-api/wiki)

## Drift
https://nin-kode-api.artsdatabanken.no/

## Test
https://nin-kode-api.test.artsdatabanken.no/

## Development

Populate local database: [Console app readme](./NiN.Console/README.md)

### Docker

cd NinKode.WebApi

docker build -f ./Dockerfile --force-rm -t artsdatabanken/nin-kode-api ..

docker run -d -p 5001:8000 artsdatabanken/nin-kode-api

docker push artsdatabanken/nin-kode-api
