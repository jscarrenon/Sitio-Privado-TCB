/// <binding AfterBuild='build' Clean='clean' ProjectOpened='watch' />
/*
This file in the main entry point for defining Gulp tasks and using Gulp plugins.
Click here to learn more. http://go.microsoft.com/fwlink/?LinkId=518007
*/

var gulp = require('gulp'),
jshint = require('gulp-jshint'),
del = require('del'),
debug = require('gulp-debug'),
filter = require('gulp-filter'),
sass = require('gulp-sass'),
sourcemaps = require('gulp-sourcemaps'),
ngHtml2Js = require('gulp-ng-html2js'),
concat = require("gulp-concat"),
watch = require('gulp-watch'),
batch = require('gulp-batch'),
plumber = require('gulp-plumber'),

paths = {
    app: {
        src: './Frontend/app/**/*',
        dest: './Content/app/'
    },
    styles: {
        src: './Frontend/sass/**/*',
        dest: './Content/css/'
    },
    templates: {
        src: './Frontend/templates/**/*',
        dest: './Content/templates/'
    },
    statics: {
        src: './Frontend/img/**/*',
        dest: './Content/img/'
    }
};

var apps = ['login', 'user', 'admin'];

gulp.task('clean-js', function (done) {
    del([paths.app.dest + '/**'], done);    // Delete everything
});

gulp.task('clean-css', function (done) {
    del([paths.styles.dest + '/**'], done);    // Delete everything
});

gulp.task('clean-templates', function (done) {
    del([paths.templates.dest + '/**'], done);    // Delete everything
});

gulp.task('clean-statics', function (done) {
    del([paths.statics.dest + '/**'], done);    // Delete everything
});

gulp.task('clean', ['clean-css', 'clean-js', 'clean-templates', 'clean-statics']);

gulp.task('build-js', ['clean-js'], function () {
    var jsFilter = filter(['**/*.js'], { restore: true });

    return gulp.src(paths.app.src)
        .pipe(jsFilter)
        .pipe(jshint())
        .pipe(jshint.reporter('jshint-stylish'))
        .pipe(jshint.reporter('fail'))
        .pipe(jsFilter.restore)

        .pipe(gulp.dest(paths.app.dest));
});

gulp.task('build-css', ['clean-css'], function () {
    return gulp.src(paths.styles.src)
        .pipe(sourcemaps.init())
        .pipe(sass())
        .pipe(sourcemaps.write('maps'))
        .pipe(gulp.dest(paths.styles.dest));
});

gulp.task('build-templates', ['clean-templates'], function () {
    var htmlFilter = filter(['**/*.html'], { restore: true });
    var templatesPipe = gulp.src(paths.templates.src)
                        .pipe(htmlFilter);

    for (var i = 0; i < apps.length; i++) {
        var appFilter = filter([apps[i] + '/**/*'], { restore: true });

        templatesPipe
            .pipe(appFilter)
            .pipe(ngHtml2Js({
                moduleName: function (file) {
                    var pathParts = file.path.split(/[\\\/]+/);
                    var app = pathParts[pathParts.length - 3];
                    var module = pathParts[pathParts.length - 2];

                    return app + '.' + module;
                },
                prefix: '/Content/templates/',
                declareModule: false
            }))
            .pipe(concat(apps[i] + '/templates.js'))
            .pipe(appFilter.restore);
    }

    return templatesPipe
        .pipe(htmlFilter.restore)
        .pipe(gulp.dest(paths.templates.dest));
});

gulp.task('build-statics', ['clean-statics'], function () {
    return gulp.src(paths.statics.src)
        .pipe(gulp.dest(paths.statics.dest));
});

gulp.task('build', ['bower', 'build-js', 'build-css', 'build-templates', 'build-statics']);

gulp.task('watch-js', function () {
    gulp.src(paths.app.src)
    .pipe(plumber())
    .pipe(watch(paths.app.src, batch(function (events, done) {
        gulp.start('build-js', done);
    })))
    ;
});

gulp.task('watch-css', function () {
    gulp.src(paths.styles.src)
    .pipe(plumber())
    .pipe(watch(paths.styles.src, batch(function (events, done) {
        gulp.start('build-css', done);
    })))
    ;
});

gulp.task('watch-templates', function () {
    gulp.src(paths.templates.src)
    .pipe(plumber())
    .pipe(watch(paths.templates.src, batch(function (events, done) {
        gulp.start('build-templates', done);
    })))
    ;
});

gulp.task('watch-statics', function () {
    gulp.src(paths.statics.src)
    .pipe(plumber())
    .pipe(watch(paths.statics.src, batch(function (events, done) {
        gulp.start('build-statics', done);
    })));
});

gulp.task('watch', ['watch-templates', 'watch-js', 'watch-css', 'watch-statics']);

gulp.task('bower', function () {

    var install = require("gulp-install");

    return gulp.src(['./bower.json'])
        .pipe(install());
});

