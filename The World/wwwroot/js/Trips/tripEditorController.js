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
		vm.newStop = {};
		vm.newStop.arrival = new Date().toLocaleDateString();
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
			vm.errorMessage = "";
			if (Object.prototype.toString.call(vm.newStop.arrival) === "[object Date]") {
				if (isNaN(vm.newStop.arrival.getTime())) {
					vm.errorMessage = "Invalid Date";
				}
			}

			if (vm.errorMessage.length > 0)
				return;

			$http.post(api + vm.tripName + "/stops", vm.newStop)
				.then(function (response) {
					vm.stops.push(response.data);
					_showMap(vm.stops);
					clear();
				}, function (error) {
					vm.errorMessage = error;
				}).finally(function () {
					vm.isBusy = false;
				});
		};

		function clear() {
			vm.newStop = {};
			vm.newStop.arrival = new Date().toLocaleDateString();
		};
	}

	function _showMap(stops) {
		if (stops && stops.length > 0) {
			let mapStops = _.map(stops, function (item) {
				return {
					lat:  item.latitude,
					long: item.longitude,
					info: item.name
				}
			});

			//Show Map
			travelMap.createMap({
				stops: mapStops,
				selector: "#map",
				currentStop: 1,
				initialZoom: 3
			})
		}
	}
})();