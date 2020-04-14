import { DataTypeEnum, UnitsEnum, ComparisonMethodEnum, IndicatorYearlyTargetDto, CreateEditIndicatorAndTargetDto } from '../../../../shared/service-proxies/service-proxies';
import { Component, OnInit, Output, EventEmitter, ViewChild, Injector, ChangeDetectorRef, AfterViewInit, ViewEncapsulation } from '@angular/core';
import { IIndicatorWithOrganizationUnit } from '../IIndicatorWithOrganizationUnit';
import { PerformanceIndicatorsServiceProxy, GetPerformanceIndicatorForEditOutput, CreateOrEditPerformanceIndicatorDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ModalDirective } from 'ngx-bootstrap';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { finalize } from 'rxjs/operators';
import { Location } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';

@Component({
    selector: 'app-createEditPerformanceIndicator',
    templateUrl: './createEditPerformanceIndicator.component.html',
    styleUrls: ['./createEditPerformanceIndicator.component.css'],
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class CreateEditPerformanceIndicatorComponent extends AppComponentBase implements OnInit, AfterViewInit {

    organizationUnitId: number;

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
        private _changeDetector: ChangeDetectorRef,
        private _activatedRoute: ActivatedRoute,
        private _router: Router,
        private _location: Location
    ) {
        super(injector);
    }

    ngOnInit(): void {
        let unitId = this._activatedRoute.snapshot.queryParams['unitId'];
        let indicatorId = this._activatedRoute.snapshot.queryParams['indicatorId'];
        this.show(unitId, indicatorId);
    }

    ngAfterViewInit(): void {

    }

    show(unitId: number, indicatorId?: number): void {
        this.showYearlyTargetForm = false;
        if (!indicatorId) {
            this.performanceIndicator = new CreateOrEditPerformanceIndicatorDto();
            this.performanceIndicator.organizationUnitId = unitId;
            this.performanceIndicator.id = indicatorId;
            this.yearlyTargets = new Array();

            this.active = true;
            this._changeDetector.detectChanges();
        } else {
            this._performanceIndicatorService
                .getPerformanceIndicatorForEdit(indicatorId)
                .subscribe(result => {
                    this.performanceIndicator = result.performanceIndicator;
                    this.yearlyTargets = result.targets.map(x => x.target);

                    this.active = true;
                    this._changeDetector.detectChanges();
                });
        }
    }

    save(): void {
        this.saving = true;

        if (this.performanceIndicator.dataType === DataTypeEnum.Boolean) {
            this.performanceIndicator.comparisonMethod = ComparisonMethodEnum.GreaterThanOrEqual;
        }

        this.indicator.yearlyTarget = this.yearlyTargets;
        this.indicator.indicator = this.performanceIndicator;

        this._performanceIndicatorService.createOrEdit(this.indicator)
            .pipe(finalize(() => { this.saving = false; }))
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.goBack();
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

    goBack(): void {
        this.active = false;
        this._location.back();
    }

}
