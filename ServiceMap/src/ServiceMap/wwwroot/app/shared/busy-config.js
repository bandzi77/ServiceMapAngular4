import { BusyConfig } from 'angular2-busy';
export function BusyConfigFactory() {
    return new BusyConfig({
        delay: 0,
        minDuration: 0,
        backdrop: true,
        message: 'Proszę czekać...',
    });
}
//# sourceMappingURL=busy-config.js.map