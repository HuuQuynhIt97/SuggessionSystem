import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
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
        path: 'todolist2',
        // component: Todolist2Component,
        data: {
          title: 'To Do List',
          breadcrumb: 'To Do List',
          functionCode: 'todolist'
        },
        children: [
          {
            path: '',
            component: Todolist2Component,
          },
          {
            path: ':tab',
            component: Todolist2Component,
          },
        ]
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
