/// <binding AfterBuild='minify' />
/*
This file is the main entry point for defining Gulp tasks and using Gulp plugins.
Click here to learn more. https://go.microsoft.com/fwlink/?LinkId=518007
*/

var gulp = require('gulp');
var uglify = require('gulp-uglify');
var ngAnnotate = require('gulp-ng-annotate');

gulp.task('minify', function () {
	return gulp.src("wwwroot/js/*.js")
		//For Angular special gulp files
		.pipe(ngAnnotate())
		.pipe(uglify().on('error', function (e) {
			console.log(e);
		}))
		.pipe(gulp.dest("wwwroot/lib/_app"));
});