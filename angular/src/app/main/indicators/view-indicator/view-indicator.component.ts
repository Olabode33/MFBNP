import { AuditInfoDto } from './../../../../shared/service-proxies/service-proxies';
import { Component, OnInit, Output, EventEmitter, ViewChild, Injector, ChangeDetectorRef, ViewEncapsulation } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { IIndicatorWithOrganizationUnit } from '../IIndicatorWithOrganizationUnit';
import { ModalDirective } from 'ngx-bootstrap';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { CreateOrEditPerformanceIndicatorDto, DataTypeEnum, UnitsEnum, ComparisonMethodEnum, PerformanceIndicatorsServiceProxy, IndicatorYearlyTargetDto, UpdateTargetDto } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/operators';
import { TargetUpdateModalComponent } from '../target-update-modal/target-update-modal.component';
import { EntityTypeHistoryModalComponent } from '@app/shared/common/entityHistory/entity-type-history-modal.component';
import { Location } from '@angular/common';
import { ActivatedRoute, Router, Params } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { ViewTargetProgressLogModalComponent } from '../view-target-progress-log-modal/view-target-progress-log-modal.component';

@Component({
  selector: 'app-view-indicator',
  templateUrl: './view-indicator.component.html',
  styleUrls: ['./view-indicator.component.css'],
  encapsulation: ViewEncapsulation.None,
  animations: [appModuleAnimation()]
})
export class ViewIndicatorComponent extends AppComponentBase implements OnInit {

    organizationUnitId: number;

    @Output() modalSave: EventEmitter<IIndicatorWithOrganizationUnit> = new EventEmitter<IIndicatorWithOrganizationUnit>();

    @ViewChild('entityTypeHistoryModal', { static: true }) entityTypeHistoryModal: EntityTypeHistoryModalComponent;
    @ViewChild('progressUpdateLogModal', { static: true}) progressUpdateLogModal: ViewTargetProgressLogModalComponent;
    @ViewChild('updateTargetModal', { static: true }) updateTargetModal: TargetUpdateModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    active = false;
    saving = false;

    isShown = false;

    performanceIndicator: CreateOrEditPerformanceIndicatorDto = new CreateOrEditPerformanceIndicatorDto();
    yearlyTargets: UpdateTargetDto[] = new Array();
    mdaName = '';
    deliverableName = '';
    auditInfo: AuditInfoDto = new AuditInfoDto();

    dataTypeEnum = DataTypeEnum;
    unitEnum = UnitsEnum;
    comparisonMethodEnum = ComparisonMethodEnum;

    _entityTypeFullName = 'PMSDemo.PerformanceIndicators.IndicatorYearlyTarget';

    showAttachmentAccordion = false;
    showAuditInfoAccordion = false;

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
        this._activatedRoute.params.subscribe((params: Params) => {
            let indicatorId: number;

            if (params.indicatorId) {
                indicatorId = +params['indicatorId'];
            }

            this.show(indicatorId);
        });
    }

    show(indicatorId: number): void {
        this._performanceIndicatorService
            .getPerformanceIndicatorForEdit(indicatorId)
            .subscribe(result => {
                this.performanceIndicator = result.performanceIndicator;
                this.yearlyTargets = result.targets;
                this.auditInfo = result.auditInfo;
                this.mdaName = result.mdaName;
                this.deliverableName = result.deliverableName;
                console.log(this.auditInfo);

                this.active = true;
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

                    this.mdaName = result.mdaName;
                    this.deliverableName = result.deliverableName;
                });
        }
    }

    updateTarget(target: UpdateTargetDto): void {
        this.updateTargetModal.show(target, this.performanceIndicator);
    }

    goBack(): void {
        this.active = false;
        this._location.back();
    }

    showHistory(target: IndicatorYearlyTargetDto): void {
        this.progressUpdateLogModal.show(target.id);
        // this.entityTypeHistoryModal.show({
        //     entityId: target.id.toString(),
        //     entityTypeFullName: this._entityTypeFullName,
        //     entityTypeDescription: this.performanceIndicator.name + '- <br /> Target Year: ' + target.year.toString()
        // });
    }

}
