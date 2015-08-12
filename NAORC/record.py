import sys
import time
from naoqi import ALProxy

IP = "nao.local"
PORT = 9559


videoRecorderProxy = ALProxy("ALVideoRecorder", IP, PORT)

# This records a 320*240 MJPG video at 10 fps.
# Note MJPG can't be recorded with a framerate lower than 3 fps.
videoRecorderProxy.setResolution(1)
videoRecorderProxy.setFrameRate(10)
videoRecorderProxy.setVideoFormat("MJPG")
videoRecorderProxy.startVideoRecord("/home/nao/recordings/cameras", "myvideo")

time.sleep(5)

# Video file is saved on the robot in the
# /home/nao/recordings/cameras/ folder.
videoInfo = videoRecorderProxy.stopVideoRecord()

print "Video was saved on the robot: ", videoInfo[1]
print "Num frames: ", videoInfo[0]