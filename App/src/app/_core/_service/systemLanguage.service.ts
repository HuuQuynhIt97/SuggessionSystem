import { EnvService } from './env.service';
import { environment } from 'src/environments/environment';
import { CURDService } from './CURD.service';
import { Injectable } from '@angular/core';

import { UtilitiesService } from './utilities.service';
import { SelfScore, ToDoList, ToDoListByLevelL1L2Dto, ToDoListL1L2, ToDoListOfQuarter } from '../_model/todolistv2';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Objective } from '../_model/objective';
import { OperationResult } from '../_model/operation.result';

@Injectable({
  providedIn: 'root'
})
export class SystemLanguageService {

  messageSource = new BehaviorSubject<boolean>(null);
  messageUploadSource = new BehaviorSubject<boolean>(null);
  currentMessage = this.messageSource.asObservable();
  currentUploadMessage = this.messageUploadSource.asObservable();
  entity = 'Todolist2';
  base = environment.apiUrl;
  // có thể subcribe theo dõi thay đổi value của biến này thay cho messageSource
  constructor(private http: HttpClient, public env: EnvService, utilitiesService: UtilitiesService) {
  }
  // method này để change source message
  changeMessage(message) {
    this.messageSource.next(message);
  }
  changeUploadMessage(message) {
    this.messageUploadSource.next(message);
  }

  getAll() {
    return this.http.get<any>(`${this.env.apiUrl}SystemLanguage/GetAll`, {});
  }
  create(model) {
    return this.http.post(this.env.apiUrl + 'SystemLanguage/create', model);
  }
  update(model) {
    return this.http.put(this.env.apiUrl + 'SystemLanguage/Update', model);
  }
  delete(id: number) {
    return this.http.delete(this.env.apiUrl + 'SystemLanguage/Delete/' + id);
  }
  getLanguages(lang) {
    return this.http.get<any>(`${this.env.apiUrl}SystemLanguage/GetLanguages/${lang}`, {});
  }

}
