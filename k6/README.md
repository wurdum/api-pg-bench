# k6 perf tests

```
docker build -t api-k6 .
docker run --rm --name api-k6 api-k6 run --vus 10 --duration 20s  /app/grpc-get-latest-price-test.js
```
