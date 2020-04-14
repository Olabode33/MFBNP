import { Component, OnInit, ViewChild, Output, EventEmitter, Injector, ChangeDetectorRef } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ModalDirective } from 'ngx-bootstrap';
import { MdasServiceProxy, CreateOrEditMdaDto, CommonLookupServiceProxy, FindUsersInput, NameValueDto } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/operators';
import { CommonLookupModalComponent } from '@app/shared/common/lookup/common-lookup-modal.component';

export interface IMdaOnEdit {
    id?: number;
    parentId?: number;
    displayName?: string;
}

@Component({
    selector: 'app-create-edit-mda-modal',
    templateUrl: './create-edit-mda-modal.component.html',
    styleUrls: ['./create-edit-mda-modal.component.css']
})
export class CreateEditMdaModalComponent extends AppComponentBase implements OnInit {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('userLookupModal', {static: true}) userLookupModal: CommonLookupModalComponent;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    mda: IMdaOnEdit = {};
    responsibleUserName = '';
    role = '';
    responsiblePersonId = -1;

    constructor(
        injector: Injector,
        private _mdaServiceProxy: MdasServiceProxy,
        private _changeDetector: ChangeDetectorRef,
        private _commonLookupService: CommonLookupServiceProxy,
    ) {
        super(injector);
    }

    ngOnInit() {
        this.userLookupModal.configure({
            title: this.l('SelectAUser'),
            dataSource: (skipCount: number, maxResultCount: number, filter: string, tenantId?: number) => {
                let input = new FindUsersInput();
                input.filter = filter;
                input.maxResultCount = maxResultCount;
                input.skipCount = skipCount;
                input.tenantId = null;
                return this._commonLookupService.findUsers(input);
            }
        });
    }

    onShown(): void {
        document.getElementById('mdaDisplayName').focus();
    }

    show(mdaUnit: IMdaOnEdit): void {
        this.mda = mdaUnit;
        if (mdaUnit.id) {
            this._mdaServiceProxy.getMdaForEdit(mdaUnit.id).subscribe(result => {
                this.responsiblePersonId = result.mda.responsiblePersonId;
                this.responsibleUserName = result.responsiblePersonName;
                this.role = result.mda.role;
                this.active = true;
                this.modal.show();
                this._changeDetector.detectChanges();
            });
        } else {
            this.active = true;
            this.modal.show();
            this._changeDetector.detectChanges();
        }

    }

    save(): void {
        const createEditInput = new CreateOrEditMdaDto();
        createEditInput.id = this.mda.id;
        createEditInput.displayName = this.mda.displayName;
        createEditInput.responsiblePersonId = this.responsiblePersonId;

        this.saving = true;
        this._mdaServiceProxy
            .createOrEdit(createEditInput)
            .pipe(finalize(() => this.saving = false))
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

    showUserLookUpModal(): void {
        this.userLookupModal.show();
    }

    selectUser(item: NameValueDto): void {
        this.responsiblePersonId = parseInt(item.value);
        this.responsibleUserName = item.name;
    }

}
