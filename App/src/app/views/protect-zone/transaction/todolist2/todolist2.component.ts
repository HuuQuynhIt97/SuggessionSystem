import { StatusName } from './../../../../_core/enum/JobType';
import { filter } from 'rxjs/operators';
import { NgxSpinnerService } from 'ngx-spinner';
import { Account2Service } from 'src/app/_core/_service/account2.service';
import { EnvService } from './../../../../_core/_service/env.service';
import { PdcaComponent } from './pdca/pdca.component';
import { PlanComponent } from './plan/plan.component';
import { PerformanceService } from './../../../../_core/_service/performance.service';
import { Subscription } from 'rxjs';
import { AccountGroupService } from './../../../../_core/_service/account.group.service';
import { Component, OnInit, TemplateRef, ViewChild, QueryList, ViewChildren, OnDestroy, ElementRef } from '@angular/core';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { GridComponent } from '@syncfusion/ej2-angular-grids';
import { ObjectiveService } from 'src/app/_core/_service/objective.service';
import { AccountGroup } from 'src/app/_core/_model/account.group';
import { Todolistv2Service } from 'src/app/_core/_service/todolistv2.service';
import { PeriodType, SystemRole, ToDoListType, SystemScoreType } from 'src/app/_core/enum/system';
import { environment } from 'src/environments/environment';
import { AlertifyService } from 'src/app/_core/_service/alertify.service';
import { Performance } from 'src/app/_core/_model/performance';
import { DatePipe } from '@angular/common';
import { SpreadsheetComponent } from '@syncfusion/ej2-angular-spreadsheet';
import { MessageConstants } from 'src/app/_core/_constants/system';
import { NgTemplateNameDirective } from '../ng-template-name.directive';
import { Router } from '@angular/router';
import { Todolist2Service } from 'src/app/_core/_service/todolist2.service';
import { UploadFileComponent } from './upload-file/upload-file.component';
import { StatusCode } from 'src/app/_core/enum/JobType';
@Component({
  selector: 'app-todolist2',
  templateUrl: './todolist2.component.html',
  styleUrls: ['./todolist2.component.scss'],
  providers: [DatePipe]
})
export class Todolist2Component implements OnInit, OnDestroy {
  editSettings = { showDeleteConfirmDialog: false, allowEditing: false, allowAdding: false, allowDeleting: false, mode: 'Normal' };
  editSettingsDisPatch = { showDeleteConfirmDialog: false, allowEditing: true, allowAdding: false, allowDeleting: false, mode: 'Normal' };
  data: any[] = [];
  dataHis: any[] = [];
  password = '';
  modalReference: NgbModalRef;
  fields: object = { text: 'name', value: 'id' };
  toolbarOptions = ['Search'];
  wrapSettings= { wrapMode: 'Content' };
  pageSettings = { pageCount: 20, pageSizes: true, pageSize: 10 };
  @ViewChild('grid') public grid: GridComponent;
  @ViewChild('gridBuying') public gridBuying: GridComponent;
  @ViewChild('gridComplete') public gridComplete: GridComponent;
  locale = localStorage.getItem('lang');
  totalPrice = 0;
  name: any;
  fullName: any;
  dataPicked = [];
  dataPickedDitchPatch = [];
  dataAdd: any
  check: boolean = true
  dataBuyingAdd: any
  public enableVirtualization: boolean = true;
  pendingTab: boolean =  false;
  buyingTab: boolean = true;
  completeTab: boolean = false;
  dispatchData: any
  @ViewChild('suggession', { static: true })
  public suggession: TemplateRef<any>;
  @ViewChild('fileModal', { static: true })
  public fileModal: TemplateRef<any>;
  @ViewChild('suggessionReturn', { static: true })
  public suggessionReturn: TemplateRef<any>;
  @ViewChild('details', { static: true })
  public details: TemplateRef<any>;
  @ViewChild('tabProcess', { static: true })
  public tabProcess: TemplateRef<any>;

  @ViewChild('completeTerminateModel', { static: true })
  public completeTerminateModel: TemplateRef<any>;

  @ViewChild('satisfiedCloseModel', { static: true })
  public satisfiedCloseModel: TemplateRef<any>;

  @ViewChild('rejectModel', { static: true })
  public rejectModel: TemplateRef<any>;

  @ViewChild('dissatisfiedModel', { static: true })
  public dissatisfiedModel: TemplateRef<any>;

