import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { PriorityAreasComponent } from './priorityAreas/priorityAreas/priorityAreas.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { OrganizationUnitsComponent } from '@app/admin/organization-units/organization-units.component';
import { CreateEditPerformanceIndicatorComponent } from './indicators/createEditPerformanceIndicator/createEditPerformanceIndicator.component';

@NgModule({
    imports: [
        RouterModule.forChild([
            {
                path: '',
                children: [
                    { path: 'deliverables', component: OrganizationUnitsComponent, data: { permission: 'Pages.Deliverable' }  },
                    { path: 'deliverable/new', component: CreateEditPerformanceIndicatorComponent, data: { permission: 'Pages.PerformanceIndicator.Create'}},
                    { path: 'deliverable/edit', component: CreateEditPerformanceIndicatorComponent, data: { permission: 'Pages.PerformanceIndicator.Edit'}},
                    { path: 'priority-areas', component: PriorityAreasComponent, data: { permission: 'Pages.PriorityAreas' }  },
                    { path: 'dashboard', component: DashboardComponent, data: { permission: 'Pages.Tenant.Dashboard' } },
                    { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
                    { path: '**', redirectTo: 'dashboard' }
                ]
            }
        ])
    ],
    exports: [
        RouterModule
    ]
})
export class MainRoutingModule { }
