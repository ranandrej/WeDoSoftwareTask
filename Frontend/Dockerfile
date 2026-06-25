FROM node:22-alpine AS build
WORKDIR /app

COPY package.json package-lock.json ./
RUN npm ci

COPY . .
RUN npm run build -- --configuration=docker

FROM node:22-alpine AS final
WORKDIR /app
RUN npm install -g serve

COPY --from=build /app/dist/Frontend/browser ./dist

EXPOSE 4200
CMD ["serve", "-s", "dist", "-l", "4200"]
