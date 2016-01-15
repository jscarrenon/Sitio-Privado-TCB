/// <binding BeforeBuild='clean' AfterBuild='css-task, scripts-task, vendors-task, spa-task, encoding-task, resources-task' Clean='clean' />
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
var less = require('gulp-less');
var merge = require('merge-stream');
var del = require('del');

var paths = {
    index: './Views/Home/Index.cshtml',
    homeFolder: './Views/Home/',
    domainFiles: ['./app/domain/*.js'],
    appFiles: ['./app/*.js', './app/common/services/*.js', './app/common/controllers/*.js', './app/common/typings/*.js', './app/home/*.js', './app/inversiones/*.js', './app/mis-inversiones/*.js'],
    stylesCss: ['./Styles/*.css'],
    stylesLess: ['./Styles/*.less'],
    scripts: ['./Scripts/extras/*.js', './Scripts/typings/slicknav/slicknav.js'],
    bower_components: ['./bower_components/angular-route/angular-route.js',
                                 './bower_components/angular/angular.js',
                                 './bower_components/jquery/dist/jquery.js'],
    images: ['./Resources/img/*'],
    fonts: ['./Resources/fonts/*']
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

    var customCssStream = gulp.src(paths.stylesCss);
    var customLessStream = gulp.src(paths.stylesLess);

    return target
            .pipe(inject(
                customCssStream.pipe(print())
                .pipe(concat('stylesCss.css'))
                .pipe(gulp.dest('.build/css')), { name: 'stylesCss' })
                )
            .pipe(gulp.dest(paths.homeFolder))
            .pipe(inject(
                customLessStream.pipe(print())
                .pipe(concat('stylesLess.css'))
                .pipe(less())
                .pipe(gulp.dest('.build/css')), { name: 'stylesLess' })
                )
            .pipe(gulp.dest(paths.homeFolder));
});

gulp.task('scripts-task', function () {
    var target = gulp.src(paths.index);

    var extrasJsStream = gulp.src(paths.scripts);

    return target
            .pipe(inject(
                extrasJsStream.pipe(print())
                .pipe(concat('extras.js'))
                .pipe(gulp.dest('.build/js')), { name: 'extras' })
                )
            .pipe(gulp.dest(paths.homeFolder));
});

gulp.task('resources-task', function () {

    var imagesStream = gulp.src(paths.images);
    var fontsStream = gulp.src(paths.fonts);

    var images = imagesStream.pipe(print())
                    .pipe(gulp.dest('.build/img'));
    var fonts = fontsStream.pipe(print())
                .pipe(gulp.dest('.build/fonts'));

    return merge(images, fonts);
});

gulp.task('encoding-task', function () {
    var target = gulp.src(paths.index);

    return target
            .pipe(header('\ufeff'))
            .pipe(gulp.dest(paths.homeFolder));
});

