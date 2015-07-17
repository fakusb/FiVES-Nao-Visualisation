# vim: fenc=utf-8 foldmethod=marker
# call this script with nao-dummy.html as argument.

import os
import sys
import math
from subprocess import call, Popen, PIPE

if not len(sys.argv) > 1:
    print("No file argument given.")
    sys.exit()

infile = sys.argv[1]
if not os.path.isfile(infile):
    print("No valid file argument given.")
    sys.exit()

vals = {}

HeadYaw = 42
HeadPitch = -23

RShoulderPitch = -50
RShoulderRoll = -50
RElbowRoll = 60
RElbowYaw = 45
RWristYaw = 68
RHand = 0

LShoulderPitch = 0
LShoulderRoll = 0
LElbowYaw = 0
LElbowRoll = -40
LWristYaw = 50
LHand = 0

RHipYawPitch = -65
RHipPitch = -19
RHipRoll = 13
RKneePitch = 55
RAnklePitch = -16
RAnkleRoll = 0

LHipYawPitch = -65
LHipPitch = 0
LHipRoll = 23
LKneePitch = 0
LAnklePitch = 13
LAnkleRoll = -23

fmtstr = "{:.6f}"

# chest & head {{{
vals['chest_1_1'] = '0.010000'
vals['chest_1_2'] = '0.000000'
vals['chest_1_3'] = '0.000000'
vals['chest_2_1'] = '0.000000'
vals['chest_2_2'] = '0.010000'
vals['chest_2_3'] = '0.000000'
vals['chest_3_1'] = '0.000000'
vals['chest_3_2'] = '0.000000'
vals['chest_3_3'] = '0.010000'

vals['neck_1_1'] = fmtstr.format(math.cos(math.radians(-HeadYaw)))#'1.000000'
vals['neck_1_2'] = fmtstr.format(-math.sin(math.radians(-HeadYaw)))#'0.000000'
vals['neck_1_3'] = '0.000000'
vals['neck_2_1'] = fmtstr.format(math.sin(math.radians(-HeadYaw)))#'0.000000'
vals['neck_2_2'] = fmtstr.format(math.cos(math.radians(-HeadYaw)))#'1.000000'
vals['neck_2_3'] = '0.000000'
vals['neck_3_1'] = '0.000000'
vals['neck_3_2'] = '0.000000'
vals['neck_3_3'] = '1.000000'

vals['head_1_1'] = fmtstr.format(math.cos(math.radians(-HeadPitch)))#'1.000000'
vals['head_1_2'] = '0.000000'
vals['head_1_3'] = fmtstr.format(math.sin(math.radians(-HeadPitch)))#'0.000000'
vals['head_2_1'] = '0.000000'
vals['head_2_2'] = '1.000000'
vals['head_2_3'] = '0.000000'
vals['head_3_1'] = fmtstr.format(-math.sin(math.radians(-HeadPitch)))#'0.000000'
vals['head_3_2'] = '0.000000'
vals['head_3_3'] = fmtstr.format(math.cos(math.radians(-HeadPitch)))#'1.000000'
# }}}

# right arm {{{
vals['rshoulder_1_1'] = fmtstr.format(math.cos(math.radians(-RShoulderPitch)))#'1.000000'
vals['rshoulder_1_2'] = '0.000000'
vals['rshoulder_1_3'] = fmtstr.format(math.sin(math.radians(-RShoulderPitch)))#'0.000000'
vals['rshoulder_2_1'] = '0.000000'
vals['rshoulder_2_2'] = '1.000000'
vals['rshoulder_2_3'] = '0.000000'
vals['rshoulder_3_1'] = fmtstr.format(-math.sin(math.radians(-RShoulderPitch)))#'0.000000'
vals['rshoulder_3_2'] = '0.000000'
vals['rshoulder_3_3'] = fmtstr.format(math.cos(math.radians(-RShoulderPitch)))#'1.000000'

