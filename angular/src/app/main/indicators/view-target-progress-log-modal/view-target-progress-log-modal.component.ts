import { TargetProgressLogDto } from './../../../../shared/service-proxies/service-proxies';
import { Component, OnInit, ViewChild, Injector, ChangeDetectorRef } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ModalDirective } from 'ngx-bootstrap';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { PerformanceIndicatorsServiceProxy } from '@shared/service-proxies/service-proxies';

@Component({
  selector: 'app-view-target-progress-log-modal',
  templateUrl: './view-target-progress-log-modal.component.html',
  styleUrls: ['./view-target-progress-log-modal.component.css']
})
export class ViewTargetProgressLogModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    active = false;

    targetLog: TargetProgressLogDto[] = new Array();

    constructor(
        injector: Injector,
        private _performanceIndicatorService: PerformanceIndicatorsServiceProxy,
        private _changeDetector: ChangeDetectorRef
    ) {
        super(injector);
    }

    show(targetId?: number): void {
        if (targetId) {
            this._performanceIndicatorService.getTargetUpdateLog(targetId).subscribe(result => {
                this.targetLog = result;

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
