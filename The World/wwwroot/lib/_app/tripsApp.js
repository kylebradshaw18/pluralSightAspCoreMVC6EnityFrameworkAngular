!function(){"use strict";angular.module("tripsApp",["simpleControls","ngRoute"]).config(["$routeProvider",function(r){r.when("/",{controller:"tripsController",controllerAs:"vm",templateUrl:"/views/tripsView.html"}),r.when("/editor/:tripName",{controller:"tripEditorController",controllerAs:"vm",templateUrl:"/views/tripEditorView.html"}),r.otherwise({redirectTo:"/"})}])}();