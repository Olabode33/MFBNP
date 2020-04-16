import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { PriorityAreasComponent } from './priorityAreas/priorityAreas/priorityAreas.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { OrganizationUnitsComponent } from '@app/admin/organization-units/organization-units.component';
import { CreateEditPerformanceIndicatorComponent } from './indicators/createEditPerformanceIndicator/createEditPerformanceIndicator.component';
import { ViewIndicatorComponent } from './indicators/view-indicator/view-indicator.component';
import { ViewActivityComponent } from './activity/view-activity/view-activity.component';

@NgModule({
    imports: [
        RouterModule.forChild([
            {
                path: '',
                children: [
                    { path: 'deliverables', component: OrganizationUnitsComponent, data: { permission: 'Pages.Deliverable' }  },
                    { path: 'deliverable/new/:unitId', component: CreateEditPerformanceIndicatorComponent, data: { permission: 'Pages.PerformanceIndicator.Create'}},
                    { path: 'deliverable/edit/:unitId/:indicatorId', component: CreateEditPerformanceIndicatorComponent, data: { permission: 'Pages.PerformanceIndicator.Edit'}},
                    { path: 'deliverable/view/:indicatorId', component: ViewIndicatorComponent, data: { permission: 'Pages.PerformanceIndicator'}},
                    { path: 'activity/view/:activityId', component: ViewActivityComponent, data: { permission: 'Pages.PerformanceActivity'}},
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
