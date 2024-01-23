FROM nginx:alpine
EXPOSE 5000
EXPOSE 3000

COPY ./front ./front
COPY ./back ./back

RUN sudo apt-get update
RUN  sudo apt-get install docker-compose-plugin
COPY docker-compose.yml .
RUN docker-compose up