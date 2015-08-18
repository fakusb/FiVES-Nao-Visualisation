joints = {
	0: "HeadYaw",
	1: "HeadPitch",
	2: "RShoulderPitch",
	3: "RShoulderRoll",
	4: "RElbowRoll",
	5: "RElbowYaw",
	6: "RWristYaw",
	7: "RHand",
	8: "LShoulderPitch",
	9: "LShoulderRoll",
	10: "LElbowRoll",
	11: "LElbowYaw",
	12: "LWristYaw",
	13: "LHand",
	14: "RHipYawPitch",
	15: "RHipPitch",
	16: "RHipRoll",
	17: "RKneePitch",
	18: "RAnklePitch",
	19: "RAnkleRoll",
	20: "LHipYawPitch",
	21: "LHipPitch",
	22: "LHipRoll",
	23: "LKneePitch",
	24: "LAnklePitch",
	25: "LAnkleRoll"}


ALMEMORY_KEY_NAMES = []

for j in joints.itervalues():
	ALMEMORY_KEY_NAMES.append("Device/SubDeviceList/{j}/Position/Sensor/Value".format(j=j))

import os
import sys
import time
import itertools
from struct import *
from naoqi import ALProxy
from threading import Timer

memory = ALProxy("ALMemory", "127.0.0.1", 9559)

def query():
    for (num, key) in itertools.izip(joints, ALMEMORY_KEY_NAMES):
    	if num == 14:
    		continue
    	elif num == 20:
    		connection.send(pack('<Bf', 14, -memory.getData(key)))
        connection.send(pack('<Bf', num, memory.getData(key)))

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
        self.start()
        self.function(*self.args, **self.kwargs)
    
    def start(self):
        if not self.is_running:
            self._timer = Timer(self.interval, self._run)
            self._timer.start()
            self.is_running = True
    
    def stop(self):
        self._timer.cancel()
        self.is_running = False

rt = RepeatedTimer(0.02, query)