vals['rbiceps_1_1'] = fmtstr.format(math.cos(math.radians(-RShoulderRoll)))#'1.000000'
vals['rbiceps_1_2'] = fmtstr.format(-math.sin(math.radians(-RShoulderRoll)))#'0.000000'
vals['rbiceps_1_3'] = '0.000000'
vals['rbiceps_2_1'] = fmtstr.format(math.sin(math.radians(-RShoulderRoll)))#'0.000000'
vals['rbiceps_2_2'] = fmtstr.format(math.cos(math.radians(-RShoulderRoll)))#'1.000000'
vals['rbiceps_2_3'] = '0.000000'
vals['rbiceps_3_1'] = '0.000000'
vals['rbiceps_3_2'] = '0.000000'
vals['rbiceps_3_3'] = '1.000000'


rym11 = 1.0
rym12 = 0.0
rym13 = 0.0
rym21 = 0.0
rym22 = math.cos(math.radians(-RElbowYaw))
rym23 = -math.sin(math.radians(-RElbowYaw))
rym31 = 0.0
rym32 = math.sin(math.radians(-RElbowYaw))
rym33 = math.cos(math.radians(-RElbowYaw))

rrm11 = math.cos(math.radians(-RElbowRoll))
rrm12 = -math.sin(math.radians(-RElbowRoll))
rrm13 = 0.0
rrm21 = math.sin(math.radians(-RElbowRoll))
rrm22 = math.cos(math.radians(-RElbowRoll))
rrm23 = 0.0
rrm31 = 0.0
rrm32 = 0.0
rrm33 = 1.0


# first yaw, then roll
vals['rforearm_1_1'] = fmtstr.format(rrm11*rym11+rrm12*rym21+rrm13*rym31)###'1.000000'
vals['rforearm_1_2'] = fmtstr.format(rrm11*rym12+rrm12*rym22+rrm13*rym32)###'0.000000'
vals['rforearm_1_3'] = fmtstr.format(rrm11*rym13+rrm12*rym23+rrm13*rym33)###'0.000000'
vals['rforearm_2_1'] = fmtstr.format(rrm21*rym11+rrm22*rym21+rrm23*rym31)###'0.000000'
vals['rforearm_2_2'] = fmtstr.format(rrm21*rym12+rrm22*rym22+rrm23*rym32)###'1.000000'
vals['rforearm_2_3'] = fmtstr.format(rrm21*rym13+rrm22*rym23+rrm23*rym33)###'0.000000'
vals['rforearm_3_1'] = fmtstr.format(rrm31*rym11+rrm32*rym21+rrm33*rym31)###'0.000000'
vals['rforearm_3_2'] = fmtstr.format(rrm31*rym12+rrm32*rym22+rrm33*rym32)###'0.000000'
vals['rforearm_3_3'] = fmtstr.format(rrm31*rym13+rrm32*rym23+rrm33*rym33)###'1.000000'

vals['rhand_1_1'] = '1.000000'
vals['rhand_1_2'] = '0.000000'
vals['rhand_1_3'] = '0.000000'
vals['rhand_2_1'] = '0.000000'
vals['rhand_2_2'] = fmtstr.format(math.cos(math.radians(-RWristYaw)))#'1.000000'
vals['rhand_2_3'] = fmtstr.format(-math.sin(math.radians(-RWristYaw)))#'0.000000'
vals['rhand_3_1'] = '0.000000'
vals['rhand_3_2'] = fmtstr.format(math.sin(math.radians(-RWristYaw)))#'0.000000'
vals['rhand_3_3'] = fmtstr.format(math.cos(math.radians(-RWristYaw)))#'1.000000'

vals['rphalanx7_1_1'] = '1.000000'
vals['rphalanx7_1_2'] = '0.000000'
vals['rphalanx7_1_3'] = '0.000000'
vals['rphalanx7_2_1'] = '0.000000'
vals['rphalanx7_2_2'] = '1.000000'
vals['rphalanx7_2_3'] = '0.000000'
vals['rphalanx7_3_1'] = '0.000000'
vals['rphalanx7_3_2'] = '0.000000'
vals['rphalanx7_3_3'] = '1.000000'

vals['rphalanx8_1_1'] = '1.000000'
vals['rphalanx8_1_2'] = '0.000000'
vals['rphalanx8_1_3'] = '0.000000'
vals['rphalanx8_2_1'] = '0.000000'
vals['rphalanx8_2_2'] = '1.000000'
vals['rphalanx8_2_3'] = '0.000000'
vals['rphalanx8_3_1'] = '0.000000'
vals['rphalanx8_3_2'] = '0.000000'
vals['rphalanx8_3_3'] = '1.000000'

