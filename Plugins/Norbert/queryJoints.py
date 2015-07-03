joints = ["HeadYaw",
		"HeadPitch",
		"RShoulderPitch",
		"RShoulderRoll",
		"RElbowRoll",
		"RElbowYaw",
		"RWristYaw",
		"RHand",
		"LShoulderPitch",
		"LShoulderRoll",
		"LElbowRoll",
		"LElbowYaw",
		"LWristYaw",
		"LHand",
		"RHipYawPitch",
		"RHipPitch",
		"RHipRoll",
		"RKneePitch",
		"RAnklePitch",
		"RAnkleRoll",
		"LHipYawPitch",
		"LHipPitch",
		"LHipRoll",
		"LKneePitch",
		"LAnklePitch",
		"LAnkleRoll"]

ALMEMORY_KEY_NAMES = []

for j in joints:
	ALMEMORY_KEY_NAMES.append("Device/SubDeviceList/{j}/Position/Sensor/Value".format(j=j))
	#ALMEMORY_KEY_NAMES.append("Device/SubDeviceList/{j}/Position/Actuator/Value".format(j=j))

import os
import sys
import time
import itertools
from naoqi import ALProxy

memory = ALProxy("ALMemory", "127.0.0.1", 9559)

def query():
	for (label, key) in itertools.izip(joints, ALMEMORY_KEY_NAMES):
		print label, memory.getData(key)

