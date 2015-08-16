import socket

sock_port = 4711 # Modified by DataConnection.cs

sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
server_address = ('', sock_port)
sock.bind(server_address)
sock.listen(1)

connection = None # Has to be set to the first component of the result of calling sock.accept!
