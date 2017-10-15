import { Component, OnInit, Input } from '@angular/core';
import { IDepotDetails } from './depotDetails';

@Component({
    selector: 'depot-details',
    templateUrl: './depot-detail.component.html'
})

export class ServiceTntDetailComponent implements OnInit {
    pageTitle: string = 'Dane Oddziału';
    @Input() depotsTnt: IDepotDetails[];

  
    ngOnInit(): void
    {
        //let id = +this._route.snapshot.params['id'];
        //this.pageTitle += `:${id}`;
    }
}