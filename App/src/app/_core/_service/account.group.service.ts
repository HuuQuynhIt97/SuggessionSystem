import { EnvService } from './env.service';
import { AccountGroup } from './../_model/account.group';
import { CURDService } from './CURD.service';
import { Injectable } from '@angular/core';

import { HttpClient } from '@angular/common/http';
import { UtilitiesService } from './utilities.service';
import { Observable } from 'rxjs';
@Injectable({
  providedIn: 'root'
})
export class AccountGroupService extends CURDService<AccountGroup> {

  constructor(http: HttpClient,utilitiesService: UtilitiesService, env: EnvService)
  {
    super(http,"AccountGroup", utilitiesService , env);
  }
  getAccountGroupForTodolistByAccountId(): Observable<any[]> {
    return this.http.get<any[]>(`${this.env.apiUrl}${this.entity}/GetAccountGroupForTodolistByAccountId`);
  }
  getAllTab() {
    return this.http.get<any[]>(`${this.env.apiUrl}Tab/GetAll`);
  }
}
