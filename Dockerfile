FROM nginx:latest
EXPOSE 5000
EXPOSE 3000

COPY ./front/dist/ ./usr/share/nginx/html
COPY ./nginx/nginx.conf ./etc/nginx/nginx.conf

RUN mkdir /var/log/nginx/front
RUN mkdir /var/log/nginx/back
RUN mkdir /var/log/nginx/back/resouce
RUN mkdir /var/log/nginx/back/resouce/db

COPY back/bin/Release/net8.0/ /var/log/nginx/back
COPY back/resource/db/ /var/log/nginx/back/resouce/db


