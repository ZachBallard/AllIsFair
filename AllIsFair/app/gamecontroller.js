(function () {
    'use strict';

    angular
        .module('allsFair')
        .controller('gamecontroller', gamecontroller);

    gamecontroller.$inject = ['$scope', '$http']; 

    function gamecontroller($scope, $http) {
        $scope.title = 'gamecontroller';

        activate();

        function activate() {
            refreshGameState();
        }

        function refreshGameState() {
            $http.get("/games/getgamestate").then(function (res) {
                console.log(res.data);
                $scope.game = res.data;
            });
        }

        $scope.tileClick = function (tile) {
            if (!tile.IsPossibleMove) {
                return;
            }
            console.log(tile);
            $http.post("/games/trymove", { x2: tile.X, y2: tile.Y }).then(function(eventType, res) {
                console.log(res.data);
                refreshGameState();
                if (eventType === 0) {
                    $http.post("/games/DrawEvent"), { eventType }.then(function() {
                        //handle event result screen
                        //call opponent action
                    });
                } else {
                    //we know they attacked
                    $http.post("/games/drawevent"), { eventType }.then(function() {
                        //call oppents action
                    });
                    
                }
                
            });

        }

        //function DrawCard(type) {
        //    $.ajax({
        //        type: "POST",
        //        data: { type: type },
        //        url: '@Url.Action("DrawEvent", "Games")'
        //    });
        //}
    }
})();
