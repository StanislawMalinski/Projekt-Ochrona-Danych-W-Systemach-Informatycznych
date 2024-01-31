FROM nginx:latest
EXPOSE 5000
EXPOSE 3000


COPY ./front/dist/ ./usr/share/nginx/html
COPY ./nginx/nginx.conf ./etc/nginx/nginx.conf

RUN mkdir /var/log/nginx/front
RUN mkdir /var/log/nginx/back

