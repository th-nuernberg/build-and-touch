import socket
import struct

TCP_IP = '127.0.0.1'
TCP_PORT = 8181
BUFFER_SIZE = 1024

s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

def connect():
    s.connect((TCP_IP, TCP_PORT))

def send(msg):
    """ Send a serialized message (protobuf Message interface)
        to a socket, prepended by its length packed in 4
        bytes (big endian).
    """
    packed_len = struct.pack('<L', len(msg)) # Little endian / unsigned long (4 bytes)
    print("Sending message of length " + str(len(msg)))
    s.sendall(packed_len + msg)

def close():
    s.close()
