﻿import { CreateOrEditMdaDto, DeliverablesServiceProxy } from './../../../../shared/service-proxies/service-proxies';
import { CreateEditMdaModalComponent } from './../create-edit-mda-modal/create-edit-mda-modal.component';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MdasServiceProxy  } from '@shared/service-proxies/service-proxies';
import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as _ from 'lodash';
import * as moment from 'moment';
import { finalize } from 'rxjs/operators';

@Component({
    templateUrl: './mda.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class MDAComponent extends AppComponentBase {

    @ViewChild('createEditMdaModalComponent', { static: true }) createEditMdaModalComponent: CreateEditMdaModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';


    generatingReport = false;

    constructor(
        injector: Injector,
        private _mdaServiceProxy: MdasServiceProxy,
        private _deliverableServiceProxy: DeliverablesServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _router: Router
    ) {
        super(injector);
    }

    getMDAs(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._mdaServiceProxy.getAll(
            this.filterText,
            this.primengTableHelper.getSorting(this.dataTable),
            this.primengTableHelper.getSkipCount(this.paginator, event),
            this.primengTableHelper.getMaxResultCount(this.paginator, event)
        ).subscribe(result => {
            console.log(result.items);
            this.primengTableHelper.totalRecordsCount = result.totalCount;
            this.primengTableHelper.records = result.items;
            this.primengTableHelper.hideLoadingIndicator();
        });
    }

    reloadPage(): void {
        this.paginator.changePage(this.paginator.getPage());
    }

    createMDA(): void {
        this.createEditMdaModalComponent.show({ parentId: null});
    }

    viewMDA(priorityAreaId: number): void {
        // this._router.navigate(['app/main/priority-areas/view', priorityAreaId]);
    }

    deleteMDA(mda: CreateOrEditMdaDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._mdaServiceProxy.delete(mda.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(mdaId: number): void {
        this.generatingReport = true;
        this._deliverableServiceProxy.getMdaDeliverablesToExcel(mdaId)
            .pipe(finalize(() => this.generatingReport = false))
            .subscribe(result => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }

    exportAllToExcel(): void {
        this.generatingReport = true;
        this._deliverableServiceProxy.getAllMdasDeliverablesToExcel()
            .pipe(finalize(() => this.generatingReport = false))
            .subscribe(result => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }
}
