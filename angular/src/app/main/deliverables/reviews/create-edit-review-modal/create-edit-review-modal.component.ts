import { Component, OnInit, Output, EventEmitter, ViewChild, Injector, ChangeDetectorRef } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { CreateOrEditPerformanceReviewDto, PerformanceReviewServiceProxy } from '@shared/service-proxies/service-proxies';
import { IIndicatorWithOrganizationUnit } from '@app/main/indicators/IIndicatorWithOrganizationUnit';
import { ModalDirective } from 'ngx-bootstrap';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { finalize } from 'rxjs/operators';

@Component({
    selector: 'app-create-edit-review-modal',
    templateUrl: './create-edit-review-modal.component.html',
    styleUrls: ['./create-edit-review-modal.component.css']
})
export class CreateEditReviewModalComponent extends AppComponentBase {

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

    show(unitId: number, reviewId?: number): void {
        this.showYearlyTargetForm = false;
        if (!reviewId) {
            this.review = new CreateOrEditPerformanceReviewDto();
            this.review.organizationUnitId = unitId;
            this.review.id = reviewId;

            this.active = true;
            this.modal.show();
            this._changeDetector.detectChanges();
        } else {
            this._performanceReviewService
                .getPerformanceReviewForEdit(reviewId)
                .subscribe(result => {
                    this.review = result.review;

                    this.active = true;
                    this.modal.show();
                    this._changeDetector.detectChanges();
                });
        }
    }

    save(): void {
        this.saving = true;

        this._performanceReviewService.createOrEdit(this.review)
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
