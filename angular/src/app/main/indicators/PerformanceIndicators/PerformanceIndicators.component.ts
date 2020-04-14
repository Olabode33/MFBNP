import { Component, OnInit, Output, EventEmitter, ViewChild, Injector } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { IBasicOrganizationUnitInfo } from '@app/admin/organization-units/basic-organization-unit-info';
import { PerformanceIndicatorsServiceProxy, OrganizationUnitUserListDto, CreateOrEditPerformanceIndicatorDto } from '@shared/service-proxies/service-proxies';
import { LazyLoadEvent } from 'primeng/api';
import { finalize } from 'rxjs/operators';
import { IIndicatorWithOrganizationUnit } from '../IIndicatorWithOrganizationUnit';
import { CreateEditPerformanceIndicatorModalComponent } from '../createEditPerformanceIndicatorModal/createEditPerformanceIndicatorModal.component';
import { ViewIndicatorComponent } from '../view-indicator/view-indicator.component';
import { UpdateIndicatorProgressComponent } from '../update-indicator-progress/update-indicator-progress.component';
import { EntityTypeHistoryModalComponent } from '@app/shared/common/entityHistory/entity-type-history-modal.component';

@Component({
    selector: 'app-PerformanceIndicators',
    templateUrl: './PerformanceIndicators.component.html',
    styleUrls: ['./PerformanceIndicators.component.css']
})
export class PerformanceIndicatorsComponent extends AppComponentBase implements OnInit {

    @Output() memberRemoved = new EventEmitter<IIndicatorWithOrganizationUnit>();
    @Output() membersAdded = new EventEmitter<IIndicatorWithOrganizationUnit>();

    @ViewChild('addIndicatorModal', { static: true }) addIndicatorModal: CreateEditPerformanceIndicatorModalComponent;
    @ViewChild('updateIndicatorModal', { static: true }) updateIndicatorModal: UpdateIndicatorProgressComponent;
    @ViewChild('viewIndicatorModal', { static: true }) viewIndicatorModal: ViewIndicatorComponent;
    @ViewChild('entityTypeHistoryModal', { static: true }) entityTypeHistoryModal: EntityTypeHistoryModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    private _organizationUnit: IBasicOrganizationUnitInfo = null;

    _entityTypeFullName = 'PMSDemo.PerformanceIndicators.PerformanceIndicator';

    constructor(
        injector: Injector,
        private _performanceIndicatorServiceProxy: PerformanceIndicatorsServiceProxy
    ) {
        super(injector);
    }

    get organizationUnit(): IBasicOrganizationUnitInfo {
        return this._organizationUnit;
    }

    set organizationUnit(ou: IBasicOrganizationUnitInfo) {
        if (this._organizationUnit === ou) {
            return;
        }

        this._organizationUnit = ou;
        //this.addIndicatorModal.organizationUnitId = ou.id;
        if (ou) {
            this.refreshIndicators();
        }
    }

    ngOnInit() {
    }

    getPerformanceIndicators(event?: LazyLoadEvent) {
        if (!this._organizationUnit) {
            return;
        }

        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);

            return;
        }

        this.primengTableHelper.showLoadingIndicator();
        this._performanceIndicatorServiceProxy.getAllForUnit(
            '',
            this._organizationUnit.id,
            this.primengTableHelper.getSorting(this.dataTable),
            this.primengTableHelper.getSkipCount(this.paginator, event),
            this.primengTableHelper.getMaxResultCount(this.paginator, event)
        ).pipe(finalize(() => this.primengTableHelper.hideLoadingIndicator()))
         .subscribe(result => {
            this.primengTableHelper.totalRecordsCount = result.totalCount;
            this.primengTableHelper.records = result.items;
            //console.log(this.primengTableHelper.records);
            this.primengTableHelper.hideLoadingIndicator();
        });
    }

    reloadPage(): void {
        this.paginator.changePage(this.paginator.getPage());
    }

    refreshIndicators(): void {
        this.reloadPage();
    }

    openAddModal(): void {
        this.addIndicatorModal.show(this._organizationUnit.id);
    }

    viewIndicator(indicatorId: number): void {
        this.viewIndicatorModal.show(indicatorId);
    }

    editIndicator(indicatorId: number): void {
        this.addIndicatorModal.show(this._organizationUnit.id, indicatorId);
    }

    updateProgress(indicatorId: number): void {
        this.updateIndicatorModal.show(indicatorId);
    }

    deleteIndicator(indicator: CreateOrEditPerformanceIndicatorDto): void {
        this.message.confirm(
            this.l('RemoveIndicatorFromOuWarningMessage', indicator.name),
            this.l('AreYouSure'),
            isConfirmed => {
                if (isConfirmed) {
                    this._performanceIndicatorServiceProxy
                        .delete(indicator.id)
                        .subscribe(() => {
                            this.notify.success(this.l('SuccessfullyRemoved'));
                            //this.memberRemoved.emit(null);
                            this.refreshIndicators();
                        });
                }
            }
        );
    }

    showHistory(indicator: CreateOrEditPerformanceIndicatorDto): void {
        this.entityTypeHistoryModal.show({
            entityId: indicator.id.toString(),
            entityTypeFullName: this._entityTypeFullName,
            entityTypeDescription: indicator.name
        });
    }

    // addMembers(data: any): void {
    //     this.membersAdded.emit({
    //         userIds: data.userIds,
    //         ouId: data.ouId
    //     });

    //     this.refreshMembers();
    // }

}
