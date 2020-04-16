import { Component, OnInit, Output, EventEmitter, ViewChild, Injector, ChangeDetectorRef } from '@angular/core';
import { IndicatorYearlyTargetDto, DataTypeEnum, UnitsEnum, ComparisonMethodEnum, PerformanceIndicatorsServiceProxy, CreateOrEditPerformanceIndicatorDto, UpdateTargetDto, IndicatorAttachmentDto } from '@shared/service-proxies/service-proxies';
import { IIndicatorWithOrganizationUnit } from '../IIndicatorWithOrganizationUnit';
import { ModalDirective } from 'ngx-bootstrap';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/common/app-component-base';
import { AppConsts } from '@shared/AppConsts';

@Component({
    selector: 'app-target-update-modal',
    templateUrl: './target-update-modal.component.html',
    styleUrls: ['./target-update-modal.component.css']
})
export class TargetUpdateModalComponent extends AppComponentBase {

    @Output() modalSave: EventEmitter<IIndicatorWithOrganizationUnit> = new EventEmitter<IIndicatorWithOrganizationUnit>();

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    active = false;
    saving = false;

    isShown = false;

    updateTarget: UpdateTargetDto = new UpdateTargetDto();
    indicatorTarget: IndicatorYearlyTargetDto = new IndicatorYearlyTargetDto();
    attachments: IndicatorAttachmentDto[] = new Array();
    attachment: IndicatorAttachmentDto = new IndicatorAttachmentDto();
    performanceIndicator: CreateOrEditPerformanceIndicatorDto = new CreateOrEditPerformanceIndicatorDto();
    preActual = '';

    dataTypeEnum = DataTypeEnum;
    unitEnum = UnitsEnum;
    comparisonMethodEnum = ComparisonMethodEnum;

    uploadUrl: string;
    uploadedFiles: any[] = [];

    constructor(
        injector: Injector,
        private _performanceIndicatorService: PerformanceIndicatorsServiceProxy,
        private _changeDetector: ChangeDetectorRef
    ) {
        super(injector);
        this.uploadUrl = AppConsts.remoteServiceBaseUrl + '/DemoUiComponents/UploadFiles';
    }

    show(target: UpdateTargetDto, indicator: CreateOrEditPerformanceIndicatorDto): void {
        this.updateTarget = target;
        this.indicatorTarget = target.target;
        this.attachments = target.attachments;
        this.performanceIndicator = indicator;
        this.preActual = target.target.actual;

        this.active = true;
        this.modal.show();
        this._changeDetector.detectChanges();
    }

    save(): void {
        this.saving = true;

        this.updateTarget.target = this.indicatorTarget;
        this.updateTarget.attachments = this.attachments;
        this._performanceIndicatorService.updateYearTargetProgress(this.updateTarget)
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

    onUpload(event): void {

        let resultArray = event.originalEvent.body.result;
        let files = event.files;

        for (let i = 0; i < resultArray.length; i++) {
            let attachment = new IndicatorAttachmentDto();

            attachment.documentId = resultArray[i].id;
            attachment.fileName = files[i].name;
            attachment.fileFormat = files[i].type;
            attachment.indicatorTargetId = this.indicatorTarget.id;

            this.attachments.push(attachment);
        }
    }

    onBeforeSend(event): void {
        event.xhr.setRequestHeader('Authorization', 'Bearer ' + abp.auth.getToken());
    }

    downloadResourceFile(attachment: IndicatorAttachmentDto): string {
        return AppConsts.remoteServiceBaseUrl +
            '/File/DownloadBinaryFile?id=' +
            attachment.documentId +
            '&contentType=' +
            attachment.fileFormat +
            '&fileName=' +
            attachment.fileName;
    }

}
