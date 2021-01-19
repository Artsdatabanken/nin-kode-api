# nin-kode-api

cd NinKode.WebApi

docker build -f ./Dockerfile --force-rm -t artsdatabanken/nin-kode-api ..

docker run -d -p 5001:8000 artsdatabanken/nin-kode-api

docker push artsdatabanken/nin-kode-api
