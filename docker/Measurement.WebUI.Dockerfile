FROM nginx:1.23.3-alpine
COPY ["src/Measurement/Measurement.WebUI/", "/usr/share/nginx/html"]

# When the container starts, replace the env.js with values from environment variables
CMD ["/bin/sh",  "-c",  "envsubst < /usr/share/nginx/html/js/env.template.js > /usr/share/nginx/html/js/env.js && exec nginx -g 'daemon off;'"]