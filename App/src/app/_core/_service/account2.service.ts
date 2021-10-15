import { Injectable } from '@angular/core'
import { Observable } from 'rxjs'
import { HttpClient } from '@angular/common/http'
import { catchError } from 'rxjs/operators'

import { CURDService } from './CURD.service'
import { Account } from '../_model/account'
import { UtilitiesService } from './utilities.service'
import { OperationResult } from '../_model/operation.result'
import { EnvService } from './env.service'

@Injectable({
  providedIn: 'root'
})
export class Account2Service extends CURDService<Account> {

  constructor(
      http: HttpClient,
      utilitiesService: UtilitiesService,
      env: EnvService
    )
  {
    super(http,"Account", utilitiesService,env);
  }
  lock(id): Observable<OperationResult> {
    return this.http.put<OperationResult>(`${this.env.apiUrl}Account/lock?id=${id}`, {}).pipe(
      catchError(this.handleError)
    );
  }
  getAccounts(): Observable<any[]> {
    return this.http.get<any[]>(`${this.env.apiUrl}Account/GetAccounts`);
  }
  getAllRole() {
    return this.http.get<any[]>(`${this.env.apiUrl}Role/GetAll`);
  }

  getAllAccount() {
    return this.http.get(this.env.apiUrl + 'Account/GetAll', {});
  }
  deleteUser(id) {
    return this.http.delete(`${this.env.apiUrl}Account/Delete/${id}`);
  }
  updateUser(update) { return this.http.put(`${this.env.apiUrl}Account/Update`, update); }

  createUser(create) { return this.http.post(`${this.env.apiUrl}Account/Add`, create); }

}