vals['rphalanx4_1_1'] = '1.000000'
vals['rphalanx4_1_2'] = '0.000000'
vals['rphalanx4_1_3'] = '0.000000'
vals['rphalanx4_2_1'] = '0.000000'
vals['rphalanx4_2_2'] = '1.000000'
vals['rphalanx4_2_3'] = '0.000000'
vals['rphalanx4_3_1'] = '0.000000'
vals['rphalanx4_3_2'] = '0.000000'
vals['rphalanx4_3_3'] = '1.000000'

vals['rphalanx5_1_1'] = '1.000000'
vals['rphalanx5_1_2'] = '0.000000'
vals['rphalanx5_1_3'] = '0.000000'
vals['rphalanx5_2_1'] = '0.000000'
vals['rphalanx5_2_2'] = '1.000000'
vals['rphalanx5_2_3'] = '0.000000'
vals['rphalanx5_3_1'] = '0.000000'
vals['rphalanx5_3_2'] = '0.000000'
vals['rphalanx5_3_3'] = '1.000000'

vals['rphalanx6_1_1'] = '1.000000'
vals['rphalanx6_1_2'] = '0.000000'
vals['rphalanx6_1_3'] = '0.000000'
vals['rphalanx6_2_1'] = '0.000000'
vals['rphalanx6_2_2'] = '1.000000'
vals['rphalanx6_2_3'] = '0.000000'
vals['rphalanx6_3_1'] = '0.000000'
vals['rphalanx6_3_2'] = '0.000000'
vals['rphalanx6_3_3'] = '1.000000'

vals['rphalanx1_1_1'] = '1.000000'
vals['rphalanx1_1_2'] = '0.000000'
vals['rphalanx1_1_3'] = '0.000000'
vals['rphalanx1_2_1'] = '0.000000'
vals['rphalanx1_2_2'] = '1.000000'
vals['rphalanx1_2_3'] = '0.000000'
vals['rphalanx1_3_1'] = '0.000000'
vals['rphalanx1_3_2'] = '0.000000'
vals['rphalanx1_3_3'] = '1.000000'

vals['rphalanx2_1_1'] = '1.000000'
vals['rphalanx2_1_2'] = '0.000000'
vals['rphalanx2_1_3'] = '0.000000'
vals['rphalanx2_2_1'] = '0.000000'
vals['rphalanx2_2_2'] = '1.000000'
vals['rphalanx2_2_3'] = '0.000000'
vals['rphalanx2_3_1'] = '0.000000'
vals['rphalanx2_3_2'] = '0.000000'
vals['rphalanx2_3_3'] = '1.000000'

vals['rphalanx3_1_1'] = '1.000000'
vals['rphalanx3_1_2'] = '0.000000'
vals['rphalanx3_1_3'] = '0.000000'
vals['rphalanx3_2_1'] = '0.000000'
vals['rphalanx3_2_2'] = '1.000000'
vals['rphalanx3_2_3'] = '0.000000'
vals['rphalanx3_3_1'] = '0.000000'
vals['rphalanx3_3_2'] = '0.000000'
vals['rphalanx3_3_3'] = '1.000000'
# }}}

# left arm {{{
vals['lshoulder_1_1'] = fmtstr.format(math.cos(math.radians(-LShoulderPitch)))#'1.000000'
vals['lshoulder_1_2'] = '0.000000'
vals['lshoulder_1_3'] = fmtstr.format(math.sin(math.radians(-LShoulderPitch)))#'0.000000'
vals['lshoulder_2_1'] = '0.000000'
vals['lshoulder_2_2'] = '1.000000'
vals['lshoulder_2_3'] = '0.000000'
vals['lshoulder_3_1'] = fmtstr.format(-math.sin(math.radians(-LShoulderPitch)))#'0.000000'
vals['lshoulder_3_2'] = '0.000000'
vals['lshoulder_3_3'] = fmtstr.format(math.cos(math.radians(-LShoulderPitch)))#'1.000000'

