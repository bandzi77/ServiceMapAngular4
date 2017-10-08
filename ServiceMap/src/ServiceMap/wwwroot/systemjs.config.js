/**
 * System configuration for Angular 2 
 * Adjust as necessary for your application needs.
 */
(function (global) {
    System.config({
        paths: {
            // paths serve as alias
            'npm:': 'node_modules/'
        },
        // map tells the System loader where to look for things
        map: {
            // our app is within the app folder
            app: 'app',

            //angular bundles
            '@angular/core': 'npm:@angular/core/bundles/core.umd.js',
            '@angular/common': 'npm:@angular/common/bundles/common.umd.js',
            '@angular/compiler': 'npm:@angular/compiler/bundles/compiler.umd.js',
            '@angular/platform-browser': 'npm:@angular/platform-browser/bundles/platform-browser.umd.js',
            '@angular/platform-browser-dynamic': 'npm:@angular/platform-browser-dynamic/bundles/platform-browser-dynamic.umd.js',
            '@angular/http': 'npm:@angular/http/bundles/http.umd.js',
            '@angular/router': 'npm:@angular/router/bundles/router.umd.js',
            '@angular/forms': 'npm:@angular/forms/bundles/forms.umd.js',

            // other libraries
            'rxjs': 'npm:rxjs',
            'moment': 'npm:moment/bundles/moment.umd.js',
            'ng2-bootstrap': 'npm:ng2-bootstrap/bundles/ng2-bootstrap.umd.js',

            'primeng':  'npm:primeng',

            // Busy Indicator
            'ts-metadata-helper': 'npm:angular2-busy/node_modules/ts-metadata-helper',
            'angular2-dynamic-component': 'npm:angular2-busy/node_modules/angular2-dynamic-component',
            'angular2-busy': 'npm:angular2-busy',
            'core-js': 'npm:core-js'
  



        },
        // packages tells the System loader how to load when no filename and/or no extension
        packages: {
            'app': {
                main: './main.js',
                defaultExtension: 'js'
            },
            'rxjs': {
                defaultExtension: 'js'
            },
            'ng2-bootstrap':
            {
                defaultExtension: 'js'
            },

            // Busy Indicator
            'ts-metadata-helper': {
                defaultExtension: 'js'
            },
            'angular2-dynamic-component': {
                defaultExtension: 'js'
            },
            'angular2-busy': {
                main: './index.js',
                defaultExtension: 'js'
            },
            'core-js': {
                main: 'index.js',
                defaultExtension: 'js'
            },
            'primeng': { defaultExtension: 'js' },

        }
    });
})(this);