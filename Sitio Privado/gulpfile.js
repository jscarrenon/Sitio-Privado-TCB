/// <binding BeforeBuild='clean' AfterBuild='css-task, vendors-task, spa-task, encoding-task' Clean='clean' />
/*
This file in the main entry point for defining Gulp tasks and using Gulp plugins.
Click here to learn more. http://go.microsoft.com/fwlink/?LinkId=518007
*/

var gulp = require('gulp');
var inject = require('gulp-inject');
var concat = require('gulp-concat');
var print = require('gulp-print');
var angularFilesort = require('gulp-angular-filesort');
var uglify = require('gulp-uglify');
var header = require('gulp-header');
var del = require('del');

var paths = {
    index: './Views/Home/Index.cshtml',
    domainFiles: ['./app/domain/*.js'],
    appFiles: ['./app/*.js', './app/posts/*.js', './app/common/services/*.js'],
    styles: ['./Styles/site.css'],
    bower_components: ['./bower_components/angular-route/angular-route.js',
                                 './bower_components/angular/angular.js',
                                 './bower_components/jquery/dist/jquery.js'],
    homeFolder: './Views/Home/'
};

gulp.task('clean', function () {
    return del(['.build']);
});

gulp.task('spa-task', function () {
    var target = gulp.src(paths.index);

    var appDomainStream = gulp.src(paths.domainFiles);
    var appStream = gulp.src(paths.appFiles);

    return target
            .pipe(inject(appDomainStream
                    .pipe(print())
                    .pipe(concat('domain.js'))
                    .pipe(uglify())
                    .pipe(gulp.dest('.build/spa')), { name: 'domain' }))
            .pipe(gulp.dest(paths.homeFolder))
            .pipe(inject(appStream
                    .pipe(print())
                    .pipe(concat('app.js'))
                    .pipe(uglify())
                    .pipe(gulp.dest('.build/spa')), { name: 'app' }))
            .pipe(gulp.dest(paths.homeFolder))
});

gulp.task('vendors-task', function () {
    var target = gulp.src(paths.index);

    var vendorStream = gulp.src(paths.bower_components);

    return target
            .pipe(inject(
                vendorStream.pipe(print())
                            .pipe(angularFilesort()) // comment out and the application will break
                            .pipe(concat('vendors.js'))
                            .pipe(gulp.dest('.build/vendors')), { name: 'vendors' }))
            .pipe(gulp.dest(paths.homeFolder));
});

gulp.task('css-task', function () {
    var target = gulp.src(paths.index);

    var customCssStream = gulp.src(paths.styles);

    return target
            .pipe(inject(
                customCssStream.pipe(print())
                .pipe(concat('appStyles.css'))
                .pipe(gulp.dest('.build/css')), { name: 'styles' })
                )
            .pipe(gulp.dest(paths.homeFolder));
});

gulp.task('encoding-task', function () {
    var target = gulp.src(paths.index);

    return target
            .pipe(header('\ufeff'))
            .pipe(gulp.dest(paths.homeFolder));
});

