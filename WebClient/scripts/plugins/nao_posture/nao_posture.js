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
    var dict = {};
    dict["HeadYaw"] = {obj: "Head_transform", axis: zAxis}
    dict["HeadPitch"] = {obj: "Neck_transform", axis: yAxis}

    dict["RShoulderPitch"] = {obj: "RShoulderPadMobil_transform", axis: yAxis}
    // dict["RShoulderRoll"] = {obj: "Head_transform", axis: xAxis}
    // dict["RElbowRoll"] = {obj: "Head_transform", axis: xAxis}
    // dict["RElbowYaw"] = {obj: "Head_transform", axis: zAxis}
    // dict["RWristYaw"] = {obj: "Head_transform", axis: zAxis}
    //
    // dict["LShoulderPitch"] = {obj: "Head_transform", axis: yAxis}
    // dict["LShoulderRoll"] = {obj: "Head_transform", axis: xAxis}
    // dict["LElbowRoll"] = {obj: "Head_transform", axis: xAxis}
    // dict["LElbowYaw"] = {obj: "Head_transform", axis: zAxis}
    // dict["LWristYaw"] = {obj: "Head_transform", axis: zAxis}
    //
    // // dict["RHipYawPitch"] = {obj: "Head_transform", axis: yAxis}
    // dict["RHipPitch"] = {obj: "Head_transform", axis: yAxis}
    // dict["RHipRoll"] = {obj: "Head_transform", axis: xAxis}
    // dict["RKneePitch"] = {obj: "Head_transform", axis: yAxis}
    // dict["RAnklePitch"] = {obj: "Head_transform", axis: yAxis}
    // dict["RAnkleRoll"] = {obj: "Head_transform", axis: xAxis}
    //
    // // dict["LHipYawPitch"] = {obj: "Head_transform", axis: yAxis}
    // dict["LHipPitch"] = {obj: "Head_transform", axis: yAxis}
    // dict["LHipRoll"] = {obj: "Head_transform", axis: xAxis}
    // dict["LKneePitch"] = {obj: "Head_transform", axis: yAxis}
    // dict["LAnklePitch"] = {obj: "Head_transform", axis: yAxis}
    // dict["LAnkleRoll"] = {obj: "Head_transform", axis: xAxis}

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
                        console.log("Caua bunga!");
                        elem.rotation.set(axisAngleRotation);
                    }
                }
            }

            // if (componentName == "nao_posture" && attributeName == "HeadYaw")
            // {
            //     console.log("Here we go!");
            //
            //     var axisAngleRotation = new XML3DRotation();
            //
            //     axisAngleRotation.setAxisAngle(zAxis, entity["nao_posture"]["HeadYaw"]);
            //     var transformationForEntity = entity.getTransformElement();
            //     if(transformationForEntity)
            //     {
            //         console.log("Caua bunga!");
            //         // entity.xml3dView.groupElement.getElementById("Head").transform.rotation.set(axisAngleRotation);
            //         entity.xml3dView.defElement.children[2].rotation.set(axisAngleRotation);
            //         //transformationForEntity.rotation.set(axisAngleRotation);
            //     }



                /*
                var _xml3dElement = FIVES.Resources.SceneManager.xml3dElement;



        var transformTag = XML3D.createElement("transform");
        transformTag.setAttribute("id", "transform-" + entity.guid) ;

        transformTag.rotation.set(this._createRotationFromOrientation(entity));

        _mainDefs.appendChild(transformTag);
        return transformTag;



                        var entityGroup = XML3D.createElement("group");
        entityGroup.setAttribute("id", "Entity-" + entity.guid);
        entityGroup.setAttribute("transform", "#transform-" + entity.guid );
        return entityGroup;
*/

            // }
        }
    };

    l.updateHeadYaw = function(entity, headYaw) {
		console.log("updateHeadYaw");
        //entity.nao_posture.headYaw = headYaw;
        //FIVES.Resources.SceneManager.applyPositionToXML3DView(entity);
    };

    l.setEntityHeadYaw = function(entity, headYaw) {
        this.sendEntityHeadYawUpdate(entity.guid, headYaw);
    };

    l.sendEntityHeadYawUpdate = function(guid, headYaw) {
		console.log("sendEntityHeadYawUpdate");
        //this.updateEntityPosition(guid, position, _fivesCommunicator.generateTimestamp());
    };

    FIVES.Plugins.Posture = new nao_posture();

}());
