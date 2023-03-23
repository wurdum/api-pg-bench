FROM node:16.17 AS build

WORKDIR /app

COPY "k6/package.json" "k6/package-lock.json" "./"
RUN npm install

COPY k6/ ./
RUN npm start

FROM grafana/k6:0.43.1 AS k6
WORKDIR /app
COPY --from=build /app/dist .

ENTRYPOINT ["k6"]
