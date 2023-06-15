
FROM node:18.14.2 AS base

WORKDIR /app

COPY . .



RUN npm ci && npm run build


FROM nginx:alpine

COPY --from=base /app/dist/* /usr/share/nginx/html

EXPOSE 80
EXPOSE 443



