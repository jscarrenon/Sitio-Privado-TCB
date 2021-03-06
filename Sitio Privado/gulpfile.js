/// <binding BeforeBuild='clean' AfterBuild='css-task, templates-task, scripts-task, better-dom-task, vendors-task, spa-task, encoding-task, resources-task, login-task' Clean='clean' />
/*
This file in the main entry point for defining Gulp tasks and using Gulp plugins.
Click here to learn more. http://go.microsoft.com/fwlink/?LinkId=518007
*/

var argv = require('yargs').argv;
var gulp = require('gulp');
var runSequence = require('run-sequence');
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
var util = require('gulp-util');

var appName = 'tannerPrivadoApp';

var paths = {
    config: './config/',
    buildFolder: '.build',
    index: './Views/Home/Index.cshtml',
    login: {
        src: './Views/Account/SignInExternal.cshtml',
        stylesLess: ['./Styles/login.less'],
        scripts: ['./Scripts/extras/login.js'],
        folder: './Views/Account'
    },
    homeFolder: './Views/Home',
    domainFiles: ['./app/domain/IEntity.js', './app/domain/IInput.js', './app/domain/*.js'],
    appFiles: ['./app/*.js',
                './app/config/*.js',
                './app/common/directives/*.js',
                './app/common/services/*.js',
                './app/common/controllers/*.js',
                './app/common/typings/*.js',
                './app/modules/**/*.js'],
    stylesCss: ['./Styles/*.css'],
    stylesLess: ['./Styles/*.less', '!./Styles/login.less'],
    scripts: ['./Scripts/extras/jquery.sticky.js', './Scripts/extras/*.js', '!./Scripts/extras/jquery-1.12.0.js', '!./Scripts/extras/login.js'],
    bower_components: ['./bower_components/angular-google-analytics/dist/angular-google-analytics.js',
                        './bower_components/angular-localforage/dist/angular-localForage.min.js',
                        './bower_components/localforage/dist/localforage.min.js',
                        './bower_components/angular-rut/dist/angular-rut.min.js',
                        './bower_components/angular-bootstrap/ui-bootstrap-tpls.min.js',
                        './bower_components/angular-i18n/angular-locale_es-cl.js',
                        './bower_components/angular-route/angular-route.js',
                        './bower_components/angular/angular.js',
                        './bower_components/jquery/dist/jquery.js'
                        ],
    better_dom: ['./bower_components/better-dom/dist/better-dom.js',
                    './bower_components/better-i18n-plugin/dist/better-i18n-plugin.js',
                    './bower_components/better-dateinput-polyfill/dist/better-dateinput-polyfill.js',
                    './bower_components/better-dateinput-polyfill/i18n/better-dateinput-polyfill.es.js'],
    better_dom_legacy: ['./bower_components/better-dom/dist/better-dom-legacy.js',
                        './bower_components/better-dom/dist/better-dom-legacy.htc'],
    images: ['./Resources/img/*'],
    fonts: ['./Resources/fonts/*'],
    htmls: ['./app/**/*.html'],
    templates: ['./app/common/templates/pagination.html']
};

gulp.task('default', function (callback) {
    runSequence('clean', 'css-task', 'templates-task', 'scripts-task', 'better-dom-task', 'vendors-task', 'spa-task', 'encoding-task', 'resources-task', 'login-task', callback);
});

// This task should not be used because it depends on execution arguments (argv) and the BeforeBuild is not configured to support them
gulp.task('config-task', function () {
    var configEnv = argv.c || argv.config || 'local';

    printLog('Copying files for environment: ' + configEnv);
    return gulp.src(paths.config + configEnv + '/**/*')
        .pipe(gulp.dest('./app/config/'));
});

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

gulp.task('better-dom-task', function () {
    var target = gulp.src(paths.index);

    var betterDomStream = gulp.src(paths.better_dom);
    var betterDomLegacyJsStream = gulp.src(paths.better_dom_legacy[0]);
    var betterDomLegacyHtcStream = gulp.src(paths.better_dom_legacy[1]);

    var betterDom = target
                        .pipe(inject(
                            betterDomStream.pipe(print())
                                        .pipe(gulp.dest(paths.buildFolder + '/js')), { name: 'better-dom' }))
                        .pipe(gulp.dest(paths.homeFolder));
    var betterDomLegacyJs = target
            .pipe(inject(
                    betterDomLegacyJsStream.pipe(print())
                                        .pipe(gulp.dest(paths.buildFolder + '/js')), { starttag: '<!--[if IE]>', endtag: '<![endif]-->' }))
            .pipe(gulp.dest(paths.homeFolder));
    var betterDomLegacyHtc = betterDomLegacyHtcStream.pipe(print())
                                        .pipe(gulp.dest(paths.buildFolder + '/js'));

    return merge(betterDom, betterDomLegacyJs, betterDomLegacyHtc);
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

gulp.task('login-task', function () {
    var target = gulp.src(paths.login.src);

    var customLessStream = gulp.src(paths.login.stylesLess);
    var css =  target
            .pipe(inject(
                customLessStream.pipe(print())
                .pipe(concat('stylesLoginLess.css'))
                .pipe(less())
                .pipe(gulp.dest(paths.buildFolder+'/css')), { name: 'stylesLess' })
                )
            .pipe(gulp.dest(paths.login.folder));

    var extrasJsStream = gulp.src(paths.login.scripts);
    var scripts = target
            .pipe(inject(
                extrasJsStream.pipe(print())
                .pipe(concat('extrasLogin.js'))
                .pipe(gulp.dest(paths.buildFolder + '/js')), { name: 'extras' })
                )
            .pipe(gulp.dest(paths.login.folder));

    var encoding = target
            .pipe(header('\ufeff'))
            .pipe(gulp.dest(paths.login.folder));

    return merge(css, scripts, encoding);
});

function printLog(msg) { return util.log(util.colors.cyan(msg)); }
function printError(msg) { return util.log(util.colors.red(msg)); }
