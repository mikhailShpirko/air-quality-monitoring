from flask import Flask, jsonify
from sds011 import sds011
import time
import sys

app = Flask(__name__)
sds = None
try:
    sds = sds011("/dev/ttyUSB0")
    sds.sleep()
    print("SDS011 connection initialized", file=sys.stdout)
except Exception as err:
    print(f"Error initializing SDS011 connection: {err}, {type(err)}", file=sys.stderr)
    sds = None


@app.route('/api/query_measurement')
def query_measurement():
    try:
        global sds
        if sds is None:
            sds = sds011("/dev/ttyUSB0")
        sds.work()
        """The most optimal for sensor usage flow is"""
        """not to keep it running all the time"""
        """warm sensor up, get measurement and put back to sleep"""
        """it is not performance efficient (will work much slower)"""
        """but sensor will serve much longer"""
        time.sleep(30)
        measurement = sds.query()      
        return jsonify({
            "pm2_5": measurement[0],
            "pm10": measurement[1],
            }), 200
    except Exception as err:
        return f"Error occured: {err}, {type(err)}", 500  
    finally:
        if sds is not None:
            sds.sleep()

if __name__ == '__main__':
    import os
    HOST = os.environ.get('SERVER_HOST', 'localhost')
    try:
        PORT = int(os.environ.get('SERVER_PORT', '5555'))
    except ValueError:
        PORT = 5555
    app.run(HOST, PORT)
