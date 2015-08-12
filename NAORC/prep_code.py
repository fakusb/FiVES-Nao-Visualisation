import sys
from naoqi import ALProxy
import vision_definitions
import binascii
import socket

ip = "nao.local"
port = 9559
camera = 0

tts = ALProxy("ALTextToSpeech", "nao.local", port)
videoProxy = ALProxy("ALVideoDevice", ip, port)

resolution = vision_definitions.kQVGA  # 320 * 240
colorSpace = vision_definitions.kRGBColorSpace
imgClient = videoProxy.subscribe("_client", resolution, colorSpace, 5)

videoProxy.setParam(vision_definitions.kCameraSelectID, camera)


sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
server_address = ('', 4711)
sock.bind(server_address)
sock.listen(1)

connection = None # Has to be set to the first component of the result of calling sock.accept!

def sendImage():
	image = videoProxy.getImageRemote(imgClient)
	pixels = image[6]
	connection.send(pixels)

def close():
	videoProxy.unsubscribe(imgClient)

