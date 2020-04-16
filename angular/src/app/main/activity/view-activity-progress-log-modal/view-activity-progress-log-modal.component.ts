import { Component, OnInit, ViewChild, Injector, ChangeDetectorRef } from '@angular/core';
import { PerformanceActivitiesServiceProxy, ActivityProgressLogDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ModalDirective } from 'ngx-bootstrap';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';

@Component({
    selector: 'app-view-activity-progress-log-modal',
    templateUrl: './view-activity-progress-log-modal.component.html',
    styleUrls: ['./view-activity-progress-log-modal.component.css']
})
export class ViewActivityProgressLogModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    active = false;

    activityLog: ActivityProgressLogDto[] = new Array();

    constructor(
        injector: Injector,
        private _performanceActivitiesServiceProxy: PerformanceActivitiesServiceProxy,
        private _changeDetector: ChangeDetectorRef
    ) {
        super(injector);
    }

    show(activityId?: number): void {
        if (activityId) {
            this._performanceActivitiesServiceProxy.getTargetUpdateLog(activityId).subscribe(result => {
                this.activityLog = result;

                this.active = true;
                this.modal.show();
                this._changeDetector.detectChanges();
            });
        } else {
            this.notify.error('The selected target is not valid!');
        }
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

}
