import { Routes } from '@angular/router';
import { MainLayoutComponent } from './core/layout/main-layout/main-layout.component';
import { DashboardComponent } from './features/dashboard/dashboard.component';
import { LoginComponent } from './features/login/login.component';
import { PaymentsComponent } from './features/payments/payments.component';
import { BatchComponent } from './features/batch/batch.component';
import { UsersComponent } from './features/users/users.component';
import { ReportsComponent } from './features/reports/reports.component';
import { SettingsComponent } from './features/settings/settings.component';
import { RegisterComponent } from './features/register/register.component';
import { ProfileComponent } from './features/profile/profile.component';
import { LogsComponent } from './features/logs/logs.component';
import { DocsComponent } from './features/docs/docs.component';
import { authGuard } from './core/guards/auth.guard';

export const routes: Routes = [
    // Public routes
    { path: 'login', component: LoginComponent },
    { path: 'register', component: RegisterComponent },

    // Protected routes inside the main layout
    {
        path: '',
        component: MainLayoutComponent,
        canActivate: [authGuard],
        children: [
            { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
            { path: 'dashboard', component: DashboardComponent },
            { path: 'payments', component: PaymentsComponent },
            { path: 'batch', component: BatchComponent },
            { path: 'users', component: UsersComponent },
            { path: 'reports', component: ReportsComponent },
            { path: 'settings', component: SettingsComponent },
            { path: 'profile', component: ProfileComponent },
            { path: 'logs', component: LogsComponent },
            { path: 'docs', component: DocsComponent }
        ]
    },

    // Catch-all
    { path: '**', redirectTo: 'login' }
];
