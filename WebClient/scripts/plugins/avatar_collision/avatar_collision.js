/**
 * Created with JetBrains WebStorm.
 * Author: Torsten Spieldenner
 * Date: 2/19/14
 * Time: 9:23 AM
 * (c) DFKI 2013
 * http://www.dfki.de
 */

var FIVES = FIVES || {};
FIVES.Plugins = FIVES.Plugins || {};

(function () {
    "use strict";

    var _xml3dElement;
    var _fivesCommunicator = FIVES.Communication.FivesCommunicator;

    var avatarCollision = function () {
        _xml3dElement = $("xml3d")[0];
        FIVES.Events.AddConnectionEstablishedHandler(this._createFunctionWrappers.bind(this));
        FIVES.Events.AddOnComponentUpdatedHandler(this._handleEntityPositionUpdate(this));
    };

    var a = avatarCollision.prototype;

    a._createFunctionWrappers = function() {
        this.setAvatarGroundlevel = _fivesCommunicator.connection.generateFuncWrapper("gravity.setGroundlevel");
    };

    a._handleEntityPositionUpdate = function(entity) {
        if(entity.guid == FIVES.AvatarEntityGuid)
        {
            this.putMeshOnGround(entity);
            if(entity.velocity)
                this.stopMotionOnCollision(entity);
        }
    };

    a.putMeshOnGround = function(entity) {
        var hitpointWithGround = this.getHitpointWithGround(entity);
        if(hitpointWithGround && !isNaN(hitpointWithGround.y))
        {
            this.setAvatarGroundlevel(FIVES.AvatarEntityGuid, hitpointWithGround.y);
        }
    };

    a.stopMotionOnCollision = function(entity) {
        var rayOrigin = getCollisionRayOrigin(entity);
        var view = $(_xml3dElement.activeView)[0];
        var entityDirection = entity.xml3dView.transformElement.rotation.rotateVec3(new XML3DVec3(1,0,0));
        if(entity.velocity.x < 0)
            entityDirection = entityDirection.negate();

        var ray = new XML3DRay(rayOrigin, entityDirection);

        var outHitpoint = new XML3DVec3(0,0,0);
        _xml3dElement.getElementByRay(ray, outHitpoint);
        if(outHitpoint.x && !isNaN(outHitpoint.x)
            && outHitpoint.z && !isNaN(outHitpoint.z))
        {
            var entityPositionInPlane = new XML3DVec3(entity.position.x, 0, entity.position.z);
            var hitpointInPlane = new XML3DVec3(outHitpoint.x, 0, outHitpoint.z);
            if(entityPositionInPlane.subtract(hitpointInPlane).length() < 2.5)
            {
                FIVES.Communication.FivesCommunicator.setAvatarForwardBackwardMotion(0);
            }
        }
    };

    a.getHitpointWithGround = function(entity) {
        var rayOrigin = getCollisionRayOrigin(entity);
        var ray = new XML3DRay(rayOrigin, new XML3DVec3(0,-1,0));
        var outHitpoint = new XML3DVec3(0,0,0);
        _xml3dElement.getElementByRay(ray, outHitpoint);
        return outHitpoint;
    };

    var getCollisionRayOrigin = function(entity) {
        var boundingBox = entity.xml3dView.groupElement.getBoundingBox();
        var center = boundingBox.center();
        var view = $(_xml3dElement.activeView)[0];
        var viewDirection = view.getDirection();
        var rayOrigin = new XML3DVec3(center.x + viewDirection.x * 0.5, boundingBox.max.y, center.z + viewDirection.z * 0.5);
        return rayOrigin;
    };

}());

