import { EnvService } from './env.service';
import { environment } from './../../../environments/environment';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';
@Injectable({
  providedIn: 'root'
})
export class TodolistService {
  baseUrl = environment.apiUrl;
  messageSource = new BehaviorSubject<number>(0);
  currentMessage = this.messageSource.asObservable();
  // method này để change source message
  changeMessage(message) {
    this.messageSource.next(message);
  }
  constructor(
    private http: HttpClient ,
    public env: EnvService) { }

  import(formData) {
    // const formData = new FormData();
    // formData.append('UploadedFile', file);
    // formData.append('CreatedBy', createdBy);
    return this.http.post(this.env.apiUrl + 'Todolist/Import', formData);
  }

  getAll(id) {
    return this.http.get(this.env.apiUrl + `ToDoList/GetAllToDoListByUserID/${id}`, {})
  }
  delete(id,file_name) {
    return this.http.delete(`${this.env.apiUrl}ToDoList/Delete/${id}/${file_name}`)
  }
  downloadExcel(url: string){
    return this.http.get(url,{responseType: 'blob'})
  }
  approve(ToDoListID,userID) {
    return this.http.get(this.env.apiUrl + `ToDoList/Approval/${ToDoListID}/${userID}`, {})
  }
  LoadTimeLine(ToDoListID) {
    return this.http.get(this.env.apiUrl + `ToDoList/LoadTimeLine/${ToDoListID}`, {})
  }
  reject(model) {
    return this.http.post(this.env.apiUrl + 'ToDoList/reject', model)
  }
}
