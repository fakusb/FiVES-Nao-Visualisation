// This file is part of FiVES.
//
// FiVES is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// FiVES is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with FiVES.  If not, see <http://www.gnu.org/licenses/>.


require.config({
    paths: {
        'jquery' : 'lib/jquery',
        'kiara' : 'lib/kiara',
        'websocket-json' : 'lib/websocket-json'
    }
});

requirejs(['kiara', 'jquery', 'websocket-json', 'plugins/testing/testing'],
function(KIARA, $) {

    function setScript(guid) {
        //var script = promptString("serverScript = ");
        var script = "console.log('hello from client');";
        createServerScriptFor(guid, script);
        return false;
    }

    function ReportError(message) {
        // Show error message.
        alert("SignIn failed!\n"+message);
    }

    var loginComplete = false;
    var signinBtnPressed = false;
    function login() {
        // This is to allow hiding modal when login is complete.
        if (loginComplete)
            return true;

        var login = "norbert";
        var password = "norbert";

        var connectCallback = function(success, message) {
            if (success) {
                loginComplete = true;
            } else {
                ReportError(message);
            }
        };

        var authCallback = function(success, message) {
            if (success) {
                FIVES.Communication.FivesCommunicator.connect(connectCallback);
            } else {
                ReportError(message);
            }
        };

        FIVES.Communication.FivesCommunicator.auth(login, password, authCallback);

        return false;
    }

    function main() {
        var context = KIARA.createContext();
        var service = "kiara/fives.json";

        FIVES.Communication.FivesCommunicator.initialize(context, service);
        FIVES.Resources.SceneManager.initialize("xml3dView");

        login();
    }
    $(document).ready(main);
});