  @ViewChild('gridDisPatch')
  gridDisPatch: GridComponent;
  ModelCreate: { productId: number; consumerId: number; qtyTamp: number; };
  TabClass: any = "btn btn-primary"
  buyingTabClass: any = "btn btn-success"
  completeTabClass: any = "btn btn-default"
  base = environment.apiUrl
  noImage = '/assets/img/photo1.png';
  img: any
  dataFake: any
  databuyingPersion: any
  startDate = new Date();
  endDate = new Date();
  teamId: any

  teamData: any[];
  teamIdStore: string;
  tabData: any[];
  file: any = [];
  filesLeft = [];
  filesRight = [];
  pondOptions = {
    class: 'my-filepond',
    multiple: true,
    // acceptedFileTypes: 'application/vnd.ms-excel, application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
    server: {
      process: this.env.apiUrl + 'todolist/SaveFile',
      revert: null
    }
  }
  dataUser: import("f:/Angular/SYSTEM/SuggessionSystem/App/src/app/_core/_model/objective").Objective[];
  userFields: object = { text: 'fullName', value: 'id' };
  userId: number = 0
  toId: number = 0 ;
  titleText: string
  issueText: string
  commentText: string
  suggessionText: string
  proposal: boolean = true
  files: any;
  ideaId: any;
  nameTitle: any;
  issueTitle: any;
  accountGroupId: number[];
  accountGroupText: any;
  createdBy: any;
  toggle = true;
  status = 'Enable';
  statusName: any;
  constructor(
    private service: ObjectiveService,
    private router: Router,
    private alertify: AlertifyService,
    public todolistService: Todolistv2Service,
    public todolist2Service: Todolist2Service,
    private accountService: Account2Service,
    private accountGroupService: AccountGroupService,
    public modalService: NgbModal,
    private datePipe: DatePipe,
    private performanceService: PerformanceService,
    public env: EnvService,
    private spinner: NgxSpinnerService
  ) {
  }
  ngOnDestroy(): void {
  }
  changeTab(item) {
    this.spinner.show()
    this.tabData.forEach(element => {
      element.name == item.name ? element.statues = true : element.statues =false
    });
    switch (item.name) {
      case StatusCode.Proposal:
        this.getTabProposal()
        this.proposal = true
        break;
      case StatusCode.Processing:
        this.getTabProcessing()
        this.proposal = false
        break;
      case StatusCode.Erick:
        this.getTabErick()
        this.proposal = false
        break;
      case StatusCode.Close:
        this.getTabClose()
        this.proposal = false
        break;
      default:
        break;
    }
  }
  ngOnInit(): void {
    this.proposal = true
    this.accountGroupText = JSON.parse(localStorage.getItem('user')).accountGroupText;
    this.getAllTab();
    this.spinner.show()
    this.getTabProposal()
    if (localStorage.getItem('user') !== null) {
      this.userId = Number(JSON.parse(localStorage.getItem('user')).id);
      console.log(this.userId);
    }
  }
  enableDisableRule() {
    this.toggle = !this.toggle;
    this.status = this.toggle ? 'Enable' : 'Disable';
  }
  detail(item) {
    console.log(item);
    this.ideaId = item.id
    this.createdBy = item.createdBy
    this.nameTitle = item.name
    this.issueTitle = item.title
    this.statusName = item.statusName
    switch (item.statusName) {
      case StatusName.Apply:
        this.showModalDetails(this.details)
        break;
      case StatusName.Update:
        this.showModalDetails(this.tabProcess)
        break;
      case StatusName.Reject:
        this.showModalDetails(this.rejectModel)
        break;
      case StatusName.Complete:
        this.showModalDetails(this.completeTerminateModel)
        break;
      case StatusName.Terminate:
        this.showModalDetails(this.completeTerminateModel)
        break;
      case StatusName.Dissatisfied:
        this.showModalDetails(this.dissatisfiedModel)
        break;
      case StatusName.Satisfied:
        this.showModalDetails(this.satisfiedCloseModel)
        break;
      case StatusName.Close:
        this.showModalDetails(this.satisfiedCloseModel)
        break;
      default:
        break;
    }
    this.getIdeaHisById(item.id)
  }
  accept() {
    const formData = new FormData();
    formData.append("IdeaID", this.ideaId.toString());
    formData.append("CreatedBy", this.userId.toString());
    formData.append("Comment", this.commentText);
    for (let item of this.file) {
      formData.append('UploadedFile', item);
    }
    this.todolist2Service.accept(formData).subscribe(res => {
      this.alertify.success("Accept Successfully")
      this.getTabProposal()
      this.file = []
      this.modalReference.close()
    })
  }
  update() {
    const formData = new FormData();
    formData.append("IdeaID", this.ideaId.toString());
    formData.append("CreatedBy", this.userId.toString());
    formData.append("Comment", this.commentText);
    for (let item of this.file) {
      formData.append('UploadedFile', item);
    }
    this.todolist2Service.update(formData).subscribe(res => {
      this.alertify.success("Update Successfully")
      this.modalReference.close()
      this.file = []
      this.getTabProcessing()
    })
  }