vals['lbiceps_1_1'] = fmtstr.format(math.cos(math.radians(-LShoulderRoll)))#'1.000000'
vals['lbiceps_1_2'] = fmtstr.format(-math.sin(math.radians(-LShoulderRoll)))#'0.000000'
vals['lbiceps_1_3'] = '0.000000'
vals['lbiceps_2_1'] = fmtstr.format(math.sin(math.radians(-LShoulderRoll)))#'0.000000'
vals['lbiceps_2_2'] = fmtstr.format(math.cos(math.radians(-LShoulderRoll)))#'1.000000'
vals['lbiceps_2_3'] = '0.000000'
vals['lbiceps_3_1'] = '0.000000'
vals['lbiceps_3_2'] = '0.000000'
vals['lbiceps_3_3'] = '1.000000'


lym11 = 1.0
lym12 = 0.0
lym13 = 0.0
lym21 = 0.0
lym22 = math.cos(math.radians(-LElbowYaw))
lym23 = -math.sin(math.radians(-LElbowYaw))
lym31 = 0.0
lym32 = math.sin(math.radians(-LElbowYaw))
lym33 = math.cos(math.radians(-LElbowYaw))

lrm11 = math.cos(math.radians(-LElbowRoll))
lrm12 = -math.sin(math.radians(-LElbowRoll))
lrm13 = 0.0
lrm21 = math.sin(math.radians(-LElbowRoll))
lrm22 = math.cos(math.radians(-LElbowRoll))
lrm23 = 0.0
lrm31 = 0.0
lrm32 = 0.0
lrm33 = 1.0


# first yaw, then roll
vals['lforearm_1_1'] = fmtstr.format(lrm11*lym11+lrm12*lym21+lrm13*lym31)###'1.000000'
vals['lforearm_1_2'] = fmtstr.format(lrm11*lym12+lrm12*lym22+lrm13*lym32)###'0.000000'
vals['lforearm_1_3'] = fmtstr.format(lrm11*lym13+lrm12*lym23+lrm13*lym33)###'0.000000'
vals['lforearm_2_1'] = fmtstr.format(lrm21*lym11+lrm22*lym21+lrm23*lym31)###'0.000000'
vals['lforearm_2_2'] = fmtstr.format(lrm21*lym12+lrm22*lym22+lrm23*lym32)###'1.000000'
vals['lforearm_2_3'] = fmtstr.format(lrm21*lym13+lrm22*lym23+lrm23*lym33)###'0.000000'
vals['lforearm_3_1'] = fmtstr.format(lrm31*lym11+lrm32*lym21+lrm33*lym31)###'0.000000'
vals['lforearm_3_2'] = fmtstr.format(lrm31*lym12+lrm32*lym22+lrm33*lym32)###'0.000000'
vals['lforearm_3_3'] = fmtstr.format(lrm31*lym13+lrm32*lym23+lrm33*lym33)###'1.000000'

vals['lhand_1_1'] = '1.000000'
vals['lhand_1_2'] = '0.000000'
vals['lhand_1_3'] = '0.000000'
vals['lhand_2_1'] = '0.000000'
vals['lhand_2_2'] = fmtstr.format(math.cos(math.radians(-LWristYaw)))#'1.000000'
vals['lhand_2_3'] = fmtstr.format(-math.sin(math.radians(-LWristYaw)))#'0.000000'
vals['lhand_3_1'] = '0.000000'
vals['lhand_3_2'] = fmtstr.format(math.sin(math.radians(-LWristYaw)))#'0.000000'
vals['lhand_3_3'] = fmtstr.format(math.cos(math.radians(-LWristYaw)))#'1.000000'

vals['lphalanx7_1_1'] = '1.000000'
vals['lphalanx7_1_2'] = '0.000000'
vals['lphalanx7_1_3'] = '0.000000'
vals['lphalanx7_2_1'] = '0.000000'
vals['lphalanx7_2_2'] = '1.000000'
vals['lphalanx7_2_3'] = '0.000000'
vals['lphalanx7_3_1'] = '0.000000'
vals['lphalanx7_3_2'] = '0.000000'
vals['lphalanx7_3_3'] = '1.000000'

