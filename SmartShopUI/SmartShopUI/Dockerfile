FROM node:18 AS build-env
WORKDIR /app

COPY package*.json ./
RUN npm install

COPY . ./
RUN npm run build --prod

FROM nginx:alpine
COPY --from=build-env /app/dist/smart-shop-ui /usr/share/nginx/html