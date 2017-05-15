//tripEditorController.js
(function () {

	"use strict";

	angular.module("tripsApp")
		.controller("tripEditorController", tripEditorController);

	function tripEditorController($routeParams, $http) {
		const api = "/api/trips/";
		let vm = this;

		vm.tripName = $routeParams.tripName;
		vm.stops = [];
		vm.errorMessage = "";
		vm.isBusy = false;
		vm.newStop = {};

		$http.get(api + vm.tripName + "/stops")
			.then(function (response) {
				angular.copy(response.data, vm.stops);
				_showMap(vm.stops);
			}, function (error) {
				vm.errorMessage = error;
			}).finally(function () {
				vm.isBusy = false;
			});

		vm.addStop = function () {
			vm.isBusy = true;
			$http.post(api + vm.tripName + "/stops", vm.newStop)
				.then(function (response) {
					vm.stops.push(response.data);
					_showMap(vm.stops);
					vm.newstop = {};
				}, function (error) {
					vm.errorMessage = error;
				}).finally(function () {
					vm.isBusy = false;
				});
		};

	}

	function _showMap(stops) {
		if (stops && stops.length > 0) {
			debugger;
			let mapStops = _.map(stops, function (item) {
				return {
					lat: item.latitude,
					long: item.longitude,
					info: item.name
				}
			});

			//Show Map
			travelMap.createMap({
				stops: stops,
				selector: "#map",
				currentStop: 1,
				initialZoom: 3
			})
		}
	}

})();