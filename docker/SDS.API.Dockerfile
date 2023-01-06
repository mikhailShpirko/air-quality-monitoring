FROM python:3.6
WORKDIR /src
COPY src/Sensor/SDS011.API/requirements.txt .
RUN pip install -r requirements.txt
COPY src/Sensor/SDS011.API .
ENTRYPOINT [ "python" ]
CMD [ "app.py" ]