/// <binding BeforeBuild='clean' AfterBuild='css-task, templates-task, scripts-task, vendors-task, spa-task, encoding-task, resources-task' Clean='clean' />
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
var templateCache = require('gulp-angular-templatecache');

var appName = 'tannerPrivadoApp';

var paths = {
    buildFolder: '.build',
    index: './Views/Home/Index.cshtml',
    homeFolder: './Views/Home',
    domainFiles: ['./app/domain/IEntity.js', './app/domain/IInput.js', './app/domain/*.js'],
    appFiles: ['./app/*.js',
                './app/common/directives/*.js',
                './app/common/services/*.js',
                './app/common/controllers/*.js',
                './app/common/typings/*.js',
                './app/modules/**/*.js'],
    stylesCss: ['./Styles/*.css'],
    stylesLess: ['./Styles/*.less'],
    scripts: ['./Scripts/extras/jquery.sticky.js','./Scripts/extras/*.js'],
    bower_components: ['./bower_components/angular-bootstrap/ui-bootstrap-tpls.min.js',
                        './bower_components/angular-i18n/angular-locale_es-cl.js',
                        './bower_components/angular-route/angular-route.js',
                        './bower_components/angular/angular.js',
                        './bower_components/jquery/dist/jquery.js'],
    images: ['./Resources/img/*'],
    fonts: ['./Resources/fonts/*'],
    htmls: ['./app/**/*.html'],
    templates: ['./app/common/templates/pagination.html']
};

gulp.task('clean', function () {
    return del([paths.buildFolder]);
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
                    .pipe(gulp.dest(paths.buildFolder+'/spa')), { name: 'domain' }))
            .pipe(gulp.dest(paths.homeFolder))
            .pipe(inject(appStream
                    .pipe(print())
                    .pipe(concat('app.js'))
                    .pipe(uglify())
                    .pipe(gulp.dest(paths.buildFolder+'/spa')), { name: 'app' }))
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
                            .pipe(gulp.dest(paths.buildFolder+'/vendors')), { name: 'vendors' }))
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
                .pipe(gulp.dest(paths.buildFolder+'/css')), { name: 'stylesCss' })
                )
            .pipe(gulp.dest(paths.homeFolder))
            .pipe(inject(
                customLessStream.pipe(print())
                .pipe(concat('stylesLess.css'))
                .pipe(less())
                .pipe(gulp.dest(paths.buildFolder+'/css')), { name: 'stylesLess' })
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
                .pipe(gulp.dest(paths.buildFolder+'/js')), { name: 'extras' })
                )
            .pipe(gulp.dest(paths.homeFolder));
});

gulp.task('resources-task', function () {

    var imagesStream = gulp.src(paths.images);
    var fontsStream = gulp.src(paths.fonts);
    var htmlsStream = gulp.src(paths.htmls);

    var images = imagesStream.pipe(print())
                    .pipe(gulp.dest(paths.buildFolder+'/img'));
    var fonts = fontsStream.pipe(print())
                .pipe(gulp.dest(paths.buildFolder+'/fonts'));
    var htmls = htmlsStream.pipe(print())
                .pipe(gulp.dest(paths.buildFolder+'/html'));

    return merge(images, fonts, htmls);
});

gulp.task('templates-task', function () {
    var target = gulp.src(paths.index);

    var templatesStream = gulp.src(paths.templates);

    return target
            .pipe(inject(
                templatesStream.pipe(print())
                .pipe(templateCache('templates.js', { module: appName }))
                .pipe(gulp.dest(paths.buildFolder + '/js')), { name: 'templates' })
                )
            .pipe(gulp.dest(paths.homeFolder));
});

gulp.task('encoding-task', function () {
    var target = gulp.src(paths.index);

    return target
            .pipe(header('\ufeff'))
            .pipe(gulp.dest(paths.homeFolder));
});
