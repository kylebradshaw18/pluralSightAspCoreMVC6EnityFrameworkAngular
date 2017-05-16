//tripsController.js
(function () {

	"use strict";

	angular.module("tripsApp")
		.controller("tripsController", tripsController);

	function tripsController($http) {
		const api = "/api/trips";
		var vm = this;
		vm.trips = [];

		vm.newTrip = {};
		vm.errorMessage = "";
		vm.busy = true;

		$http.get(api).then(function (response) {
			//Success
			angular.copy(response.data, vm.trips);
		}, function (error) {
			//Failure
			vm.errorMessage = "Failed to load data" + error;
		}).finally(function () {
			vm.busy = false;
		});

		vm.addTrip = function () {
			vm.busy = true;
			vm.errorMessage = "";
			$http.post(api, vm.newTrip).then(function (response) {
				//success
				vm.trips.push(response.data);

			}, function (error) {
				//Failure
				vm.errorMessage = "Failed to save new trip";
			}).finally(function () {
				vm.busy = false;
				vm.newTrip = {};
			});
		};
	}

})();