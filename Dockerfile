FROM node:18-alpine

EXPOSE 5000

WORKDIR /app
COPY front/package.json /app
COPY front/src /app/src
COPY front/public /app/public
COPY front/index.html /app
COPY front/public /app/public
COPY front/clientconfig.json /app
COPY front/vite.config.ts /app
COPY front/tsconfig.json /app
COPY front/tsconfig.node.json /app

RUN npm install
RUN npm run build

CMD ["npm", "run", "serve"]

