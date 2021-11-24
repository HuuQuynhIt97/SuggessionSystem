import { UploadFileComponent } from './todolist2/upload-file/upload-file.component';
import { DatePickerModule } from '@syncfusion/ej2-angular-calendars';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TranslateModule, TranslateLoader, TranslateService } from '@ngx-translate/core';
import { DateInputsModule } from '@progress/kendo-angular-dateinputs';
import { CheckBoxAllModule, SwitchModule } from '@syncfusion/ej2-angular-buttons';
import { DropDownListModule,  MultiSelectAllModule } from '@syncfusion/ej2-angular-dropdowns';
import { GridAllModule } from '@syncfusion/ej2-angular-grids';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { L10n, loadCldr, setCulture } from '@syncfusion/ej2-base';

import { HttpClient } from '@angular/common/http';
import { TransactionRoutingModule } from './transaction.routing.module';
import { TabModule, ToolbarModule } from '@syncfusion/ej2-angular-navigations';

import { TreeGridAllModule } from '@syncfusion/ej2-angular-treegrid';

import { SpreadsheetAllModule } from '@syncfusion/ej2-angular-spreadsheet';
import { NgTemplateNameDirective } from './ng-template-name.directive';
import { Todolist2Component } from './todolist2/todolist2.component';
import { UploaderModule } from '@syncfusion/ej2-angular-inputs';
import { NgxSpinnerModule } from 'ngx-spinner';
import { TooltipModule } from '@syncfusion/ej2-angular-popups';
export function HttpLoaderFactory(http: HttpClient) {
  return new TranslateHttpLoader(http, './assets/i18n/', '.json');
}
import { FilePondModule, registerPlugin } from 'ngx-filepond';
import * as FilePondPluginFileValidateType from 'filepond-plugin-file-validate-type';
import * as FilePondPluginImagePreview from 'filepond-plugin-image-preview';
import * as FilePondPluginFileValidateSize from 'filepond-plugin-file-validate-size';
registerPlugin(FilePondPluginFileValidateType,FilePondPluginFileValidateSize,FilePondPluginImagePreview);
import  { NgxDocViewerModule } from 'ngx-doc-viewer';
import { TextareaAutosizeModule } from 'ngx-textarea-autosize';

declare var require: any;
let defaultLang: string;
const lang = localStorage.getItem('lang');
loadCldr(
  require('cldr-data/supplemental/numberingSystems.json'),
  require('cldr-data/main/en/ca-gregorian.json'),
  require('cldr-data/main/en/numbers.json'),
  require('cldr-data/main/en/timeZoneNames.json'),
  require('cldr-data/supplemental/weekdata.json')); // To load the culture based first day of week

loadCldr(
  require('cldr-data/supplemental/numberingSystems.json'),
  require('cldr-data/main/vi/ca-gregorian.json'),
  require('cldr-data/main/vi/numbers.json'),
  require('cldr-data/main/vi/timeZoneNames.json'),
  require('cldr-data/supplemental/weekdata.json')); // To load the culture based first day of week


  loadCldr(
    require('cldr-data/supplemental/numberingSystems.json'),
    require('cldr-data/main/zh/ca-gregorian.json'),
    require('cldr-data/main/zh/numbers.json'),
    require('cldr-data/main/zh/timeZoneNames.json'),
    require('cldr-data/supplemental/weekdata.json')); // To load the culture based first day of week

    if (lang === 'vi') {
      defaultLang = lang;
    } else if (lang === 'en') {
      defaultLang = 'en';
    } else if (lang === 'zh'){
      defaultLang = 'zh';
    }
@NgModule({
  declarations: [
    Todolist2Component,
    NgTemplateNameDirective,
    UploadFileComponent
  ],
  imports: [
    NgxDocViewerModule,
    FilePondModule,
    TextareaAutosizeModule,
    CommonModule,
    TooltipModule,
    FormsModule,
    ReactiveFormsModule,
    DropDownListModule,
    GridAllModule,
    TreeGridAllModule,
    CheckBoxAllModule,
    SwitchModule,
    TransactionRoutingModule,
    DateInputsModule ,
    ToolbarModule,
    MultiSelectAllModule,
    DatePickerModule,
    TabModule,
    SpreadsheetAllModule,
    UploaderModule,
    NgxSpinnerModule,
    TranslateModule.forChild({
      loader: {
        provide: TranslateLoader,
        useFactory: HttpLoaderFactory,
        deps: [HttpClient]
      },
      defaultLanguage: defaultLang
    }),
  ]
})
export class TransactionModule{
  vi: any;
  en: any;
  constructor(public translate: TranslateService) {
    translate.addLangs(['en', 'zh', 'vi']);
    if (lang === 'vi') {
      defaultLang = 'vi';
      setTimeout(() => {
        L10n.load(require('../../../../assets/ej2-lang/vi.json'));
        setCulture('vi');
      });
    } else if (lang === 'en') {
      defaultLang = 'en';
      setTimeout(() => {
        L10n.load(require('../../../../assets/ej2-lang/en.json'));
        setCulture('en');
      });
    }else{
      defaultLang = 'zh';
      setTimeout(() => {
        L10n.load(require('../../../../assets/ej2-lang/zh.json'));
        setCulture('zh');
      });
    }
    translate.use(defaultLang);
  }
}

