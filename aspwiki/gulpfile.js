"use strict";

var project = require('./project.json');

var gulp = require("gulp"),
    concat = require("gulp-concat"),
    less = require("gulp-less"),
    cssmin = require('gulp-cssmin'),
    jsmin = require('gulp-minify'),
    rename = require('gulp-rename');

gulp.watch('Styles/*.less', ["less"]);

gulp.task("less", function () {
    return gulp.src('Styles/**/*.less')
        .pipe(concat('main.css'))
        .pipe(less())
        .pipe(gulp.dest(project.webroot + '/css'));
});


gulp.task('minifycss', function () {
    return gulp.src(project.webroot + '/css/main.css')
        .pipe(cssmin())
        .pipe(rename({ suffix: '.min' }))
        .pipe(gulp.dest(project.webroot + '/css'));
});

gulp.task('minifyjs', function () {
    return gulp.src(project.webroot + '/js/*.js')
        .pipe(jsmin())
        .pipe(gulp.dest(project.webroot + '/js'));
});