  asign() {
    const formData = new FormData();
    formData.append("IdeaID", this.ideaId.toString());
    formData.append("CreatedBy", this.userId.toString());
    formData.append("Comment", this.commentText);
    for (let item of this.file) {
      formData.append('UploadedFile', item);
    }
    this.todolist2Service.update(formData).subscribe(res => {
      this.alertify.success("Successfully")
      this.modalReference.close()
      this.file = []
      this.getTabErick()
    })
  }
  closeIdea() {
    const formData = new FormData();
    formData.append("IdeaID", this.ideaId.toString());
    formData.append("CreatedBy", this.userId.toString());
    formData.append("Comment", this.commentText);
    for (let item of this.file) {
      formData.append('UploadedFile', item);
    }
    this.todolist2Service.close(formData).subscribe(res => {
      this.alertify.success("Update Successfully")
      this.modalReference.close()
      this.file = []
      this.getTabErick()
    })
  }
  terminate() {
    const formData = new FormData();
    formData.append("IdeaID", this.ideaId.toString());
    formData.append("CreatedBy", this.userId.toString());
    formData.append("Comment", this.commentText);
    for (let item of this.file) {
      formData.append('UploadedFile', item);
    }
    this.todolist2Service.terminate(formData).subscribe(res => {
      this.alertify.success("Successfully")
      this.file = []
      this.getTabProcessing()
      this.modalReference.close()
    })
  }
  complete() {
    const formData = new FormData();
    formData.append("IdeaID", this.ideaId.toString());
    formData.append("CreatedBy", this.userId.toString());
    formData.append("Comment", this.commentText);
    for (let item of this.file) {
      formData.append('UploadedFile', item);
    }
    this.todolist2Service.complete(formData).subscribe(res => {
      this.alertify.success("Successfully")
      this.file = []
      this.getTabProcessing()
      this.modalReference.close()
    })
  }
  reject() {
    const formData = new FormData();
    formData.append("IdeaID", this.ideaId.toString());
    formData.append("CreatedBy", this.userId.toString());
    formData.append("Comment", this.commentText);
    for (let item of this.file) {
      formData.append('UploadedFile', item);
    }
    this.todolist2Service.reject(formData).subscribe(res => {
      this.alertify.success("Successfully")
      this.file = []
      this.getTabProposal()
      this.modalReference.close()
    })
  }

  satisfied() {
    const formData = new FormData();
    formData.append("IdeaID", this.ideaId.toString());
    formData.append("CreatedBy", this.userId.toString());
    formData.append("Comment", this.commentText);
    for (let item of this.file) {
      formData.append('UploadedFile', item);
    }
    this.todolist2Service.satisfied(formData).subscribe(res => {
      this.alertify.success("Successfully")
      this.file = []
      this.getTabProposal()
      this.modalReference.close()
    })
  }

  dissatisfied() {
    const formData = new FormData();
    formData.append("IdeaID", this.ideaId.toString());
    formData.append("CreatedBy", this.userId.toString());
    formData.append("Comment", this.commentText);
    for (let item of this.file) {
      formData.append('UploadedFile', item);
    }
    this.todolist2Service.dissatisfied(formData).subscribe(res => {
      this.alertify.success("Successfully")
      this.file = []
      this.getTabProposal()
      this.modalReference.close()
    })
  }

