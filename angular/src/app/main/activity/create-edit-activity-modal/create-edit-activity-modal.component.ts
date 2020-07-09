import { Component, OnInit, Output, EventEmitter, ViewChild, Injector, ChangeDetectorRef } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { IActivityWithOrganizationUnit } from '../IActivityWithOrganizationUnit';
import { CreateOrEditPerformanceActivityDto, PerformanceActivitiesServiceProxy, DataTypeEnum, UnitsEnum, ComparisonMethodEnum } from '@shared/service-proxies/service-proxies';
import { ModalDirective } from 'ngx-bootstrap';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { finalize } from 'rxjs/operators';
import * as moment from 'moment';

@Component({
    selector: 'app-create-edit-activity-modal',
    templateUrl: './create-edit-activity-modal.component.html',
    styleUrls: ['./create-edit-activity-modal.component.css']
})
export class CreateEditActivityModalComponent extends AppComponentBase {

    organizationUnitId: number;

    @Output() modalSave: EventEmitter<IActivityWithOrganizationUnit> = new EventEmitter<IActivityWithOrganizationUnit>();

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    active = false;
    saving = false;

    isShown = false;

    performanceActivity: CreateOrEditPerformanceActivityDto = new CreateOrEditPerformanceActivityDto();
    plannedStartDate: Date;
    plannedCompletionDate: Date;
    actualStartDate: Date;
    actualCompletionDate: Date;

    dataTypeEnum = DataTypeEnum;
    unitEnum = UnitsEnum;
    comparisonMethodEnum = ComparisonMethodEnum;

    constructor(
        injector: Injector,
        private _performanceActivityService: PerformanceActivitiesServiceProxy,
        private _changeDetector: ChangeDetectorRef
    ) {
        super(injector);
    }

    show(unitId: number, indicatorId?: number): void {
        if (!indicatorId) {
            this.performanceActivity = new CreateOrEditPerformanceActivityDto();
            this.performanceActivity.organizationUnitId = unitId;
            this.performanceActivity.id = indicatorId;

            this.active = true;
            this.modal.show();
            this._changeDetector.detectChanges();
        } else {
            this._performanceActivityService
                .getPerformanceActivityForEdit(indicatorId)
                .subscribe(result => {
                    this.performanceActivity = result.performanceActivity;
                    if (result.performanceActivity.plannedStartDate) {
                        this.plannedStartDate = result.performanceActivity.plannedStartDate.toDate();
                    }
                    if (result.performanceActivity.plannedCompletionDate) {
                        this.plannedCompletionDate = result.performanceActivity.plannedCompletionDate.toDate();
                    }
                    if (result.performanceActivity.actualStartDate) {
                        this.actualStartDate = result.performanceActivity.actualStartDate.toDate();
                    }
                    if (result.performanceActivity.actualCompletionDate) {
                        this.actualCompletionDate = result.performanceActivity.actualCompletionDate.toDate();
                    }
                    this.active = true;
                    this.modal.show();
                    this._changeDetector.detectChanges();
                });
        }
    }

    save(): void {
        this.saving = true;

        if (this.plannedStartDate) {
            this.performanceActivity.plannedStartDate = moment(this.plannedStartDate);
        } else {
            this.performanceActivity.plannedStartDate = null;
        }

        if (this.plannedCompletionDate) {
            this.performanceActivity.plannedCompletionDate = moment(this.plannedCompletionDate);
        } else {
            this.performanceActivity.plannedCompletionDate = null;
        }

        if (this.actualStartDate) {
            this.performanceActivity.actualStartDate = moment(this.actualStartDate);
        } else {
            this.performanceActivity.actualStartDate = null;
        }

        if (this.actualCompletionDate) {
            this.performanceActivity.actualCompletionDate = moment(this.actualCompletionDate);
        } else {
            this.performanceActivity.actualCompletionDate = null;
        }

        this._performanceActivityService.createOrEdit(this.performanceActivity)
            .pipe(finalize(() => { this.saving = false; }))
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
            });
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }


}
