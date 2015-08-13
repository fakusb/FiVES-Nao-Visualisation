import sys
from naoqi import ALProxy
import vision_definitions
import binascii
import socket

ip = "nao.local"
port = 9559
camera = 0

tts = ALProxy("ALTextToSpeech", "nao.local", port)
video = ALProxy("ALVideoDevice", ip, port)
motion = ALProxy("ALMotion", ip, port)

resolution = vision_definitions.kQVGA  # 320 * 240
colorSpace = vision_definitions.kRGBColorSpace
video.setParam(vision_definitions.kCameraSelectID, camera)

sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
server_address = ('', 4711)
sock.bind(server_address)
sock.listen(1)

connection = None # Has to be set to the first component of the result of calling sock.accept!

def enableCamera():
	imgClient = video.subscribe("_client", resolution, colorSpace, 5)

def sendImage():
	image = video.getImageRemote(imgClient)
	pixels = image[6]
	connection.send(pixels)

def disableCamera():
	video.unsubscribe(imgClient)

def close():
	disableCamera()

def startMove(joint, target, speed):
	tasks1 = motion.getTaskList()
	motion.post.angleInterpolationWithSpeed(joint, target, speed)
	for t in motion.getTaskList():
		if t not in tasks1:
			return t[1]
		else:
			tasks1.remove(t)

def stopMove(moveID):
	motion.killTask(moveID)

def enableStiffness():
	motion.stiffnessInterpolation("Body", 1.0, 1.0)

def disableStiffness():
	motion.stiffnessInterpolation("Body", 0.0, 1.0)
