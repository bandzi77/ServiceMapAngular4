"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var angular2_busy_1 = require("angular2-busy");
function BusyConfigFactory() {
    return new angular2_busy_1.BusyConfig({
        delay: 0,
        minDuration: 0,
        backdrop: true,
        message: 'Proszę czekać...',
    });
}
exports.BusyConfigFactory = BusyConfigFactory;
//# sourceMappingURL=busy-config.js.map