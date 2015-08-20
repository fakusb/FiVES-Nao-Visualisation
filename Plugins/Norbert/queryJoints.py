joints = [
	"HeadYaw",		# 0
	"HeadPitch",		# 1
	"RShoulderPitch",	# 2
	"RShoulderRoll",	# 3
	"RElbowRoll",		# 4
	"RElbowYaw",		# 5
	"RWristYaw",		# 6
	"RHand",		# 7
	"LShoulderPitch",	# 8
	"LShoulderRoll",	# 9
	"LElbowRoll",		# 10
	"LElbowYaw",		# 11
	"LWristYaw",		# 12
	"LHand",		# 13
	"RHipYawPitch",		# 14
	"RHipPitch",		# 15
	"RHipRoll",		# 16
	"RKneePitch",		# 17
	"RAnklePitch",		# 18
	"RAnkleRoll",		# 19
	"LHipYawPitch",		# 20
	"LHipPitch",		# 21
	"LHipRoll",		# 22
	"LKneePitch",		# 23
	"LAnklePitch",		# 24
	"LAnkleRoll"]		# 25
	# "XPosition"		# 26
	# "YPosition"		# 27
	# "Orientation"		# 28
	# "XAngle"		# 29
	# "YAngle"		# 30

jointBuffer = {}

ALMEMORY_KEY_NAMES = []

i = 0
for j in joints:
	if i <= 25:
		ALMEMORY_KEY_NAMES.append((i, "Device/SubDeviceList/{j}/Position/Sensor/Value".format(j=j)))
	else:
		ALMEMORY_KEY_NAMES.append((i, "None"))
	jointBuffer[i] = float("inf")
	i += 1

import os
import sys
import time
import itertools
from struct import *
from naoqi import ALProxy
from threading import Timer

memory = ALProxy("ALMemory", "127.0.0.1", 9559)
motion = ALProxy("ALMotion", "127.0.0.1", 9559)

def query():
	for (idx, key) in ALMEMORY_KEY_NAMES:
		if idx == 14:
			continue
		
		v = memory.getData(key)
		
		if abs(v - jointBuffer[idx]) > 0.0025:
			jointBuffer[idx] = v
			if idx == 20:
				connection.send(pack('<Bf', 14, -v))
			connection.send(pack('<Bf', idx, v))
			# if idx == 20:
			# 	print(unpack('<Bf', (pack('<Bf', 14, -v))))
			# print(unpack('<Bf', (pack('<Bf', idx, v))))
	
	x, y, z, wx, wy, wz = motion.getPosition("Torso", 1, True)
	connection.send(pack('<Bf', 26, x))
	connection.send(pack('<Bf', 27, y))
	connection.send(pack('<Bf', 28, z))
	connection.send(pack('<Bf', 29, wx))
	connection.send(pack('<Bf', 30, wy))
	connection.send(pack('<Bf', 31, wz))

class RepeatedTimer(object):
	def __init__(self, interval, function, *args, **kwargs):
		self._timer     = None
		self.interval   = interval
		self.function   = function
		self.args       = args
		self.kwargs     = kwargs
		self.is_running = False
	
	def _run(self):
		self.is_running = False
		self.function(*self.args, **self.kwargs)
		self.start()
		
	def start(self):
		if not self.is_running:
			self._timer = Timer(self.interval, self._run)
			self._timer.start()
			self.is_running = True
	
	def stop(self):
		self._timer.cancel()
		self.is_running = False

rt = RepeatedTimer(0.02, query)
