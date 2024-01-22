FROM node:18-alpine

EXPOSE 5000

WORKDIR /app
COPY package.json /app
COPY src /app/src
COPY public /app/public
COPY index.html /app
COPY public /app/public
COPY clientconfig.json /app
COPY vite.config.ts /app
COPY tsconfig.json /app
COPY tsconfig.node.json /app

RUN npm install
RUN npm run build

CMD ["npm", "run", "serve"]