vals['lphalanx8_1_1'] = '1.000000'
vals['lphalanx8_1_2'] = '0.000000'
vals['lphalanx8_1_3'] = '0.000000'
vals['lphalanx8_2_1'] = '0.000000'
vals['lphalanx8_2_2'] = '1.000000'
vals['lphalanx8_2_3'] = '0.000000'
vals['lphalanx8_3_1'] = '0.000000'
vals['lphalanx8_3_2'] = '0.000000'
vals['lphalanx8_3_3'] = '1.000000'

vals['lphalanx4_1_1'] = '1.000000'
vals['lphalanx4_1_2'] = '0.000000'
vals['lphalanx4_1_3'] = '0.000000'
vals['lphalanx4_2_1'] = '0.000000'
vals['lphalanx4_2_2'] = '1.000000'
vals['lphalanx4_2_3'] = '0.000000'
vals['lphalanx4_3_1'] = '0.000000'
vals['lphalanx4_3_2'] = '0.000000'
vals['lphalanx4_3_3'] = '1.000000'

vals['lphalanx5_1_1'] = '1.000000'
vals['lphalanx5_1_2'] = '0.000000'
vals['lphalanx5_1_3'] = '0.000000'
vals['lphalanx5_2_1'] = '0.000000'
vals['lphalanx5_2_2'] = '1.000000'
vals['lphalanx5_2_3'] = '0.000000'
vals['lphalanx5_3_1'] = '0.000000'
vals['lphalanx5_3_2'] = '0.000000'
vals['lphalanx5_3_3'] = '1.000000'

vals['lphalanx6_1_1'] = '1.000000'
vals['lphalanx6_1_2'] = '0.000000'
vals['lphalanx6_1_3'] = '0.000000'
vals['lphalanx6_2_1'] = '0.000000'
vals['lphalanx6_2_2'] = '1.000000'
vals['lphalanx6_2_3'] = '0.000000'
vals['lphalanx6_3_1'] = '0.000000'
vals['lphalanx6_3_2'] = '0.000000'
vals['lphalanx6_3_3'] = '1.000000'

vals['lphalanx1_1_1'] = '1.000000'
vals['lphalanx1_1_2'] = '0.000000'
vals['lphalanx1_1_3'] = '0.000000'
vals['lphalanx1_2_1'] = '0.000000'
vals['lphalanx1_2_2'] = '1.000000'
vals['lphalanx1_2_3'] = '0.000000'
vals['lphalanx1_3_1'] = '0.000000'
vals['lphalanx1_3_2'] = '0.000000'
vals['lphalanx1_3_3'] = '1.000000'

vals['lphalanx2_1_1'] = '1.000000'
vals['lphalanx2_1_2'] = '0.000000'
vals['lphalanx2_1_3'] = '0.000000'
vals['lphalanx2_2_1'] = '0.000000'
vals['lphalanx2_2_2'] = '1.000000'
vals['lphalanx2_2_3'] = '0.000000'
vals['lphalanx2_3_1'] = '0.000000'
vals['lphalanx2_3_2'] = '0.000000'
vals['lphalanx2_3_3'] = '1.000000'

vals['lphalanx3_1_1'] = '1.000000'
vals['lphalanx3_1_2'] = '0.000000'
vals['lphalanx3_1_3'] = '0.000000'
vals['lphalanx3_2_1'] = '0.000000'
vals['lphalanx3_2_2'] = '1.000000'
vals['lphalanx3_2_3'] = '0.000000'
vals['lphalanx3_3_1'] = '0.000000'
vals['lphalanx3_3_2'] = '0.000000'
vals['lphalanx3_3_3'] = '1.000000'
# }}}

# right leg {{{

rhux = 0
rhuy = -1/math.sqrt(2)
rhuz = -1/math.sqrt(2)
rhl11 = math.cos(math.radians(RHipYawPitch))
# no - here!
rhl12 = math.sin(math.radians(RHipYawPitch)) * (-rhuz)
rhl13 = math.sin(math.radians(RHipYawPitch)) * (rhuy)
rhl21 = math.sin(math.radians(RHipYawPitch)) * (rhuz)
rhl22 = math.cos(math.radians(RHipYawPitch))
rhl23 = math.sin(math.radians(RHipYawPitch)) * (-rhux)
rhl31 = math.sin(math.radians(RHipYawPitch)) * (-rhuy)
rhl32 = math.sin(math.radians(RHipYawPitch)) * (rhux)
rhl33 = math.cos(math.radians(RHipYawPitch))

