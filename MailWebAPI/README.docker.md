# Run docker

docker run -e ASPNETCORE_URLS=http://+:5000 -p 8081:5000 -v ./data:/app/data  mail-web-api 