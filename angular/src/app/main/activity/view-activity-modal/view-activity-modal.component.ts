import { Component, OnInit, Output, EventEmitter, ViewChild, Injector, ChangeDetectorRef } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { IActivityWithOrganizationUnit } from '../IActivityWithOrganizationUnit';
import { CreateOrEditPerformanceActivityDto, PerformanceActivitiesServiceProxy, DataTypeEnum, UnitsEnum, ComparisonMethodEnum, CompletionStatusEnum, ActivityAttachmentDto, AuditInfoDto } from '@shared/service-proxies/service-proxies';
import { ModalDirective } from 'ngx-bootstrap';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { AppConsts } from '@shared/AppConsts';

@Component({
    selector: 'app-view-activity-modal',
    templateUrl: './view-activity-modal.component.html',
    styleUrls: ['./view-activity-modal.component.css']
})
export class ViewActivityModalComponent extends AppComponentBase {

    organizationUnitId: number;

    @Output() modalSave: EventEmitter<IActivityWithOrganizationUnit> = new EventEmitter<IActivityWithOrganizationUnit>();

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    active = false;
    saving = false;

    isShown = false;

    performanceActivity: CreateOrEditPerformanceActivityDto = new CreateOrEditPerformanceActivityDto();
    attachments: ActivityAttachmentDto[] = new Array();
    mdaName = '';
    deliverableName = '';
    auditInfo: AuditInfoDto = new AuditInfoDto();

    dataTypeEnum = DataTypeEnum;
    unitEnum = UnitsEnum;
    comparisonMethodEnum = ComparisonMethodEnum;
    completionStatusEnum = CompletionStatusEnum;

    showAttachmentAccordion = false;
    showAuditInfoAccordion = false;

    constructor(
        injector: Injector,
        private _performanceActivitiesService: PerformanceActivitiesServiceProxy,
        private _changeDetector: ChangeDetectorRef,
    ) {
        super(injector);
    }

    show(indicatorId: number): void {
        this._performanceActivitiesService
            .getPerformanceActivityForEdit(indicatorId)
            .subscribe(result => {
                this.performanceActivity = result.performanceActivity;
                this.attachments = result.attachments;
                this.mdaName = result.mdaName;
                this.deliverableName = result.deliverableName;
                this.auditInfo = result.auditInfo;

                this.active = true;
                this.modal.show();
                this._changeDetector.detectChanges();
            });
    }

    downloadResourceFile(attachment: ActivityAttachmentDto): string {
        return AppConsts.remoteServiceBaseUrl +
            '/File/DownloadBinaryFile?id=' +
            attachment.documentId +
            '&contentType=' +
            attachment.fileFormat +
            '&fileName=' +
            attachment.fileName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

}
