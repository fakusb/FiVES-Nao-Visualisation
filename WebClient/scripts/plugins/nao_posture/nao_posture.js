/**
 * Author: Gereon Fox
 * Date: 7/17/15
 * Time: 12:36 AM
 */

var FIVES = FIVES || {};
FIVES.Plugins = FIVES.Plugins || {};

(function (){
    "use strict";

    var _fivesCommunicator = FIVES.Communication.FivesCommunicator;

    var nao_posture = function() {
        FIVES.Events.AddConnectionEstablishedHandler(this._createFunctionWrappers.bind(this));
        FIVES.Events.AddOnComponentUpdatedHandler(this._componentUpdatedHandler.bind(this));
    };

    var l = nao_posture.prototype;

    l._createFunctionWrappers = function (){
        this.updateEntityHeadYaw = _fivesCommunicator.connection.generateFuncWrapper("nao_posture.updateHeadYaw");
    };

    var xAxis = new XML3DVec3();
    xAxis.x = 1;
    xAxis.y = 0;
    xAxis.z = 0;
    var yAxis = new XML3DVec3();
    yAxis.x = 0;
    yAxis.y = 1;
    yAxis.z = 0;
    var zAxis = new XML3DVec3();
    zAxis.x = 0;
    zAxis.y = 0;
    zAxis.z = 1;
    var rHipAxis = new XML3DVec3();
    rHipAxis.x = 0;
    rHipAxis.y = -1;
    rHipAxis.z = -1;
    var lHipAxis = new XML3DVec3();
    lHipAxis.x = 0;
    lHipAxis.y = 1;
    lHipAxis.z = -1;
    var dict = {};
    dict["HeadYaw"] = {obj: "Head_transform", axis: zAxis}
    dict["HeadPitch"] = {obj: "Neck_transform", axis: yAxis}

    dict["RShoulderPitch"] = {obj: "RShoulderPadMobil_transform", axis: yAxis}
    dict["RShoulderRoll"] = {obj: "RBiceps_transform", axis: zAxis}
    dict["RElbowRoll"] = {obj: "RForeArm_transform", axis: zAxis}
    dict["RElbowYaw"] = {obj: "RElbow_transform", axis: xAxis}
    dict["RWristYaw"] = {obj: "RHand_transform", axis: xAxis}

    dict["LShoulderPitch"] = {obj: "LShoulderPadMobil_transform", axis: yAxis}
    dict["LShoulderRoll"] = {obj: "LBiceps_transform", axis: zAxis}
    dict["LElbowRoll"] = {obj: "LForeArm_transform", axis: zAxis}
    dict["LElbowYaw"] = {obj: "LElbow_transform", axis: xAxis}
    dict["LWristYaw"] = {obj: "LHand_transform", axis: xAxis}

    dict["RHipYawPitch"] = {obj: "RHip_transform", axis: rHipAxis}
    dict["RHipPitch"] = {obj: "RUpperThigh_transform", axis: yAxis}
    dict["RHipRoll"] = {obj: "RThigh_transform", axis: xAxis}
    dict["RKneePitch"] = {obj: "RShinebone_transform", axis: yAxis}
    dict["RAnklePitch"] = {obj: "RAnkle_transform", axis: yAxis}
    dict["RAnkleRoll"] = {obj: "RFoot_transform", axis: xAxis}

    dict["LHipYawPitch"] = {obj: "LHip_transform", axis: lHipAxis}
    dict["LHipPitch"] = {obj: "LUpperThigh_transform", axis: yAxis}
    dict["LHipRoll"] = {obj: "LThigh_transform", axis: xAxis}
    dict["LKneePitch"] = {obj: "LShinebone_transform", axis: yAxis}
    dict["LAnklePitch"] = {obj: "LAnkle_transform", axis: yAxis}
    dict["LAnkleRoll"] = {obj: "LFoot_transform", axis: xAxis}

    l._componentUpdatedHandler = function(entity, componentName, attributeName) {
        if(componentName == "nao_posture")
        {
            //console.log(componentName + "." + attributeName + " = " + entity[componentName][attributeName]);

            var entry = dict[attributeName];
            if (entry)
            {
                var objname = entry.obj;
                var axis = entry.axis;

                var elem;
                var i;
                for (i = 0; i < entity.xml3dView.defElement.children.length ; i++)
                {
                    elem = entity.xml3dView.defElement.children[i];
                    if (elem.id.split("-")[0] == objname)
                    {
                        var axisAngleRotation = new XML3DRotation();
                        axisAngleRotation.setAxisAngle(axis, entity["nao_posture"][attributeName]);
                        // console.log("Caua bunga!");
                        elem.rotation.set(axisAngleRotation);
                        break;
                    }
                }
            }
        }
    };

    FIVES.Plugins.Posture = new nao_posture();

}());
