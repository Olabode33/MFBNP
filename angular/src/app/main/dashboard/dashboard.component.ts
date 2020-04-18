import { Component, Injector, ViewEncapsulation, OnInit } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DashboardCustomizationConst } from '@app/shared/common/customizable-dashboard/DashboardCustomizationConsts';
import { SafeUrl, DomSanitizer } from '@angular/platform-browser';
import { TenantDashboardServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/operators';
import { appModuleAnimation } from '@shared/animations/routerTransition';

@Component({
    templateUrl: './dashboard.component.html',
    styleUrls: ['./dashboard.component.less'],
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})

export class DashboardComponent extends AppComponentBase implements OnInit {
    dashboardName = DashboardCustomizationConst.dashboardNames.defaultTenantDashboard;
    loading = false;
    powerBiUrl: SafeUrl;

    constructor(
        injector: Injector,
        private _sanitizer: DomSanitizer,
        private _tenantDashboardService: TenantDashboardServiceProxy
        ) {
        super(injector);
    }

    ngOnInit() {
        this.getDashboardUrl();
    }

    getDashboardUrl(): void {
        this.loading = true;
        this._tenantDashboardService.getPowerBiDashboardUrl()
            .pipe(finalize(() => { this.loading = false; }))
            .subscribe(result => {
                this.getSafeResourceUrl(result);
            });
    }

    getSafeResourceUrl(urlString: any)  {
        this.powerBiUrl = this._sanitizer.bypassSecurityTrustResourceUrl(urlString);
    }
}
