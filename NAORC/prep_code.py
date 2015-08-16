import sys
from naoqi import ALProxy
import vision_definitions
import binascii
import time

ip = "nao.local"
port = 9559
camera = 0

tts = ALProxy("ALTextToSpeech", "nao.local", port)
video = ALProxy("ALVideoDevice", ip, port)
motion = ALProxy("ALMotion", ip, port)

resolution = vision_definitions.kQVGA  # 320 * 240
colorSpace = vision_definitions.kRGBColorSpace
video.setParam(vision_definitions.kCameraSelectID, camera)

imgClient = None # Has to be set to the result of calling video.subscribe!

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
	time.sleep(0.005)
	while True:
		for t in motion.getTaskList():
			if t not in tasks1:
				return t[1]

def stopMove(moveID):
	motion.killTask(moveID)

def enableStiffness():
	motion.stiffnessInterpolation("Body", 1.0, 1.0)

def disableStiffness():
	motion.stiffnessInterpolation("Body", 0.0, 1.0)


