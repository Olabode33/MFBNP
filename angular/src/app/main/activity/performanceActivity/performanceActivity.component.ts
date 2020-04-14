import { UpdateActivityProgressModalComponent } from './../update-activity-progress-modal/update-activity-progress-modal.component';
import { CreateEditActivityModalComponent } from './../create-edit-activity-modal/create-edit-activity-modal.component';
import { Component, OnInit, Output, EventEmitter, ViewChild, Injector } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { IActivityWithOrganizationUnit } from '../IActivityWithOrganizationUnit';
import { ViewActivityModalComponent } from '../view-activity-modal/view-activity-modal.component';
import { PerformanceActivitiesServiceProxy, CreateOrEditPerformanceIndicatorDto, CreateOrEditPerformanceActivityDto } from '@shared/service-proxies/service-proxies';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { IBasicOrganizationUnitInfo } from '@app/admin/organization-units/basic-organization-unit-info';
import { LazyLoadEvent } from 'primeng/api';
import { finalize } from 'rxjs/operators';
import { EntityTypeHistoryModalComponent } from '@app/shared/common/entityHistory/entity-type-history-modal.component';

@Component({
  selector: 'app-performanceActivity',
  templateUrl: './performanceActivity.component.html',
  styleUrls: ['./performanceActivity.component.css']
})
export class PerformanceActivityComponent extends AppComponentBase implements OnInit {

    @Output() memberRemoved = new EventEmitter<IActivityWithOrganizationUnit>();
    @Output() membersAdded = new EventEmitter<IActivityWithOrganizationUnit>();

    @ViewChild('addActivityModal', { static: true }) addActivityModal: CreateEditActivityModalComponent;
    @ViewChild('updateActivityModal', { static: true }) updateActivityModal: UpdateActivityProgressModalComponent;
    @ViewChild('viewActivityModal', { static: true }) viewActivityModal: ViewActivityModalComponent;
    @ViewChild('entityTypeHistoryModal', { static: true }) entityTypeHistoryModal: EntityTypeHistoryModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    private _organizationUnit: IBasicOrganizationUnitInfo = null;

    _entityTypeFullName = 'PMSDemo.PerformanceActivities.PerformanceActivity';

    constructor(
        injector: Injector,
        private _performanceActivityServiceProxy: PerformanceActivitiesServiceProxy
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
            this.refreshActivities();
        }
    }

    ngOnInit() {
    }

    getPerformanceActivities(event?: LazyLoadEvent) {
        if (!this._organizationUnit) {
            return;
        }

        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);

            return;
        }

        this.primengTableHelper.showLoadingIndicator();
        this._performanceActivityServiceProxy.getAllForUnit(
            '',
            this._organizationUnit.id,
            this.primengTableHelper.getSorting(this.dataTable),
            this.primengTableHelper.getSkipCount(this.paginator, event),
            this.primengTableHelper.getMaxResultCount(this.paginator, event)
        ).pipe(finalize(() => this.primengTableHelper.hideLoadingIndicator()))
         .subscribe(result => {
            this.primengTableHelper.totalRecordsCount = result.totalCount;
            this.primengTableHelper.records = result.items;
            this.primengTableHelper.hideLoadingIndicator();
        });
    }

    reloadPage(): void {
        this.paginator.changePage(this.paginator.getPage());
    }

    refreshActivities(): void {
        this.reloadPage();
    }

    openAddModal(): void {
        this.addActivityModal.show(this._organizationUnit.id);
    }

    viewActivity(activityId: number): void {
        this.viewActivityModal.show(activityId);
    }

    editActivity(activityId: number): void {
        this.addActivityModal.show(this._organizationUnit.id, activityId);
    }

    updateProgress(indicatorId: number): void {
        this.updateActivityModal.show(indicatorId);
    }

    deleteActivity(activity: CreateOrEditPerformanceActivityDto): void {
        this.message.confirm(
            this.l('RemoveActivityFromOuWarningMessage', activity.name),
            this.l('AreYouSure'),
            isConfirmed => {
                if (isConfirmed) {
                    this._performanceActivityServiceProxy
                        .delete(activity.id)
                        .subscribe(() => {
                            this.notify.success(this.l('SuccessfullyRemoved'));
                            //this.memberRemoved.emit(null);
                            this.refreshActivities();
                        });
                }
            }
        );
    }

    showHistory(activity: CreateOrEditPerformanceActivityDto): void {
        this.entityTypeHistoryModal.show({
            entityId: activity.id.toString(),
            entityTypeFullName: this._entityTypeFullName,
            entityTypeDescription: activity.name
        });
    }

}