rhr11 = (1 - math.cos(math.radians(RHipYawPitch))) * rhux * rhux
rhr12 = (1 - math.cos(math.radians(RHipYawPitch))) * rhux * rhuy
rhr13 = (1 - math.cos(math.radians(RHipYawPitch))) * rhux * rhuz
rhr21 = (1 - math.cos(math.radians(RHipYawPitch))) * rhuy * rhux
rhr22 = (1 - math.cos(math.radians(RHipYawPitch))) * rhuy * rhuy
rhr23 = (1 - math.cos(math.radians(RHipYawPitch))) * rhuy * rhuz
rhr31 = (1 - math.cos(math.radians(RHipYawPitch))) * rhuz * rhux
rhr32 = (1 - math.cos(math.radians(RHipYawPitch))) * rhuz * rhuy
rhr33 = (1 - math.cos(math.radians(RHipYawPitch))) * rhuz * rhuz

vals['rhip_1_1'] = fmtstr.format(rhl11 + rhr11)#'1.000000'
vals['rhip_1_2'] = fmtstr.format(rhl12 + rhr12)#'0.000000'
vals['rhip_1_3'] = fmtstr.format(rhl13 + rhr13)#'0.000000'
vals['rhip_2_1'] = fmtstr.format(rhl21 + rhr21)#'0.000000'
vals['rhip_2_2'] = fmtstr.format(rhl22 + rhr22)#'1.000000'
vals['rhip_2_3'] = fmtstr.format(rhl23 + rhr23)#'0.000000'
vals['rhip_3_1'] = fmtstr.format(rhl31 + rhr31)#'0.000000'
vals['rhip_3_2'] = fmtstr.format(rhl32 + rhr32)#'0.000000'
vals['rhip_3_3'] = fmtstr.format(rhl33 + rhr33)#'1.000000'

vals['rupperthigh_1_1'] = '1.000000'
vals['rupperthigh_1_2'] = '0.000000'
vals['rupperthigh_1_3'] = '0.000000'
vals['rupperthigh_2_1'] = '0.000000'
vals['rupperthigh_2_2'] = fmtstr.format(math.cos(math.radians(-RHipRoll)))#'1.000000'
vals['rupperthigh_2_3'] = fmtstr.format(-math.sin(math.radians(-RHipRoll)))#'0.000000'
vals['rupperthigh_3_1'] = '0.000000'
vals['rupperthigh_3_2'] = fmtstr.format(math.sin(math.radians(-RHipRoll)))#'0.000000'
vals['rupperthigh_3_3'] = fmtstr.format(math.cos(math.radians(-RHipRoll)))#'1.000000'

vals['rthigh_1_1'] = fmtstr.format(math.cos(math.radians(-RHipPitch)))#'1.000000'
vals['rthigh_1_2'] = '0.000000'
vals['rthigh_1_3'] = fmtstr.format(math.sin(math.radians(-RHipPitch)))#'0.000000'
vals['rthigh_2_1'] = '0.000000'
vals['rthigh_2_2'] = '1.000000'
vals['rthigh_2_3'] = '0.000000'
vals['rthigh_3_1'] = fmtstr.format(-math.sin(math.radians(-RHipPitch)))#'0.000000'
vals['rthigh_3_2'] = '0.000000'
vals['rthigh_3_3'] = fmtstr.format(math.cos(math.radians(-RHipPitch)))#'1.000000'

vals['rshinebone_1_1'] = fmtstr.format(math.cos(math.radians(-RKneePitch)))#'1.000000'
vals['rshinebone_1_2'] = '0.000000'
vals['rshinebone_1_3'] = fmtstr.format(math.sin(math.radians(-RKneePitch)))#'0.000000'
vals['rshinebone_2_1'] = '0.000000'
vals['rshinebone_2_2'] = '1.000000'
vals['rshinebone_2_3'] = '0.000000'
vals['rshinebone_3_1'] = fmtstr.format(-math.sin(math.radians(-RKneePitch)))#'0.000000'
vals['rshinebone_3_2'] = '0.000000'
vals['rshinebone_3_3'] = fmtstr.format(math.cos(math.radians(-RKneePitch)))#'1.000000'

