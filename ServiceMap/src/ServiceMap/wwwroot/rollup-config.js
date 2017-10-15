import nodeResolve from 'rollup-plugin-node-resolve';
import commonjs    from 'rollup-plugin-commonjs';
import uglify      from 'rollup-plugin-uglify';

export default {
entry: 'app/main.js',
dest: 'aot/build.min.js', // output a single application bundle
sourceMap: false,
format: 'umd',
onwarn: function(warning) {
    // Skip certain warnings

    // should intercept ... but doesn't in some rollup versions
    if ( warning.code === 'THIS_IS_UNDEFINED' ) { return; }

    // console.warn everything else
    console.warn( warning.message );
},
plugins: [
    nodeResolve({jsnext: true, module: true}),
    commonjs({
        include: [ 
                'node_modules/rxjs/**',
                'node_modules/ng2-toastr/**',
                'node_modules/primeng/**',
                'node_modules/angular2-busy/**'
        ],
        namedExports: {
            'node_modules/angular2-busy/build/index.js': [ 'BusyModule','BusyConfig','BUSY_CONFIG_DEFAULTS' ],
            'node_modules/primeng/primeng.js': [
                    'PanelModule',
                    'Inputtext',
                    'InputSwitchModule',
                    'InputMaskModule',
                    'ProgressBarModule',
                    'DropdownModule',
                    'CalendarModule',
                    'InputTextModule',
                    'DataTableModule',
                    'DataListModule',
                    'ButtonModule',
                    'DialogModule',
                    'AccordionModule',
                    'RadioButtonModule',
                    'ToggleButtonModule',
                    'CheckboxModule',
                    'SplitButtonModule',
                    'ToolbarModule',
                    'SelectButtonModule',
                    'OverlayPanelModule',
                    'TieredMenuModule',
                    'GrowlModule',
                    'ChartModule',
                    'Checkbox',
                    'Dropdown',
                    'Calendar',
                    'DataGridModule',
                    'DataTable',
                    'MultiSelectModule',
                    'TooltipModule',
                    'FileUploadModule',
                    'TabViewModule',
                    'AutoCompleteModule',
                    'SharedModule',
                    'MultiSelect'
            ]
        }
    }),
    uglify()
    ]
}