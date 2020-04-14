import { Component, OnInit, Output, EventEmitter, ViewChild, Injector, ChangeDetectorRef } from '@angular/core';
import { IIndicatorWithOrganizationUnit } from '../IIndicatorWithOrganizationUnit';
import { ModalDirective } from 'ngx-bootstrap';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { CreateOrEditPerformanceIndicatorDto, DataTypeEnum, UnitsEnum, ComparisonMethodEnum, PerformanceIndicatorsServiceProxy, IndicatorYearlyTargetDto, UpdateTargetDto } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TargetUpdateModalComponent } from '../target-update-modal/target-update-modal.component';

@Component({
    selector: 'app-update-indicator-progress',
    templateUrl: './update-indicator-progress.component.html',
    styleUrls: ['./update-indicator-progress.component.css']
})
export class UpdateIndicatorProgressComponent extends AppComponentBase {

    organizationUnitId: number;

    @Output() modalSave: EventEmitter<IIndicatorWithOrganizationUnit> = new EventEmitter<IIndicatorWithOrganizationUnit>();

    @ViewChild('updateTargetModal', { static: true }) updateTargetModal: TargetUpdateModalComponent;
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    active = false;
    saving = false;

    isShown = false;

    performanceIndicator: CreateOrEditPerformanceIndicatorDto = new CreateOrEditPerformanceIndicatorDto();
    yearlyTargets: UpdateTargetDto[] = new Array();

    dataTypeEnum = DataTypeEnum;
    unitEnum = UnitsEnum;
    comparisonMethodEnum = ComparisonMethodEnum;

    constructor(
        injector: Injector,
        private _performanceIndicatorService: PerformanceIndicatorsServiceProxy,
        private _changeDetector: ChangeDetectorRef
    ) {
        super(injector);
    }

    show(indicatorId: number): void {
        this._performanceIndicatorService
            .getPerformanceIndicatorForEdit(indicatorId)
            .subscribe(result => {
                this.performanceIndicator = result.performanceIndicator;
                this.yearlyTargets = result.targets;

                this.active = true;
                this.modal.show();
                this._changeDetector.detectChanges();
            });
    }

    reload(): void {
        if (this.performanceIndicator.id) {
            this._performanceIndicatorService
                .getPerformanceIndicatorForEdit(this.performanceIndicator.id)
                .subscribe(result => {
                    this.performanceIndicator = result.performanceIndicator;
                    this.yearlyTargets = result.targets;
                });
        }
    }

    updateTarget(target: UpdateTargetDto): void {
        this.updateTargetModal.show(target, this.performanceIndicator);
    }

    save(): void {
        this.saving = true;

        this._performanceIndicatorService.updateProgress(this.performanceIndicator)
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