vals['rankle_1_1'] = fmtstr.format(math.cos(math.radians(-RAnklePitch)))#'1.000000'
vals['rankle_1_2'] = '0.000000'
vals['rankle_1_3'] = fmtstr.format(math.sin(math.radians(-RAnklePitch)))#'0.000000'
vals['rankle_2_1'] = '0.000000'
vals['rankle_2_2'] = '1.000000'
vals['rankle_2_3'] = '0.000000'
vals['rankle_3_1'] = fmtstr.format(-math.sin(math.radians(-RAnklePitch)))#'0.000000'
vals['rankle_3_2'] = '0.000000'
vals['rankle_3_3'] = fmtstr.format(math.cos(math.radians(-RAnklePitch)))#'1.000000'

vals['rfoot_1_1'] = '1.000000'
vals['rfoot_1_2'] = '0.000000'
vals['rfoot_1_3'] = '0.000000'
vals['rfoot_2_1'] = '0.000000'
vals['rfoot_2_2'] = fmtstr.format(math.cos(math.radians(-RAnkleRoll)))#'1.000000'
vals['rfoot_2_3'] = fmtstr.format(-math.sin(math.radians(-RAnkleRoll)))#'0.000000'
vals['rfoot_3_1'] = '0.000000'
vals['rfoot_3_2'] = fmtstr.format(math.sin(math.radians(-RAnkleRoll)))#'0.000000'
vals['rfoot_3_3'] = fmtstr.format(math.cos(math.radians(-RAnkleRoll)))#'1.000000'
# }}}

# left leg {{{

lhux = 0
lhuy = 1/math.sqrt(2)
lhuz = -1/math.sqrt(2)

lhl11 = math.cos(math.radians(-LHipYawPitch))
lhl12 = math.sin(math.radians(-LHipYawPitch)) * (-lhuz)
lhl13 = math.sin(math.radians(-LHipYawPitch)) * (lhuy)
lhl21 = math.sin(math.radians(-LHipYawPitch)) * (lhuz)
lhl22 = math.cos(math.radians(-LHipYawPitch))
lhl23 = math.sin(math.radians(-LHipYawPitch)) * (-lhux)
lhl31 = math.sin(math.radians(-LHipYawPitch)) * (-lhuy)
lhl32 = math.sin(math.radians(-LHipYawPitch)) * (lhux)
lhl33 = math.cos(math.radians(-LHipYawPitch))

lhr11 = (1 - math.cos(math.radians(-LHipYawPitch))) * lhux * lhux
lhr12 = (1 - math.cos(math.radians(-LHipYawPitch))) * lhux * lhuy
lhr13 = (1 - math.cos(math.radians(-LHipYawPitch))) * lhux * lhuz
lhr21 = (1 - math.cos(math.radians(-LHipYawPitch))) * lhuy * lhux
lhr22 = (1 - math.cos(math.radians(-LHipYawPitch))) * lhuy * lhuy
lhr23 = (1 - math.cos(math.radians(-LHipYawPitch))) * lhuy * lhuz
lhr31 = (1 - math.cos(math.radians(-LHipYawPitch))) * lhuz * lhux
lhr32 = (1 - math.cos(math.radians(-LHipYawPitch))) * lhuz * lhuy
lhr33 = (1 - math.cos(math.radians(-LHipYawPitch))) * lhuz * lhuz

vals['lhip_1_1'] = fmtstr.format(lhl11 + lhr11)#'1.000000'
vals['lhip_1_2'] = fmtstr.format(lhl12 + lhr12)#'0.000000'
vals['lhip_1_3'] = fmtstr.format(lhl13 + lhr13)#'0.000000'
vals['lhip_2_1'] = fmtstr.format(lhl21 + lhr21)#'0.000000'
vals['lhip_2_2'] = fmtstr.format(lhl22 + lhr22)#'1.000000'
vals['lhip_2_3'] = fmtstr.format(lhl23 + lhr23)#'0.000000'
vals['lhip_3_1'] = fmtstr.format(lhl31 + lhr31)#'0.000000'
vals['lhip_3_2'] = fmtstr.format(lhl32 + lhr32)#'0.000000'
vals['lhip_3_3'] = fmtstr.format(lhl33 + lhr33)#'1.000000'

