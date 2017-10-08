import { PipeTransform, Pipe } from '@angular/core';
import { IServiceTnt } from './serviceTnt';

@Pipe({
    name: 'productFilter'
})

export class ServicesTntFilterPipe implements PipeTransform {
    transform(value: IServiceTnt[], filterBy: string): IServiceTnt[] {
        filterBy = filterBy ? filterBy.toLocaleLowerCase() : null;
        return filterBy ? value.filter((serviceTnt: IServiceTnt) =>
            serviceTnt.town.toLocaleLowerCase().indexOf(filterBy) !== -1) : value;
    }
}

