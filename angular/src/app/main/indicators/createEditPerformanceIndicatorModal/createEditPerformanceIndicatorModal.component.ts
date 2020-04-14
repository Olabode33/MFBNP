import { DataTypeEnum, UnitsEnum, ComparisonMethodEnum, IndicatorYearlyTargetDto, CreateEditIndicatorAndTargetDto } from './../../../../shared/service-proxies/service-proxies';
import { Component, OnInit, Output, EventEmitter, ViewChild, Injector, ChangeDetectorRef } from '@angular/core';
import { IIndicatorWithOrganizationUnit } from '../IIndicatorWithOrganizationUnit';
import { PerformanceIndicatorsServiceProxy, GetPerformanceIndicatorForEditOutput, CreateOrEditPerformanceIndicatorDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ModalDirective } from 'ngx-bootstrap';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { finalize } from 'rxjs/operators';

@Component({
    selector: 'app-createEditPerformanceIndicatorModal',
    templateUrl: './createEditPerformanceIndicatorModal.component.html',
    styleUrls: ['./createEditPerformanceIndicatorModal.component.css']
})
export class CreateEditPerformanceIndicatorModalComponent extends AppComponentBase {

    organizationUnitId: number;

    @Output() modalSave: EventEmitter<IIndicatorWithOrganizationUnit> = new EventEmitter<IIndicatorWithOrganizationUnit>();

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    active = false;
    saving = false;

    isShown = false;
    showYearlyTargetForm = false;

    indicator: CreateEditIndicatorAndTargetDto = new CreateEditIndicatorAndTargetDto();
    performanceIndicator: CreateOrEditPerformanceIndicatorDto = new CreateOrEditPerformanceIndicatorDto();
    yearlyTargets: IndicatorYearlyTargetDto[] = new Array();
    target: IndicatorYearlyTargetDto = new IndicatorYearlyTargetDto();

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

    show(unitId: number, indicatorId?: number): void {
        this.showYearlyTargetForm = false;
        if (!indicatorId) {
            this.performanceIndicator = new CreateOrEditPerformanceIndicatorDto();
            this.performanceIndicator.organizationUnitId = unitId;
            this.performanceIndicator.id = indicatorId;
            this.yearlyTargets = new Array();

            this.active = true;
            this.modal.show();
            this._changeDetector.detectChanges();
        } else {
            this._performanceIndicatorService
                .getPerformanceIndicatorForEdit(indicatorId)
                .subscribe(result => {
                    this.performanceIndicator = result.performanceIndicator;
                    this.yearlyTargets = result.targets.map(x => x.target);

                    this.active = true;
                    this.modal.show();
                    //this._changeDetector.detectChanges();
                });
        }
    }

    save(): void {
        this.saving = true;

        if (this.performanceIndicator.dataType === DataTypeEnum.Boolean) {
            this.performanceIndicator.comparisonMethod = ComparisonMethodEnum.GreaterThanOrEqual;
        }

        if (this.yearlyTargets.length <= 0) {
           this.message.error('No targets defined');
           this.saving = false;
           return;
        }

        this.indicator.yearlyTarget = this.yearlyTargets;
        this.indicator.indicator = this.performanceIndicator;

        this._performanceIndicatorService.createOrEdit(this.indicator)
            .pipe(finalize(() => { this.saving = false; }))
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
            });
    }

    addTarget(): void {
        this.target.indicatorId = this.performanceIndicator.id;
        this.yearlyTargets.push(this.target);
        console.log(this.target);

        this.target = new IndicatorYearlyTargetDto();
    }

    removeTarget(index: number): void {
        let y = this.yearlyTargets[index];
        this.yearlyTargets.splice(index, 1);
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

}
