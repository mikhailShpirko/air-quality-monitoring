import struct
import serial

class sds011(object):
    """abstraction over SDS011 sensor to work in query mode"""
    """the source is partially taken from https://github.com/ikalchev/py-sds011 but refactored"""
    __HEAD = b'\xaa'
    __TAIL = b'\xab'
    __CMD_ID = b'\xb4'

    __WRITE = b"\x01"

    __REPORT_MODE_CMD = b"\x02"
    __QUERY_REPORT_MODE = b"\x01"

    __SLEEP_CMD = b"\x06"

    __SLEEP_MODE = b"\x00"
    __WORK_MODE = b"\x01"

    __QUERY_CMD = b"\x04"

    def __init__(self, serial_port):
        """Initialise and open serial port."""
        self.serial = serial.Serial(port=serial_port,
                                 baudrate=9600,
                                 timeout=2)
        self.serial.flush()
        self.__set_passive_report_mode()

    def __cmd_begin(self):
        """Get command header and command ID bytes.
        @rtype: list
        """
        return self.__HEAD + self.__CMD_ID

    def __finish_cmd(self, cmd, id1=b"\xff", id2=b"\xff"):
        """Add device ID, checksum and tail bytes.
        @rtype: list
        """
        cmd += id1 + id2
        checksum = sum(d for d in cmd[2:]) % 256
        cmd += bytes([checksum]) + self.__TAIL
        return cmd

    def __execute(self, cmd_bytes):
        """Writes a byte sequence to the serial."""
        self.serial.write(cmd_bytes)

    def __get_reply(self):
        """Read reply from serial."""
        raw = self.serial.read(size=10)
        data = raw[2:8]
        if len(data) == 0:
            return None
        if (sum(d for d in data) & 255) != raw[8]:
            return None
        return raw

    def __set_passive_report_mode(self):
        """Sets passive report mode"""
        cmd = self.__cmd_begin()
        cmd += (self.__REPORT_MODE_CMD
                + self.__WRITE
                + self.__QUERY_REPORT_MODE
                + b"\x00" * 10)
        cmd = self.__finish_cmd(cmd)
        self.__execute(cmd)
        self.__get_reply()

    def sleep(self):
        """Put sensor to sleep mode"""
        cmd = self.__cmd_begin()
        cmd += (self.__SLEEP_CMD
                + self.__WRITE
                + self.__SLEEP_MODE
                + b"\x00" * 10)
        cmd = self.__finish_cmd(cmd)
        self.__execute(cmd)
        self.__get_reply()

    def work(self):
        """Put sensor to work mode"""
        cmd = self.__cmd_begin()
        cmd += (self.__SLEEP_CMD
                + self.__WRITE
                + self.__WORK_MODE
                + b"\x00" * 10)
        cmd = self.__finish_cmd(cmd)
        self.__execute(cmd)
        self.__get_reply()

    def query(self):
        """Query the device and read the data.
        @return: Air particulate density in micrograms per cubic meter.
        @rtype: tuple(float, float) -> (PM2.5, PM10)
        """
        cmd = self.__cmd_begin()
        cmd += (self.__QUERY_CMD
                + b"\x00" * 12)
        cmd = self.__finish_cmd(cmd)
        self.__execute(cmd)

        raw = self.__get_reply()
        if raw is None:
            return None
        data = struct.unpack('<HH', raw[2:6])
        pm2_5 = data[0] / 10.0
        pm10 = data[1] / 10.0
        return (pm2_5, pm10)