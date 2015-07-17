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

    var posture = function() {
        FIVES.Events.AddConnectionEstablishedHandler(this._createFunctionWrappers.bind(this));
        FIVES.Events.AddOnComponentUpdatedHandler(this._componentUpdatedHandler.bind(this));
    };

    var l = posture.prototype;

    l._createFunctionWrappers = function (){
        this.updateEntityHeadYaw = _fivesCommunicator.connection.generateFuncWrapper("posture.updateHeadYaw");
    };

    l._componentUpdatedHandler = function(entity, componentName, attributeName) {
        if(componentName == "nao_posture")
        {
			console.log("componentUpdatedHandler");
			
			// TODO: Update Mesh here!
			
                // FIVES.Resources.SceneManager.applyPositionToXML3DView(entity);
                // IVES.Resources.SceneManager.applyOrientationToXML3DView(entity);
        }
    };

    l.updateHeadYaw = function(entity, headYaw) {
		console.log("updateHeadYaw");
        //entity.posture.headYaw = headYaw;
        //FIVES.Resources.SceneManager.applyPositionToXML3DView(entity);
    };

    l.setEntityHeadYaw = function(entity, headYaw) {
        this.sendEntityHeadYawUpdate(entity.guid, headYaw);
    };

    l.sendEntityHeadYawUpdate = function(guid, headYaw) {
		console.log("sendEntityHeadYawUpdate");
        //this.updateEntityPosition(guid, position, _fivesCommunicator.generateTimestamp());
    };

    FIVES.Plugins.Posture = new posture();

}());