const gulp = require('gulp');
const fs = require('fs');
const gutil = require('gulp-util');
const msbuild = require('gulp-msbuild');
const foreach = require('gulp-foreach');
const debug = require('gulp-debug');
const argv = require('yargs').argv;
const dotnet = require('gulp-dotnet-cli');

var config;
if (fs.existsSync("./gulp-config.js.user")) {
    config = require("./gulp-config.js.user")();
} else {
    config = require("./gulp-config.js")();
}

const solutionPath = 'D:\\K14\\2022-2023\\dot net\\project\\sitecorepro3\\sitecorepro3.sln';
const publishProfile = 'C:\\inetpub\\wwwroot\\sc102sc.dev.local';

gulp.task('build', function () {
    return gulp.src(solutionPath)
        .pipe(msbuild({
            targets: ['Clean', 'Build'],
            configuration: argv.configuration || 'Debug',
            logCommand: true,
            verbosity: 'minimal',
            toolsVersion: "current"
        }));
});

gulp.task('publish', function () {
    return gulp.src(publishProfile)
        .pipe(foreach(function (stream, file) {
            gutil.log('Publishing ' + file.path);
            return stream
                .pipe(debug());
        }));
});
gulp.task('publishAll', function () {
    return gulp.src(solutionPath)
        .pipe(msbuild({
            targets: ['Clean', 'Build', 'Publish'],
            configuration: 'Debug',
            stdout: true,
            verbosity: 'minimal',
            toolsVersion: 'current',
            properties: {
                DeployOnBuild: 'true',
                DeployDefaultTarget: 'WebPublish',
                WebPublishMethod: 'FileSystem',
                DeleteExistingFiles: 'false',
                publishUrl: publishProfile
            }
        }));
});
gulp.task('default', gulp.series('build', 'publish'));