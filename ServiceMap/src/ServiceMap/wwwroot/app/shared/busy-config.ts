import { BusyConfig } from 'angular2-busy';

export function BusyConfigFactory(): BusyConfig {
    return new BusyConfig({
        delay: 0,
        minDuration: 0,
        backdrop: true,
        message: 'Proszę czekać...',
    });
}