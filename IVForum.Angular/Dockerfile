FROM nginx:1.13.3-alpine
COPY ./nginx-default.conf /etc/nginx/conf.d/default.conf
RUN rm -rf /usr/share/nginx/html/*
COPY . /usr/share/nginx/html