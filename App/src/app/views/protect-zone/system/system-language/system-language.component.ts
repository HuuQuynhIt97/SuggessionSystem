import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { GridComponent } from '@syncfusion/ej2-angular-grids';
import { BaseComponent } from 'src/app/_core/_component/base.component';
import { MessageConstants } from 'src/app/_core/_constants/system';
import { AlertifyService } from 'src/app/_core/_service/alertify.service';
import { SystemLanguageService } from 'src/app/_core/_service/systemLanguage.service';

@Component({
  selector: 'app-system-language',
  templateUrl: './system-language.component.html',
  styleUrls: ['./system-language.component.scss']
})
export class SystemLanguageComponent extends BaseComponent implements OnInit {

  role: any;
  data: any = [];
  @ViewChild('grid') grid: GridComponent;
  pageSettings = { pageCount: 20, pageSizes: true, pageSize: 20 };
  fieldsRoleType: object = { text: 'name', value: 'name' };
  filterSettings = { type: 'Excel' };
  roleTypeData: object;
  roleTypeID: any;
  roleItem: string;
  roleID: any;
  constructor(
    private systemLanguageService: SystemLanguageService,
    private alertify: AlertifyService,
    private route: ActivatedRoute,
  ) { super(); }

  ngOnInit() {
    this.getAll();
  }
  // api
  getAll() {
    this.systemLanguageService.getAll().subscribe(res => {
      this.data = res;
    });
  }

  create(model) {
    this.systemLanguageService.create(model).subscribe(() => {
      this.alertify.success(MessageConstants.CREATED_OK_MSG);
      this.getAll();
      this.role = {
      };
    });
  }

  update(model) {
    this.systemLanguageService.update(model).subscribe(() => {
      this.alertify.success(MessageConstants.UPDATED_OK_MSG);
      this.getAll();

    });
  }
  delete(id) {
    this.alertify.delete("Delete role",'Are you sure you want to delete this role "' + id + '" ?')
    .then((result) => {
      if (result) {
        this.systemLanguageService.delete(id).subscribe(() => {
          this.getAll();
          this.alertify.success(MessageConstants.DELETED_OK_MSG);
        }, error => {
          this.alertify.warning(MessageConstants.SYSTEM_ERROR_MSG);
        });
      }
    })
    .catch((err) => {
      this.getAll();
      this.grid.refresh();
    });
  }
  // end api
  // grid event
  toolbarClick(args): void {
    switch (args.item.text) {
      /* tslint:disable */
      case 'Excel Export':
        this.grid.excelExport();
        break;
      /* tslint:enable */
      default:
        break;
    }
  }

  actionBegin(args) {
    if (args.requestType === 'save') {
      if (args.action === 'add') {
        const model = {
          SlPage: args.data.slPage ?? null,
          SlKey: args.data.slKey,
          Sltw: args.data.sltw,
          Slen: args.data.slen
        }
        this.create(model);
      }
      if (args.action === 'edit') {

        const model = {
          Id: args.data.id,
          Slpage: args.data.slPage,
          Slkey: args.data.slKey,
          Sltw: args.data.sltw,
          Slen: args.data.slen
        }
        this.update(model);
      }
    }
    if (args.requestType === 'delete') {
      this.alertify.error('Can not delete this role', true);
      this.delete(args.data[0].id);
    }
  }
  actionComplete(e: any): void {
    if (e.requestType === 'add') {
      (e.form.elements.namedItem('slPage') as HTMLInputElement).focus();
      // (e.form.elements.namedItem('id') as HTMLInputElement).disabled = true;
    }
  }
  // end event
  NO(index) {
    return (this.grid.pageSettings.currentPage - 1) * this.pageSettings.pageSize + Number(index) + 1;
  }

}
