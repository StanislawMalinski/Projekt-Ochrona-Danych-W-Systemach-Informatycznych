FROM nginx:latest
EXPOSE 5000
EXPOSE 3000

COPY ./front/dist/ ./usr/share/nginx/html
COPY ./nginx/nginx.conf ./etc/nginx/nginx.conf
COPY ./nginx/ssl.conf ./etc/nginx/conf.d/ssl.conf

RUN mkdir /var/log/nginx/front
RUN mkdir /var/log/nginx/back
RUN mkdir /var/log/nginx/back/resource
RUN mkdir /var/log/nginx/back/resource/db

COPY back/bin/Release/net8.0/projekt.dll /var/log/nginx/back/
COPY back/resource/db/ /var/log/nginx/back/resource/db

RUN apt-get update
RUN apt-get install -y vim
RUN apt-get install -y wget
RUN wget https://packages.microsoft.com/config/debian/12/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
RUN dpkg -i packages-microsoft-prod.deb
RUN rm packages-microsoft-prod.deb
RUN apt-get update
RUN apt-get install -y dotnet-sdk-8.0
RUN openssl req -x509 -nodes -days 365 -newkey rsa:2048 -keyout /etc/nginx/ssl/selfsigned.key -out /etc/nginx/ssl/selfsigned.crt -subj "/C=PL/ST=Masovia/L=Warszał/O=PW/OU=EE/CN=bank.com"

CMD ["dotnet", "/var/log/nginx/back/projekt.dll"]
