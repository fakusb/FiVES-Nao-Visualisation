joints = [
	"HeadYaw",
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

jointBuffer = {}

ALMEMORY_KEY_NAMES = []

i = 0
for j in joints:
	ALMEMORY_KEY_NAMES.append((i, "Device/SubDeviceList/{j}/Position/Sensor/Value".format(j=j)))
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
			#if idx == 20:
			#	print unpack('<Bf', (pack('<Bf', 14, -v)))
			#print unpack('<Bf', (pack('<Bf', idx, v)))

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