  attackFile(data){
    this.showModal(this.fileModal)
    this.todolist2Service.getDownloadFilesIdea(data.id).subscribe((res: any) => {
      const files = res as any || [];
      this.files = files.map(x=> {
        return {
          name: x.name,
          path: this.env.fileUrl + x.path
        }
      });
    })
  }
  getIdeaHisById(id) {
    this.todolist2Service.getIdeaHisById(id).subscribe((res: any) => {
      this.dataHis = res
      console.log(res);
    })
  }
  getTabProcessing() {
    this.todolist2Service.getTabProcessing().subscribe((res: any) => {
      console.log('getTabProcessing',res);
      this.spinner.hide()
      this.data = res
    })
  }
  getTabErick() {
    this.todolist2Service.getTabErick().subscribe((res: any) => {
      console.log('getTabErick',res);
      this.spinner.hide()
      this.data = res
    })
  }
  getTabClose() {
    this.todolist2Service.getTabClose().subscribe((res: any) => {
      console.log('getTabClose',res);
      this.spinner.hide()
      this.data = res
    })
  }
  getTabProposal() {
    this.todolist2Service.getTabProposal().subscribe((res: any) => {
      console.log('getTabProposal',res);
      this.data = res
      this.spinner.hide()
    })
  }
  save() {
    const formData = new FormData();
    formData.append("SendID", this.userId.toString());
    formData.append("ReceiveID", this.toId.toString());
    formData.append("Title", this.titleText);
    formData.append("Suggession", this.suggessionText);
    formData.append("Issue", this.issueText);
    // formData.append("topic", this.Topic_Name);
    for (let item of this.file) {
      formData.append('UploadedFile', item);
    }
    if(this.file) {
      this.todolist2Service.importSave(formData).subscribe((res: any) => {
        this.alertify.success('Upload Success!')
        this.getTabProposal()
        this.refreshText()
        this.modalReference.close();
        // this.Topic_Name = null;
      })

    } else {
      this.alertify.error('Not File Upload!')
    }
  }
  refreshText() {
    this.toId = 0
    this.titleText = null
    this.suggessionText = null
    this.issueText = null
    this.file = []
  }
  submit() {
    const formData = new FormData();
    formData.append("SendID", this.userId.toString());
    formData.append("ReceiveID", this.toId.toString());
    formData.append("Title", this.titleText);
    formData.append("Suggession", this.suggessionText);
    formData.append("Issue", this.issueText);
    // formData.append("topic", this.Topic_Name);
    for (let item of this.file) {
      formData.append('UploadedFile', item);
    }
    if(this.file) {
      this.todolist2Service.importSubmit(formData).subscribe((res: any) => {
        this.alertify.success('Upload Success!')
        this.getTabProposal()
        this.refreshText()
        this.modalReference.close();
        // this.Topic_Name = null;
      })

    } else {
      this.alertify.error('Not File Upload!')
    }
  }
  openUploadModalComponent() {
    const modalRef = this.modalService.open(UploadFileComponent, { size: 'md', backdrop: 'static', keyboard: false });
    modalRef.componentInstance.data = this.data;
    modalRef.result.then((result) => {
    }, (reason) => {
    });
  }
  loadData() {
    this.accountService.getAll().subscribe((data: any) => {
      this.dataUser = data.filter(x => x.accountGroupText === StatusCode.Spokesman);
    });
  }
  close() {
    this.file = [] ;
    this.modalReference.close();
  }
  handleFileProcess(event: any){
    this.file.push(event.file.file) ;
  }
  pondHandleAddFile(event: any) {
  }

  pondHandleRemoveFile(event: any) {
    for(var i = 0; i < this.file.length; i++) {
      if(this.file[i].name == event.file.file.name) {
        this.file.splice(i, 1);
        break;
      }
    }

  }
  openSuggessionModal() {
    this.showModal(this.suggession)
    this.loadData()
  }
  showModal(modal){
    this.modalReference = this.modalService.open(modal, { size: 'sm'});
  }
  showModalDetails(modal){
    this.modalReference = this.modalService.open(modal, { size: 'xxl'});
  }
  Upload() {

    const formData = new FormData();
    // formData.append("uploadBy", this.userID);
    // formData.append("file_code", this.makeid(8));
    // formData.append("topic", this.Topic_Name);
    for (let item of this.file) {
      formData.append('UploadedFile', item);
    }
    if(this.file) {
      // this.todolistService.import(formData).subscribe((res: any) => {
      //   this.alertify.success('Upload Success!')
      //   this.getAllDataByUser();
      //   this.modalReference.close();
      //   // this.Topic_Name = null;
      // })

    } else {
      this.alertify.error('Not File Upload!')
    }
  }
  search(args) {
    if(this.pendingTab)
      this.grid.search(this.name);
    if(this.buyingTab)
      this.gridBuying.search(this.name);
    if(this.completeTab)
      this.gridComplete.search(this.name);
  }
  getAllTab(){
    this.accountGroupService.getAllTab().subscribe(res => {

      if(this.accountGroupText === StatusCode.Spokesman) {
        this.tabData = res.filter(x => x.name !== StatusCode.Erick)
      }
      if(this.accountGroupText === StatusCode.Propersal) {
        this.tabData = res.filter(x => x.name !== StatusCode.Processing && x.name !== StatusCode.Erick)
      }

      if(this.accountGroupText === StatusCode.Erick) {
        this.tabData = res
      }

    })
  }
  NO(index) {
    return (this.grid.pageSettings.currentPage - 1) * this.pageSettings.pageSize + Number(index) + 1;
  }
}
