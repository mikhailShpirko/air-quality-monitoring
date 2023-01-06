FROM postgres:15.1
COPY deploy/db/init_measurement_db.sql /docker-entrypoint-initdb.d/