FROM nginx:alpine
EXPOSE 5000
EXPOSE 3000


COPY ./front ./front
COPY ./back ./back

RUN apt-get update && apt-get upgrade -y && \
    apt-get install -y nodejs \
    npm               

RUN npm i ./front/
RUN npm run serve --prefix ./front/
RUN dotnet run --project ./back/projekt.csproj