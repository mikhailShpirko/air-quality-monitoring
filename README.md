# Air Quality Monitoring
Software for air quality monitoring using Nova PM SDS011 sensor.

Main goal of this project was to be able to track air quality and review historical measurements. Additional technical goals I had in mind:
- Apply DDD and CQRS using MediatR
- Create clean architecture
- Have a tidy solution structure
- Cover the most critical logic with UT and use Fluent Assertions
- Try out Python to work with SDS011 Sensor

## How it works
- Communication with the sensor is done via Web API powered by Python and Flusk. There is a single endpoint that queries measurement at current time from the sensor.
- The measurements are stored in PostgreSQL database and can be managed via Web API
- The background service queries the Sensor Web API every X (confugred) amount of seconds and sends response to Measurement Web API
- There is a Web UI that shows measurements for selected period in line chart. Measurements are queried from Measurements Web API
- I am using Orange Pi 4 LTS with Ubuntu OS as host server. The sensor is connected to USB port and project is lanuched on that device.

## Launch instructions
Make sure that you have hardware (Raspberry Pi, Orange Pi or any other compatable device and Nova PM SDS011 sensor) and software (Docker, Docker Compose, etc.). Clone the project on that hardware. Execute the following command to run the project to play around with it:

```
docker compose -f deploy/AirQualityMonitoring.DockerCompose.yaml -p air_quality_monitoring up -d --build
```

After that you will be able to test the Measurement API documentation via http://localhost:9898/swagger/

Web UI will be available at: http://localhost:9899/

Execute the following command to shut down the project:

```
docker compose -f deploy/AirQualityMonitoring.DockerCompose.yaml -p air_quality_monitoring down
```

## Project structure
    .
    ├── deploy                                              # deployment scripts and configuration files
    ├── docker                                              # docker files for project components
    ├── src                                                 # source code for application components and services
    └── tests                                               # unit and integration tests       
    

## Limitations
- Basic domain entities implementation. Didn't use aggregate root, value objects and domain events. The use-cases didn't require those, so they are not implemented in order not to overcomplicate solution with unused logic.
- Very basic and primitive front-end. Simple HTML page with several CSS and JS libraries to display the graph and make API calls, hosted on NGINX image.
- Not the most efficient use of return types. Using OneOf would be more optimal and will be more declarative, also would prevent throwing exceptions and returning the dedicated type instead.
- Not resilient solution. If Measurement API is down - the measurement queried from the sensor will not reach the endpoint and will be lost. For future improvement, using queue for processing to ensure data consistency and integrity.