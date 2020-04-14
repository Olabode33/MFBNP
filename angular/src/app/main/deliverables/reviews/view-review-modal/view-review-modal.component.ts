import { Component, OnInit, Output, EventEmitter, ViewChild, Injector, ChangeDetectorRef } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { IIndicatorWithOrganizationUnit } from '@app/main/indicators/IIndicatorWithOrganizationUnit';
import { ModalDirective } from 'ngx-bootstrap';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { CreateOrEditPerformanceReviewDto, PerformanceReviewServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/operators';

@Component({
    selector: 'app-view-review-modal',
    templateUrl: './view-review-modal.component.html',
    styleUrls: ['./view-review-modal.component.css']
})
export class ViewReviewModalComponent extends AppComponentBase {

    organizationUnitId: number;

    @Output() modalSave: EventEmitter<IIndicatorWithOrganizationUnit> = new EventEmitter<IIndicatorWithOrganizationUnit>();

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    active = false;
    saving = false;

    isShown = false;
    showYearlyTargetForm = false;

    review: CreateOrEditPerformanceReviewDto = new CreateOrEditPerformanceReviewDto();

    constructor(
        injector: Injector,
        private _performanceReviewService: PerformanceReviewServiceProxy,
        private _changeDetector: ChangeDetectorRef
    ) {
        super(injector);
    }

    show(reviewId?: number): void {
        this._performanceReviewService
            .getPerformanceReviewForEdit(reviewId)
            .subscribe(result => {
                this.review = result.review;

                this.active = true;
                this.modal.show();
                this._changeDetector.detectChanges();
            });
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
