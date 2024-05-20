# SessionRedisDemo
An MVC .NET 8 demo application with cache distributed by Redis or memory, according to the desired environment. As a demonstration, Data Protection Keys are stored in Redis or local file (volume).

## Run the application

- Linux users may be required to have administrative privileges

### Redis Distributed Cache
```
docker-compose up --build web_redis
```
Endereço:
[http://localhost:8080](http://localhost:8080/)

---

### Memory Distributed Cache
```
docker-compose up --build web_memory
```
Endereço:
[http://localhost:8080](http://localhost:8080/)

---

### Local
```
dotnet run
```
Endereço:
[http://localhost:5124](http://localhost:5124/)