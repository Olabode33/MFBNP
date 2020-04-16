import { Component, OnInit, Output, EventEmitter, ViewChild, Injector, ChangeDetectorRef, ViewEncapsulation } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { IActivityWithOrganizationUnit } from '../IActivityWithOrganizationUnit';
import { CreateOrEditPerformanceActivityDto, PerformanceActivitiesServiceProxy, DataTypeEnum, UnitsEnum, ComparisonMethodEnum, CompletionStatusEnum, ActivityAttachmentDto, AuditInfoDto } from '@shared/service-proxies/service-proxies';
import { ModalDirective } from 'ngx-bootstrap';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { AppConsts } from '@shared/AppConsts';
import { Location } from '@angular/common';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { ActivatedRoute, Router, Params } from '@angular/router';

@Component({
    selector: 'app-view-activity',
    templateUrl: './view-activity.component.html',
    styleUrls: ['./view-activity.component.css'],
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class ViewActivityComponent extends AppComponentBase implements OnInit {

    organizationUnitId: number;

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
        private _activatedRoute: ActivatedRoute,
        private _router: Router,
        private _location: Location
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this._activatedRoute.params.subscribe((params: Params) => {

            if (params.activityId) {
                let activityId = +params['activityId'];
                this.show(activityId);
            }

        });
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

    goBack(): void {
        this.active = false;
        this._location.back();
    }

}
