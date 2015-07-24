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

    l._componentUpdatedHandler = function(entity, componentName, attributeName) {
        if(componentName == "nao_posture")
        {
			//console.log(componentName + "." + attributeName + " = " + entity[componentName][attributeName]);
			
			if (componentName == "nao_posture" && attributeName == "HeadYaw")
			{
				console.log("Here we go!");
				
				var axisAngleRotation = new XML3DRotation();
				
				var zAxis = new XML3DVec3();
				zAxis.x = 0;
				zAxis.y = 0;
				zAxis.z = 1;
				
				axisAngleRotation.setAxisAngle(zAxis, entity["nao_posture"]["HeadYaw"]);
				transformationForEntity.rotation.set(axisAngleRotation);
			}
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
