FROM nginx:latest
EXPOSE 5000
EXPOSE 3000

COPY ./front/dist/ ./usr/share/nginx/html
COPY ./nginx/nginx.conf ./etc/nginx/nginx.conf

RUN mkdir /var/log/nginx/front
RUN mkdir /var/log/nginx/back
RUN mkdir /var/log/nginx/back/resource
RUN mkdir /var/log/nginx/back/resource/db

COPY back/bin/Release/net8.0/ /var/log/nginx/back
COPY back/resource/db/ /var/log/nginx/back/resource/db

RUN apt-get update
RUN apt-get install -y vim
RUN apt-get install -y wget
RUN wget https://packages.microsoft.com/config/debian/12/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
RUN dpkg -i packages-microsoft-prod.deb
RUN rm packages-microsoft-prod.deb
RUN apt-get update
RUN apt-get install -y dotnet-sdk-8.0

#CMD ["dotnet", "/var/log/nginx/back/projekt.dll"]
#dotnet /var/log/nginx/back/projekt.dll


