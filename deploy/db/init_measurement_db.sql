CREATE TABLE IF NOT EXISTS measurements (
  id uuid NOT NULL,
  pm2_5 numeric NOT NULL,
  pm10 numeric NOT NULL,
  timestampticks bigint NOT NULL,
  PRIMARY KEY (id)
)