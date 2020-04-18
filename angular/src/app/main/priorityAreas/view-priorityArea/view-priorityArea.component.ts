import { Component, OnInit, ViewEncapsulation, Injector, ChangeDetectorRef, Output, EventEmitter, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { DeliverablesServiceProxy, GetDeliverableForEditOutput, CreateOrEditDeliverableDto, PriorityAreasServiceProxy, GetPriorityAreaForEditOutput, CreateOrEditPriorityAreaDto } from '@shared/service-proxies/service-proxies';
import { ActivatedRoute, Router, Params } from '@angular/router';
import { finalize, filter } from 'rxjs/operators';
import { Location } from '@angular/common';
import { IBasicOrganizationUnitInfo } from '@app/admin/organization-units/basic-organization-unit-info';
import { OrganizationUnitMembersComponent } from '@app/admin/organization-units/organization-unit-members.component';
import { PerformanceIndicatorsComponent } from '@app/main/indicators/PerformanceIndicators/PerformanceIndicators.component';
import { PerformanceActivityComponent } from '@app/main/activity/performanceActivity/performanceActivity.component';
import { PerformanceReviewComponent } from '@app/main/deliverables/reviews/performance-review/performance-review.component';
import { OrganizationTreeComponent } from '@app/admin/organization-units/organization-tree.component';

@Component({
  selector: 'app-view-priorityArea',
  templateUrl: './view-priorityArea.component.html',
  styleUrls: ['./view-priorityArea.component.css'],
  encapsulation: ViewEncapsulation.None,
  animations: [appModuleAnimation()]
})
export class ViewPriorityAreaComponent extends AppComponentBase implements OnInit {

    @Output() deliverableSelected = new EventEmitter<IBasicOrganizationUnitInfo>();

    @ViewChild('ouMembers', {static: true}) ouMembers: OrganizationUnitMembersComponent;
    @ViewChild('ouIndicators', {static: true}) ouIndicators: PerformanceIndicatorsComponent;
    @ViewChild('ouActivities', {static: true}) ouActivities: PerformanceActivityComponent;
    @ViewChild('ouReviews', {static: true}) ouReviews: PerformanceReviewComponent;
    organizationUnit: IBasicOrganizationUnitInfo = null;

    loading = false;

    deliverables: GetDeliverableForEditOutput[] = new Array();
    filteredDeliverables: GetDeliverableForEditOutput[] = new Array();
    mdaList: {value: number, name: string}[] = new Array();
    selectedMda: {value: number, name: string } = { value: -1, name: '-Select MDA'};

    priorityArea: CreateOrEditPriorityAreaDto = new CreateOrEditPriorityAreaDto();
    percentageAchieved = 0;


    constructor(
        injector: Injector,
        private _deliverablesServiceProxy: DeliverablesServiceProxy,
        private _priorityAreasServiceProxy: PriorityAreasServiceProxy,
        private _changeDetector: ChangeDetectorRef,
        private _activatedRoute: ActivatedRoute,
        private _router: Router,
        private _location: Location
        ) {
        super(injector);
    }

    ngOnInit() {
        this._activatedRoute.params.subscribe((params: Params) => {

            if (params.priorityAreaId) {
                let priorityAreaId = +params['priorityAreaId'];
                this.getDeliverables(priorityAreaId);
                this.getPriorityArea(priorityAreaId);
            }

        });
    }

    getDeliverables(priorityAreaId: number): void {
        this.loading = true;
        this._deliverablesServiceProxy.getForPriorityArea(priorityAreaId)
            .pipe(finalize(() => { this.loading = false; }))
            .subscribe(result => {
                this.deliverables = result.items;
                this.filteredDeliverables = this.deliverables;
                this.mdaList = result.items.map(x => {
                    return {value: x.deliverable.mdaId, name: x.mdaName};
                });
                const sum = this.deliverables.reduce((a, b) => a + b.percentageAchieved, 0);
                this.percentageAchieved = sum / this.deliverables.length;
            });
    }

    getPriorityArea(priorityAreaId: number): void {
        this._priorityAreasServiceProxy.getPriorityAreaForEdit(priorityAreaId)
            .subscribe(result => {
                this.priorityArea = result.priorityArea;
            });
    }

    filerByMda(): void {
        if (this.selectedMda.value === -1) {
            this.filteredDeliverables = this.deliverables;
        } else {
            this.filteredDeliverables = this.deliverables.filter(x => x.deliverable.mdaId === this.selectedMda.value);
        }
    }

    deliverableSelect(deliverable: CreateOrEditDeliverableDto) {
        this.ouSelected(<IBasicOrganizationUnitInfo>{
            id: deliverable.id,
            displayName: deliverable.displayName
        });
    }

    ouSelected(event: any): void {
        this.organizationUnit = event;
        this.ouMembers.organizationUnit = event;
        this.ouIndicators.organizationUnit = event;
        this.ouActivities.organizationUnit = event;
        this.ouReviews.organizationUnit = event;
    }

    goBack(): void {
        this._location.back();
    }

}
