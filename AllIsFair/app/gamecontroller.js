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
        function ResultsShow() {
            if ($scope.game.Result != null) {
                $("#resultsModal").modal();
            }
        }
        function DeathState() {
            if ($scope.game.GameOverInfo) {
                console.log($scope.game.GameOverInfo);
                $("#gameOverModal").modal();
            }
        }
        function refreshGameState() {
            $http.get("/games/getgamestate").then(function (res) {
                $scope.game = res.data;
                DeathState();
            });
        }

        $scope.tileClick = function (tile) {
            if (!tile.IsPossibleMove) {
                return;
            }
            console.log(tile);
            $http.post("/games/trymove", { x2: tile.X, y2: tile.Y }).then(function (res) {
                console.log(res.data);
                $scope.game = res.data;
                DeathState();
                ResultsShow();
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
