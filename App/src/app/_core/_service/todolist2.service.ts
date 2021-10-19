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
export class Todolist2Service  {
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
  l0(currentTime): Observable<any[]> {
    return this.http
      .get<any[]>(`${this.env.apiUrl}${this.entity}/L0?currentTime=${currentTime}`, {})
      .pipe(catchError(this.handleError));
  }
  submitUpdatePDCA(model): Observable<OperationResult> {
    return this.http.post<OperationResult>(`${this.env.apiUrl}${this.entity}/SubmitUpdatePDCA`, model);
  }
  submitAction(model): Observable<OperationResult> {
    return this.http.post<OperationResult>(`${this.env.apiUrl}${this.entity}/submitAction`, model);
  }
  submitKPINew(kpiId): Observable<OperationResult> {
    return this.http.post<OperationResult>(`${this.env.apiUrl}${this.entity}/SubmitKPINew?kpiId=${kpiId}`, {});
  }
  addOrUpdateStatus(request): Observable<OperationResult> {
    return this.http.post<OperationResult>(`${this.env.apiUrl}${this.entity}/AddOrUpdateStatus`, request);
  }
  getStatus(): Observable<any[]> {
    return this.http
      .get<any[]>(`${this.env.apiUrl}${this.entity}/getStatus`, {})
      .pipe(catchError(this.handleError));
  }
  getActionsForL0(kpiNewId): Observable<any> {
    return this.http
      .get<any>(`${this.env.apiUrl}${this.entity}/GetActionsForL0?kpiNewId=${kpiNewId}`, {})
      .pipe(catchError(this.handleError));
  }
  getPDCAForL0(kpiNewId,currentTime ): Observable<any> {
    return this.http
      .get<any>(`${this.env.apiUrl}${this.entity}/GetPDCAForL0?kpiNewId=${kpiNewId}&currentTime=${currentTime}`, {})
      .pipe(catchError(this.handleError));
  }

  getKPIForUpdatePDC(kpiNewId,currentTime ): Observable<any> {
    return this.http
      .get<any>(`${this.env.apiUrl}${this.entity}/GetKPIForUpdatePDC?kpiNewId=${kpiNewId}&currentTime=${currentTime}`, {})
      .pipe(catchError(this.handleError));
  }

  getTargetForUpdatePDCA(kpiNewId,currentTime ): Observable<any> {
    return this.http
      .get<any>(`${this.env.apiUrl}${this.entity}/GetTargetForUpdatePDCA?kpiNewId=${kpiNewId}&currentTime=${currentTime}`, {})
      .pipe(catchError(this.handleError));
  }

  getActionsForUpdatePDCA(kpiNewId,currentTime ): Observable<any> {
    return this.http
      .get<any>(`${this.env.apiUrl}${this.entity}/GetActionsForUpdatePDCA?kpiNewId=${kpiNewId}&currentTime=${currentTime}`, {})
      .pipe(catchError(this.handleError));
  }
  download(kpiId,uploadTime ) {
    return this.http
      .get(`${this.env.apiUrl}UploadFile/download?kpiId=${kpiId}&uploadTime=${uploadTime}`, { responseType: 'blob' })
      .pipe(catchError(this.handleError));
  }
  getAttackFiles(kpiId,uploadTime ) {
    return this.http
      .get(`${this.env.apiUrl}UploadFile/GetAttackFiles?kpiId=${kpiId}&uploadTime=${uploadTime}`)
      .pipe(catchError(this.handleError));
  }
  getDownloadFiles(kpiId,uploadTime ) {
    return this.http
      .get(`${this.env.apiUrl}UploadFile/GetDownloadFiles?kpiId=${kpiId}&uploadTime=${uploadTime}`)
      .pipe(catchError(this.handleError));
  }
  getDownloadFilesMeeting(kpiId,uploadTime ) {
    return this.http
      .get(`${this.env.apiUrl}UploadFile/GetDownloadFilesMeeting?kpiId=${kpiId}&uploadTime=${uploadTime}`)
      .pipe(catchError(this.handleError));
  }

  getDownloadFilesIdea(ideaID) {
    return this.http
      .get(`${this.env.apiUrl}UploadFile/GetDownloadFilesIdea?ideaID=${ideaID}`)
      .pipe(catchError(this.handleError));
  }
  importSubmit(formData) {
    // const formData = new FormData();
    // formData.append('UploadedFile', file);
    // formData.append('CreatedBy', createdBy);
    return this.http.post(this.env.apiUrl + 'Idea/ImportSubmit', formData);
  }
  importSave(formData) {
    // const formData = new FormData();
    // formData.append('UploadedFile', file);
    // formData.append('CreatedBy', createdBy);
    return this.http.post(this.env.apiUrl + 'Idea/ImportSave', formData);
  }
  getTabProposal() {
    return this.http.get(this.env.apiUrl + 'Idea/TabProposalGetAll', {});
  }
  getTabProcessing() {
    return this.http.get(this.env.apiUrl + 'Idea/TabProcessingGetAll', {});
  }
  getTabErick() {
    return this.http.get(this.env.apiUrl + 'Idea/TabErickGetAll', {});
  }
  getTabClose() {
    return this.http.get(this.env.apiUrl + 'Idea/TabCloseGetAll', {});
  }
  getIdeaHisById(id) {
    return this.http.get(this.env.apiUrl + `Idea/GetIdeaHisById/${id}`, {});
  }
  accept(formData) {
    return this.http.post(this.env.apiUrl + `Idea/Accept`, formData);
  }
  reject(model) {
    return this.http.post(this.env.apiUrl + `Idea/reject`, model);
  }
  update(model) {
    return this.http.post(this.env.apiUrl + `Idea/update`, model);
  }
  close(model) {
    return this.http.post(this.env.apiUrl + `Idea/Close`, model);
  }
  complete(model) {
    return this.http.post(this.env.apiUrl + `Idea/complete`, model);
  }
  terminate(model) {
    return this.http.post(this.env.apiUrl + `Idea/terminate`, model);
  }
  satisfied(model) {
    return this.http.post(this.env.apiUrl + `Idea/Satisfied`, model);
  }
  dissatisfied(model) {
    return this.http.post(this.env.apiUrl + `Idea/Dissatisfied`, model);
  }
  protected handleError(errorResponse: any) {
    if (errorResponse?.error?.message) {
        return throwError(errorResponse?.error?.message || 'Server error');
    }

    if (errorResponse?.error?.errors) {
        let modelStateErrors = '';

        // for now just concatenate the error descriptions, alternative we could simply pass the entire error response upstream
        for (const errorMsg of errorResponse?.error?.errors) {
            modelStateErrors += errorMsg + '<br/>';
        }
        return throwError(modelStateErrors || 'Server error');
    }
    return throwError('Server error');
}
}
