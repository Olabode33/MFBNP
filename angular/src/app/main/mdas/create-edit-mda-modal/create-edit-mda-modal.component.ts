import { Component, OnInit, ViewChild, Output, EventEmitter, Injector, ChangeDetectorRef } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ModalDirective } from 'ngx-bootstrap';
import { MdasServiceProxy, CreateOrEditMdaDto } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/operators';

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
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    mda: IMdaOnEdit = {};

    constructor(
        injector: Injector,
        private _mdaServiceProxy: MdasServiceProxy,
        private _changeDetector: ChangeDetectorRef
    ) {
        super(injector);
    }

    ngOnInit() {
    }

    onShown(): void {
        document.getElementById('mdaDisplayName').focus();
    }

    show(mdaUnit: IMdaOnEdit): void {
        this.mda = mdaUnit;
        this.active = true;
        this.modal.show();
        this._changeDetector.detectChanges();

    }

    save(): void {
        const createEditInput = new CreateOrEditMdaDto();
        createEditInput.id = this.mda.id;
        createEditInput.displayName = this.mda.displayName;

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

}
