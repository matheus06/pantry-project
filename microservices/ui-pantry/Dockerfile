###BUILD ANGULAR SOLUTION###
FROM node:16-alpine3.14 as build
COPY . /app
WORKDIR /app
RUN npm i
COPY . ./

ARG ENVIRONMENT
RUN npm run build --configuration ${ENVIRONMENT} --output-path=dist

###BUILD NGINX AND COPYING FILES###
FROM nginx:1.21.6-alpine
COPY nginx.conf /etc/nginx/nginx.conf
COPY --from=build /app/dist/ui-pantry /usr/share/nginx/html
CMD ["nginx", "-g", "daemon off;"]