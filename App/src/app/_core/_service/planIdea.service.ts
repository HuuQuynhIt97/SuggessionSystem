import { PlanIdea } from './../_model/planIdea';
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
export class PlanIdeaService {

  messageSource = new BehaviorSubject<string>(null);
  messageUploadSource = new BehaviorSubject<boolean>(null);
  currentMessage = this.messageSource.asObservable();
  currentUploadMessage = this.messageUploadSource.asObservable();
  entity = 'PlanIdea';
  base = environment.apiUrl;
  // có thể subcribe theo dõi thay đổi value của biến này thay cho messageSource
  constructor(private http: HttpClient, public env: EnvService, utilitiesService: UtilitiesService) {
  }

  changeMessage(message) {
    this.messageSource.next(message);
  }
  changeUploadMessage(message) {
    this.messageUploadSource.next(message);
  }

  submitPlanIdea(id) {
    return this.http.post(this.env.apiUrl + `PlanIdea/SubmitPlanIdea/${id}`, {});
  }
  erickApproval(model) {
    return this.http.post(this.env.apiUrl + `Idea/ErickApproval/`, model);
  }

  erickReject(model) {
    return this.http.post(this.env.apiUrl + `Idea/ErickReject/`, model);
  }

}
