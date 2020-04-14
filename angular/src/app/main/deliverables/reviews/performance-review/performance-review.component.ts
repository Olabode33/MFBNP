import { Component, OnInit, Output, EventEmitter, ViewChild, Injector } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { PerformanceReviewServiceProxy, CreateOrEditPerformanceReviewDto } from '@shared/service-proxies/service-proxies';
import { ViewReviewModalComponent } from '../view-review-modal/view-review-modal.component';
import { CreateEditReviewModalComponent } from '../create-edit-review-modal/create-edit-review-modal.component';
import { IIndicatorWithOrganizationUnit } from '@app/main/indicators/IIndicatorWithOrganizationUnit';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { IBasicOrganizationUnitInfo } from '@app/admin/organization-units/basic-organization-unit-info';
import { LazyLoadEvent } from 'primeng/api';
import { finalize } from 'rxjs/operators';

@Component({
    selector: 'app-performance-review',
    templateUrl: './performance-review.component.html',
    styleUrls: ['./performance-review.component.css']
})
export class PerformanceReviewComponent extends AppComponentBase {

    @Output() memberRemoved = new EventEmitter<IIndicatorWithOrganizationUnit>();
    @Output() membersAdded = new EventEmitter<IIndicatorWithOrganizationUnit>();

    @ViewChild('addReviewModal', { static: true }) addReviewModal: CreateEditReviewModalComponent;
    @ViewChild('viewReviewModal', { static: true }) viewReviewModal: ViewReviewModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    private _organizationUnit: IBasicOrganizationUnitInfo = null;

    constructor(
        injector: Injector,
        private _performanceReviewServiceProxy: PerformanceReviewServiceProxy
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
            this.refreshReviews();
        }
    }

    getPerformanceReviews(event?: LazyLoadEvent) {
        if (!this._organizationUnit) {
            return;
        }

        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);

            return;
        }

        this.primengTableHelper.showLoadingIndicator();
        this._performanceReviewServiceProxy.getAllForUnit(
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

    refreshReviews(): void {
        this.reloadPage();
    }

    openAddModal(): void {
        this.addReviewModal.show(this._organizationUnit.id);
    }

    editReview(reviewId): void {
        this.addReviewModal.show(this._organizationUnit.id, reviewId);
    }

    viewReview(reviewId: number): void {
        this.viewReviewModal.show(reviewId);
    }

    deleteReview(review: CreateOrEditPerformanceReviewDto): void {
        this.message.confirm(
            this.l('DeleteReviewWarningMessage'),
            this.l('AreYouSure'),
            isConfirmed => {
                if (isConfirmed) {
                    this._performanceReviewServiceProxy
                        .delete(review.id)
                        .subscribe(() => {
                            this.notify.success(this.l('SuccessfullyRemoved'));
                            //this.memberRemoved.emit(null);
                            this.refreshReviews();
                        });
                }
            }
        );
    }

}
