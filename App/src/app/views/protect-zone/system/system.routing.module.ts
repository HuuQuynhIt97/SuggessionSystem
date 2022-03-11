import { StatusComponent } from './status/status.component';
import { SystemLanguageComponent } from './system-language/system-language.component';
import { NgModule } from '@angular/core'
import { RouterModule, Routes } from '@angular/router'
import { AuthGuard } from 'src/app/_core/_guards/auth.guard'

import { OcUserComponent } from './oc-user/oc-user.component'
import { OcComponent } from './oc/oc.component'
import { AccountGroupComponent } from './account-group/account-group.component'
import { AccountComponent } from './account/account.component'

// import { PeriodComponent } from './period/period.component';
const routes: Routes = [
  {
    path: '',
    data: {
      title: '',
      breadcrumb: ''
    },
    children: [
      {
        path: 'account',
        component: AccountComponent,
        data: {
          title: 'Account',
          breadcrumb: 'Account',
          functionCode: 'account'
        },
        // canActivate: [AuthGuard]
      },
      {
        path: 'system-language',
        component: SystemLanguageComponent,
        data: {
          title: 'System Language',
          breadcrumb: 'System Language',
          functionCode: 'system-language'
        },
        // canActivate: [AuthGuard]
      },
      {
        path: 'account-group',
        component: AccountGroupComponent,
        data: {
          title: 'Account Group',
          breadcrumb: 'Account Group',
          functionCode: 'account-group'
        },
        // canActivate: [AuthGuard]
      },

      {
        path: 'status',
        component: StatusComponent,
        data: {
          title: 'Status',
          breadcrumb: 'Status',
          functionCode: 'status'
        },
        // canActivate: [AuthGuard]
      },



      {
        path: 'oc',
        component: OcComponent,
        data: {
          title: 'OC',
          breadcrumb: 'OC',
          functionCode: 'oc'
        },
        // canActivate: [AuthGuard]
      },
      {
        path: 'oc-user',
        component: OcUserComponent,
        data: {
          title: 'OC User',
          breadcrumb: 'OC User',
          functionCode: 'oc-user'
        },
        // canActivate: [AuthGuard]
      },


    ]
  },
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SystemRoutingModule { }
