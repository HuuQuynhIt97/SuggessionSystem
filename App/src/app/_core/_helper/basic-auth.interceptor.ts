import { Injectable } from "@angular/core";
import {
  HttpRequest,
  HttpErrorResponse,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
} from "@angular/common/http";
import { Observable, throwError } from "rxjs";
import { catchError, retry } from "rxjs/operators";
import { AlertifyService } from "../_service/alertify.service";
import { NgxSpinnerService } from "ngx-spinner";

@Injectable()
export class BasicAuthInterceptor implements HttpInterceptor {
  constructor(private alertify: AlertifyService,
    private spinner: NgxSpinnerService,) {}

  intercept(
    request: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    // add authorization header with basic auth credentials if available
    if (localStorage.getItem("token")) {
      request = request.clone({
        setHeaders: {
          Authorization: `Bearer ${localStorage.getItem("token")}`,
        },
      });
    }

    return next.handle(request).pipe(
      retry(1),
      catchError((error: HttpErrorResponse) => {
        let errorMessage = "";
        if (error.error instanceof ErrorEvent) {
          // client-side error
          errorMessage = `Error: ${error.error.message}`;
        } else {
          // server-side error
          errorMessage = `Error Status: ${error.status}\nMessage: ${error.message}`;
          
          if (error.status === 0) {
            console.log(error.status);
            this.alertify.error(
              `Lỗi máy chủ vui lòng tải lại trang (nhấn F5) và chờ trong ít phút! <br> Server error!`,
              true
            );
            this.spinner.hide()
            return throwError(errorMessage);
          }
        }
        console.log(error.status);
        const mes = `Lỗi máy chủ vui lòng tải lại trang (nhấn F5) và chờ trong ít phút! <br> Server error!<br>${errorMessage}`;
        this.alertify.error(mes, true);
        this.spinner.hide()
        return throwError(errorMessage);
      })
    );
  }
}
