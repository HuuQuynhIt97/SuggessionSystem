import { MeetingComponent } from './meeting/meeting.component';
import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { ObjectiveComponent } from './objective/objective.component';
import { TodolistComponent } from "./todolist/todolist.component";
import { UploadKpiComponent } from "./todolist/upload-kpi/upload-kpi.component";
import { Todolist2Component } from "./todolist2/todolist2.component";

const routes: Routes = [
  {
    path: '',
    data: {
      title: '',
      breadcrumb: ''
    },
    children: [
      {
        path: 'objective',
        component: ObjectiveComponent,
        data: {
          title: 'Objective',
          breadcrumb: 'KPI Objective',
          functionCode: 'objective'
        },
        // canActivate: [AuthGuard]
      },
      {
        path: 'todolist',
        component: TodolistComponent,
        data: {
          title: 'To Do List',
          breadcrumb: 'To Do List',
          functionCode: 'todolist'
        },
        // canActivate: [AuthGuard]
      },
      {
        path: 'todolist2',
        component: Todolist2Component,
        data: {
          title: 'To Do List',
          breadcrumb: 'To Do List',
          functionCode: 'todolist'
        },
        // canActivate: [AuthGuard]
      },
      {
        path: 'meeting',
        component: MeetingComponent,
        data: {
          title: 'Meeting',
          breadcrumb: 'Meeting',
          functionCode: 'meeting'
        },
        // canActivate: [AuthGuard]
      },
      {
        path: 'upload-kpi-objective',
        component: UploadKpiComponent,
        data: {
          title: 'Upload KPI Objective',
          breadcrumb: 'Upload KPI Objective',
          functionCode: 'todolist'
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
export class TransactionRoutingModule { }
