# Fase de construcción
FROM node:22-alpine AS builder

# Establece el directorio de trabajo
WORKDIR /

# Copia los archivos package.json y package-lock.json
COPY package*.json ./

# Instala las dependencias
RUN npm install

# Copia el resto de la aplicación
COPY . .

# Construye la aplicación
RUN npm run build --prod

# Fase de producción
FROM nginx:alpine

# Copia los archivos de construcción de la fase anterior
COPY --from=builder /dist/angular-blog-client /usr/share/nginx/html

# Expone el puerto en el que Nginx estará escuchando
EXPOSE 80

# Comando por defecto para ejecutar Nginx
CMD ["nginx", "-g", "daemon off;"]