vals['lupperthigh_1_1'] = '1.000000'
vals['lupperthigh_1_2'] = '0.000000'
vals['lupperthigh_1_3'] = '0.000000'
vals['lupperthigh_2_1'] = '0.000000'
vals['lupperthigh_2_2'] = fmtstr.format(math.cos(math.radians(-LHipRoll)))#'1.000000'
vals['lupperthigh_2_3'] = fmtstr.format(-math.sin(math.radians(-LHipRoll)))#'0.000000'
vals['lupperthigh_3_1'] = '0.000000'
vals['lupperthigh_3_2'] = fmtstr.format(math.sin(math.radians(-LHipRoll)))#'0.000000'
vals['lupperthigh_3_3'] = fmtstr.format(math.cos(math.radians(-LHipRoll)))#'1.000000'

vals['lthigh_1_1'] = fmtstr.format(math.cos(math.radians(-LHipPitch)))#'1.000000'
vals['lthigh_1_2'] = '0.000000'
vals['lthigh_1_3'] = fmtstr.format(math.sin(math.radians(-LHipPitch)))#'0.000000'
vals['lthigh_2_1'] = '0.000000'
vals['lthigh_2_2'] = '1.000000'
vals['lthigh_2_3'] = '0.000000'
vals['lthigh_3_1'] = fmtstr.format(-math.sin(math.radians(-LHipPitch)))#'0.000000'
vals['lthigh_3_2'] = '0.000000'
vals['lthigh_3_3'] = fmtstr.format(math.cos(math.radians(-LHipPitch)))#'1.000000'

vals['lshinebone_1_1'] = fmtstr.format(math.cos(math.radians(-LKneePitch)))#'1.000000'
vals['lshinebone_1_2'] = '0.000000'
vals['lshinebone_1_3'] = fmtstr.format(math.sin(math.radians(-LKneePitch)))#'0.000000'
vals['lshinebone_2_1'] = '0.000000'
vals['lshinebone_2_2'] = '1.000000'
vals['lshinebone_2_3'] = '0.000000'
vals['lshinebone_3_1'] = fmtstr.format(-math.sin(math.radians(-LKneePitch)))#'0.000000'
vals['lshinebone_3_2'] = '0.000000'
vals['lshinebone_3_3'] = fmtstr.format(math.cos(math.radians(-LKneePitch)))#'1.000000'

vals['lankle_1_1'] = fmtstr.format(math.cos(math.radians(-LAnklePitch)))#'1.000000'
vals['lankle_1_2'] = '0.000000'
vals['lankle_1_3'] = fmtstr.format(math.sin(math.radians(-LAnklePitch)))#'0.000000'
vals['lankle_2_1'] = '0.000000'
vals['lankle_2_2'] = '1.000000'
vals['lankle_2_3'] = '0.000000'
vals['lankle_3_1'] = fmtstr.format(-math.sin(math.radians(-LAnklePitch)))#'0.000000'
vals['lankle_3_2'] = '0.000000'
vals['lankle_3_3'] = fmtstr.format(math.cos(math.radians(-LAnklePitch)))#'1.000000'

vals['lfoot_1_1'] = '1.000000'
vals['lfoot_1_2'] = '0.000000'
vals['lfoot_1_3'] = '0.000000'
vals['lfoot_2_1'] = '0.000000'
vals['lfoot_2_2'] = fmtstr.format(math.cos(math.radians(-LAnkleRoll)))#'1.000000'
vals['lfoot_2_3'] = fmtstr.format(-math.sin(math.radians(-LAnkleRoll)))#'0.000000'
vals['lfoot_3_1'] = '0.000000'
vals['lfoot_3_2'] = fmtstr.format(math.sin(math.radians(-LAnkleRoll)))#'0.000000'
vals['lfoot_3_3'] = fmtstr.format(math.cos(math.radians(-LAnkleRoll)))#'1.000000'
# }}}

s = "{"

for key in vals:
    s += "s/_"+key+"_/"+vals[key]+"/ "+os.linesep
s += "}"

# print(s)

with open(os.path.splitext(infile)[0]+".mod.html",'w') as f:
    p = call(['sed', '-e', s, infile], stdout=f)

