import { EnvService } from './env.service';
import { AccountGroup } from './../_model/account.group';
import { CURDService } from './CURD.service';
import { Injectable } from '@angular/core';

import { HttpClient } from '@angular/common/http';
import { UtilitiesService } from './utilities.service';
import { Observable } from 'rxjs';
import { Status } from '../_model/status';

@Injectable({
  providedIn: 'root'
})
export class StatusService extends CURDService<Status> {

  constructor(http: HttpClient,utilitiesService: UtilitiesService, env: EnvService)
  {
    super(http,"Status", utilitiesService , env);
  }

}
