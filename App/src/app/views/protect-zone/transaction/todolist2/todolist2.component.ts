import { filter } from 'rxjs/operators';
import { AnnouncementModalComponent } from './announcementModal/announcementModal.component';
import { SatisfiedErickModalComponent } from './satisfiedErickModal/satisfiedErickModal.component';
import { SatisfiedSpokermanModalComponent } from './satisfiedSpokermanModal/satisfiedSpokermanModal.component';
import { StatusName } from './../../../../_core/enum/JobType';
import { NgxSpinnerService } from 'ngx-spinner';
import { Account2Service } from 'src/app/_core/_service/account2.service';
import { EnvService } from './../../../../_core/_service/env.service';
import { PerformanceService } from './../../../../_core/_service/performance.service';
import { AccountGroupService } from './../../../../_core/_service/account.group.service';
import { Component, OnInit, TemplateRef, ViewChild, QueryList, ViewChildren, OnDestroy, ElementRef } from '@angular/core';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { GridComponent, QueryCellInfoEventArgs, RowDataBoundEventArgs } from '@syncfusion/ej2-angular-grids';
import { ObjectiveService } from 'src/app/_core/_service/objective.service';
import { Todolistv2Service } from 'src/app/_core/_service/todolistv2.service';
import { environment } from 'src/environments/environment';
import { AlertifyService } from 'src/app/_core/_service/alertify.service';
import { DatePipe } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { Todolist2Service } from 'src/app/_core/_service/todolist2.service';
import { UploadFileComponent } from './upload-file/upload-file.component';
import { StatusCode } from 'src/app/_core/enum/JobType';
import { TranslateService } from '@ngx-translate/core';
import { Tooltip } from '@syncfusion/ej2-angular-popups';
import { PlanIdeaService } from 'src/app/_core/_service/planIdea.service';
@Component({
  selector: 'app-todolist2',
  templateUrl: './todolist2.component.html',
  styleUrls: ['./todolist2.component.scss'],
  providers: [DatePipe]
})
export class Todolist2Component implements OnInit, OnDestroy {
  editSettings = { showDeleteConfirmDialog: false, allowEditing: true, allowAdding: true, allowDeleting: true, mode: 'Normal' };
  editSettingsDisPatch = { showDeleteConfirmDialog: false, allowEditing: true, allowAdding: false, allowDeleting: false, mode: 'Normal' };
  data: any[] = [];
  dataHis: any[] = [];
  modalReference: NgbModalRef;
  fields: object = { text: 'name', value: 'id' };
  toolbarOptions = ['Add','Update','Delete','Cancel'];
  wrapSettings= { wrapMode: 'Content' };
  pageSettings = { pageCount: 20, pageSizes: true, pageSize: 10 };
  pageSettingsTodo = {
    pageCount: 10,
    pageSizes: [5, 10,15,20, 'All'],
    pageSize: 10,
  };
  @ViewChild('grid') public grid: GridComponent;

  @ViewChild('gridToDo') public gridToDo: GridComponent;
  locale: string = null ;
  name: any;
  public enableVirtualization: boolean = true;

  @ViewChild('suggession', { static: true })
  public suggession: TemplateRef<any>;

  @ViewChild('suggessionEdit', { static: true })
  public suggessionEdit: TemplateRef<any>;

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

  @ViewChild('satisfiedCloseSpokerManModel', { static: true })
  public satisfiedCloseSpokerManModel: TemplateRef<any>;

  @ViewChild('rejectModel', { static: true })
  public rejectModel: TemplateRef<any>;

  @ViewChild('dissatisfiedModel', { static: true })
  public dissatisfiedModel: TemplateRef<any>;

  @ViewChild('gridDisPatch')
  gridDisPatch: GridComponent;

  @ViewChild('gridCompleteTerminate')
  gridCompleteTerminate: GridComponent;
  base = environment.apiUrl.replace('/api/', '');
  noImage = '/assets/img/photo1.png';
  img: any
  tabData: any[] = [];
  file: any = [];
  filesLeft = [];
  filesRight = [];

  pondOptionsEN = {
    class: 'my-filepond',
    multiple: true,
    labelIdle: "Drop files here or <span class='filepond--label-action'>Browse</span>",
    // acceptedFileTypes: 'application/vnd.ms-excel, application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
    server: {
      url: this.env.apiUrl,
      process: 'Idea/SaveFile',
      revert: null,
    }
  }
  pondOptionsZH = {
    class: 'my-filepond',
    multiple: true,
    labelIdle: "將檔案放置至此 <span class='filepond--label-action'>Browse</span>",
    // acceptedFileTypes: 'application/vnd.ms-excel, application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
    server: {
      url: this.env.apiUrl,
      process: 'Idea/SaveFile',
      revert: null,
    }
  }
  dataUser: [];
  userFields: object = { text: 'fullName', value: 'id' };
  userId: number = 0
  toId: number = 0 ;
  titleText: string = null
  issueText: string = null
  commentText: string = ''
  suggessionText: string = null
  proposal: boolean = true
  files: any;
  ideaId: any;
  nameTitle: string = null;
  issueTitle: string = null;
  accountGroupText: any;
  createdBy: any;
  toggle = true;
  status = 'Enable';
  statusName: any;

  StatusName = StatusName
  StatusCode = StatusCode
  tab: string;
  PROPOSAL = StatusCode.Propersal
  SPOKERMAN = StatusCode.Spokesman
  ERICK = StatusCode.Erick
  PROCESSING = StatusCode.Processing
  ERICKTAB = StatusCode.ErickTab
  flag: boolean;
  constructor(
    private router: Router,
    private alertify: AlertifyService,
    public todolistService: Todolistv2Service,
    public todolist2Service: Todolist2Service,
    private accountService: Account2Service,
    private accountGroupService: AccountGroupService,
    public modalService: NgbModal,
    public env: EnvService,
    private route: ActivatedRoute,
    public translate: TranslateService,
    public planIdeaService: PlanIdeaService,
    private spinner: NgxSpinnerService
  ) {
    this.planIdeaService.currentMessage.subscribe(res => {
      if (res === 'Reload complete Tab') {
        this.getTabClose()
      }
      if (res === 'Reload approval Tab') {
        this.getTabApproval()
      }
    })
    // setTimeout(() => {
      // }, 300);
    setTimeout(() => {
      this.locale = localStorage.getItem('lang')
      translate.getTranslation(this.locale);
      translate.use(this.locale)
    }, 300);
  }

  ngOnDestroy(): void {
  }
  rowDB(args: RowDataBoundEventArgs) {
    const data = args.data as any;
    if (data.isReject && this.tab === StatusCode.Close && this.accountGroupText === StatusCode.Spokesman) {
      args.row.classList.add('bgcolor');
    }
  }
  changeTab(item) {
    this.spinner.show()
    this.tabData.forEach(element => {
      element.type == item.type ? element.statues = true : element.statues =false
    });
    switch (item.type) {
      case StatusCode.Proposal:
        this.tab = "Proposal";
        this.router.navigate([`/transaction/todolist2/${this.tab}`]);
        this.getTabProposal()
        this.proposal = true
        break;
      case StatusCode.Processing:
        this.tab = "Processing";
        this.router.navigate([`/transaction/todolist2/${this.tab}`]);
        this.getTabProcessing()
        this.proposal = false
        break;
      case StatusCode.ErickTab:
        this.tab = "Erick";
        this.router.navigate([`/transaction/todolist2/${this.tab}`]);
        this.getTabErick()
        this.proposal = false
        break;
      case StatusCode.Close:
        this.tab = "Close";
        this.router.navigate([`/transaction/todolist2/${this.tab}`]);
        this.getTabClose()
        this.proposal = false
        break;
      case StatusCode.Announcement:
        this.tab = "Announcement";
        this.router.navigate([`/transaction/todolist2/${this.tab}`]);
        this.getTabAnnouncement()
        this.proposal = false
        break;
      case StatusCode.Approval:
        this.tab = "Approval";
        this.router.navigate([`/transaction/todolist2/${this.tab}`]);
        this.getTabApproval()
        this.proposal = false
        break;
      default:
        break;
    }
  }

  ngOnInit(): void {
    this.locale = localStorage.getItem('lang')
    this.tab = this.route.snapshot.params.tab || "Proposal";
    this.proposal = true
    this.accountGroupText = Number(JSON.parse(localStorage.getItem('user')).accountGroupSequence);
    this.getAllTab();
    this.spinner.show()
    // this.getTabProposal()
    switch (this.tab) {
      case StatusCode.Proposal:
        this.getTabProposal()
        this.proposal = true
        break;
      case StatusCode.Processing:
        this.getTabProcessing()
        this.proposal = false
        break;
      case StatusCode.ErickTab:
        this.getTabErick()
        this.proposal = false
        break;
      case StatusCode.Close:
        this.getTabClose()
        this.proposal = false
        break;
      case StatusCode.Approval:
        this.getTabApproval()
        this.proposal = false
        break;
      case StatusCode.Announcement:
        this.getTabAnnouncement()
        this.proposal = false
        break;
      default:
        break;
    }
    if (localStorage.getItem('user') !== null) {
      this.userId = Number(JSON.parse(localStorage.getItem('user')).id);
    }
  }
  
  public dataBound(): void {
    if(this.accountGroupText !== StatusCode.Spokesman)
    {
      this.gridToDo.showColumns('Announcement')
    } else {
      this.gridToDo.hideColumns('Announcement')
    }
  }
  loadDataByTab(tab) {
    switch (tab) {
      case StatusCode.Proposal:
        this.getTabProposal()
        this.proposal = true
        break;
      case StatusCode.Processing:
        this.getTabProcessing()
        break;
      case StatusCode.ErickTab:
        this.getTabErick()
        break;
      case StatusCode.Close:
        this.getTabClose()
        break;
      case StatusCode.Announcement:
        this.getTabAnnouncement()
        break;
      default:
        break;
    }
  }
  announcement(data) {
    this.todolist2Service.updateAnnouncement(data.id).subscribe(res => {
      if (res) {
        this.alertify.success('Success')
        this.loadDataByTab(this.tab)
      }else {
        this.alertify.error('Server error')
      }
    })
  }
  tooltip(args: QueryCellInfoEventArgs) {
    if (args.column.field === 'title') {
      const tooltip: Tooltip = new Tooltip({
          content: args.data[args.column.field]
      }, args.cell as HTMLTableCellElement);
    }
  }
  tooltipModal(args: QueryCellInfoEventArgs) {
    if (args.column.field === 'comment') {
      const tooltip: Tooltip = new Tooltip({
          content: args.data[args.column.field]
      }, args.cell as HTMLTableCellElement);
    }
  }
  suggessionUpdate(item) {
    this.toId = item.receiveID
    this.loadData()
    this.showModal(this.suggessionEdit)
    this.titleText = item.title
    this.issueText = item.issue
    this.suggessionText = item.suggession
    this.getDownloadFiles(item.id)
    this.ideaId = item.id
    // this.getAttackFiles(item.id)
  }

  removeFile(item) {
    this.todolist2Service.removeFileIdea(item.ideaId , item.name).subscribe(res => {
      this.getFilesPreview(item.ideaId)
    })
  }

  saveEditSuggession() {
    const formData = new FormData();
    formData.append("SendID", this.userId.toString());
    formData.append("IdeaID", this.ideaId.toString());
    formData.append("ReceiveID", this.toId.toString());
    formData.append("Title", this.titleText);
    formData.append("Suggession", this.suggessionText);
    formData.append("Issue", this.issueText);
    for (let item of this.file) {
      formData.append('UploadedFile', item);
    }
    if(this.file) {
      this.todolist2Service.importSaveEdit(formData).subscribe((res: any) => {
        this.alertify.success('Upload Success!')
        this.getTabProposal()
        this.refreshText()
        this.modalReference.close();
      })

    } else {
      this.alertify.error('Not File Upload!')
    }
  }

  submitEditSuggession() {
    const formData = new FormData();
    formData.append("SendID", this.userId.toString());
    formData.append("IdeaID", this.ideaId.toString());
    formData.append("ReceiveID", this.toId.toString());
    formData.append("Title", this.titleText);
    formData.append("Suggession", this.suggessionText);
    formData.append("Issue", this.issueText);
    for (let item of this.file) {
      formData.append('UploadedFile', item);
    }
    if(this.file) {
      this.todolist2Service.importSubmitEdit(formData).subscribe((res: any) => {
        this.alertify.success('Upload Success!')
        this.getTabProposal()
        this.refreshText()
        this.modalReference.close();
      })

    } else {
      this.alertify.error('Not File Upload!')
    }
  }
  // getAttackFiles(ideaId) {
  //   this.file = [];
  //   this.todolist2Service.getAttackFiles(ideaId).subscribe(res => {
  //     this.file = res as any || [];
  //   });
  // }

  getDownloadFiles(ideaId) {
    this.todolist2Service.GetAttackFilesIdea(ideaId).subscribe(res => {
      this.files = [];
      const files = res as any || [];
      this.files = files.map(x=> {
        return {
          name: x.name,
          path: this.base + x.path,
          ideaId: x.ideaId
        }
      });
      this.filesLeft = [];
      this.filesRight = [];
      let i = 0;
      for (const item of this.files) {
        this.filesLeft.push(item);
      }
    });
  }

  getFilesPreview(ideaId) {
    this.todolist2Service.GetAttackFilesIdeaPreview(ideaId).subscribe(res => {
      this.files = [];
      const files = res as any || [];
      this.files = files.map(x=> {
        return {
          name: x.name,
          path: this.base + x.path,
          ideaId: x.ideaId
        }
      });
      this.filesLeft = [];
      this.filesRight = [];
      let i = 0;
      for (const item of this.files) {
        this.filesLeft.push(item);
      }
    });
  }

  enableDisableRule() {
    this.toggle = !this.toggle;
    this.status = this.toggle ? 'Enable' : 'Disable';
  }

  detail(item) {
    this.ideaId = item.id
    this.createdBy = item.createdBy
    this.nameTitle = item.name
    this.issueTitle = item.title
    this.statusName = item.type
    let size = 'xxl'
    switch (item.type) {
      case StatusName.NA:
        this.showModal(this.suggessionEdit)
        break;
      case StatusName.Apply:
        this.showModalDetails(this.details,size)
        break;
      case StatusName.Update:
        this.showModalDetails(this.tabProcess,size)
        break;
      case StatusName.Reject:
        this.showModalDetails(this.rejectModel,size)
        break;
      case StatusName.Complete:
        this.showModalDetails(this.completeTerminateModel,size)
        break;
      case StatusName.Terminate:
        this.showModalDetails(this.completeTerminateModel,size)
        break;
      case StatusName.Dissatisfied:
        this.showModalDetails(this.dissatisfiedModel,size)
        break;
      case StatusName.Satisfied:
        if(this.accountGroupText === StatusCode.Spokesman && this.tab === StatusCode.Close){
          this.showModalDetails2(SatisfiedSpokermanModalComponent,item,'lg')
        }else if(this.accountGroupText === StatusCode.Spokesman && this.tab === StatusCode.Proposal) {
          this.showModalDetails(this.satisfiedCloseModel,size)
        }else if(this.accountGroupText === StatusCode.Erick && this.tab === StatusCode.Approval) {
          this.showModalDetails2(SatisfiedErickModalComponent,item,'lg')
        }else if(this.accountGroupText === StatusCode.Erick && this.tab === StatusCode.Proposal) {
          this.showModalDetails(this.satisfiedCloseModel,size)
        }else if(this.accountGroupText === StatusCode.Erick && this.tab === StatusCode.Close) {
          this.showModalDetails(this.satisfiedCloseModel,size)
        }else if(this.accountGroupText === StatusCode.Propersal && this.tab === StatusCode.Close) {
          this.showModalDetails(this.satisfiedCloseModel,size)
        }else {
          this.showModalDetails2(AnnouncementModalComponent,item,'lg')
        }
          
        break;
      case StatusName.Close:
        if(this.accountGroupText === StatusCode.Spokesman && this.tab === StatusCode.Close){
          this.showModalDetails2(SatisfiedSpokermanModalComponent,item,'lg')
        }else if(this.accountGroupText === StatusCode.Spokesman && this.tab === StatusCode.Proposal) {
          this.showModalDetails(this.satisfiedCloseModel,size)
        }else if(this.accountGroupText === StatusCode.Erick && this.tab === StatusCode.Approval) {
          this.showModalDetails2(SatisfiedErickModalComponent,item,'lg')
        }else if(this.accountGroupText === StatusCode.Erick && this.tab === StatusCode.Proposal) {
          this.showModalDetails(this.satisfiedCloseModel,size)
        }else if(this.accountGroupText === StatusCode.Propersal && this.tab === StatusCode.Close) {
          this.showModalDetails(this.satisfiedCloseModel,size)
        }
        else {
          this.showModalDetails2(AnnouncementModalComponent,item,'lg')
        }
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
    const check =  this.validateComment()
    if(check === false) return;
    if(check) {
      this.todolist2Service.accept(formData).subscribe(res => {
        this.alertify.success("Accept Successfully")
        this.getTabProposal()
        this.file = []
        this.modalReference.close()
      })
    }
  }

  update() {
    const formData = new FormData();
    formData.append("IdeaID", this.ideaId.toString());
    formData.append("CreatedBy", this.userId.toString());
    formData.append("Comment", this.commentText);
    for (let item of this.file) {
      formData.append('UploadedFile', item);
    }
    const check =  this.validateComment()
    if(check === false) return;
    if(check) {
      this.todolist2Service.update(formData).subscribe(res => {
        this.alertify.success("Update Successfully")
        this.modalReference.close()
        this.file = []
        this.commentText = ""
        this.getTabProcessing()
      })
    }
  }

  asign() {
    const formData = new FormData();
    formData.append("IdeaID", this.ideaId.toString());
    formData.append("CreatedBy", this.userId.toString());
    formData.append("Comment", this.commentText);
    for (let item of this.file) {
      formData.append('UploadedFile', item);
    }
    const check =  this.validateComment()
    if(check === false) return;
    if(check) {
      this.todolist2Service.update(formData).subscribe(res => {
        this.alertify.success("Successfully")
        this.modalReference.close()
        this.file = []
        this.commentText = ""
        this.getTabErick()
      })
    }
  }
  closeIdea() {
    const formData = new FormData();
    formData.append("IdeaID", this.ideaId.toString());
    formData.append("CreatedBy", this.userId.toString());
    formData.append("Comment", this.commentText);
    for (let item of this.file) {
      formData.append('UploadedFile', item);
    }
    const check =  this.validateComment()
    if(check === false) return;
    if(check) {
      this.todolist2Service.close(formData).subscribe(res => {
        this.alertify.success("Update Successfully")
        this.modalReference.close()
        this.file = []
        this.commentText = ""
        this.getTabErick()
      })
    }
  }
  terminate() {
    const formData = new FormData();
    formData.append("IdeaID", this.ideaId.toString());
    formData.append("CreatedBy", this.userId.toString());
    formData.append("Comment", this.commentText);
    for (let item of this.file) {
      formData.append('UploadedFile', item);
    }
    const check =  this.validateComment()
    if(check === false) return;
    if(check) {
      this.todolist2Service.terminate(formData).subscribe(res => {
        this.alertify.success("Successfully")
        this.file = []
        this.commentText = ""
        this.getTabProcessing()
        this.modalReference.close()
      })
    }
  }
  complete() {
    const formData = new FormData();
    formData.append("IdeaID", this.ideaId.toString());
    formData.append("CreatedBy", this.userId.toString());
    formData.append("Comment", this.commentText);
    for (let item of this.file) {
      formData.append('UploadedFile', item);
    }
    const check =  this.validateComment()
    if(check === false) return;
    if(check) {
      this.todolist2Service.complete(formData).subscribe(res => {
        this.alertify.success("Successfully")
        this.file = []
        this.commentText = ""
        this.getTabProcessing()
        this.modalReference.close()
      })
    }
  }
  reject() {
    const formData = new FormData();
    formData.append("IdeaID", this.ideaId.toString());
    formData.append("CreatedBy", this.userId.toString());
    formData.append("Comment", this.commentText);
    for (let item of this.file) {
      formData.append('UploadedFile', item);
    }
    const check =  this.validateComment()
    if(check === false) return;
    if(check) {
      this.todolist2Service.reject(formData).subscribe(res => {
        this.alertify.success("Successfully")
        this.file = []
        this.commentText = ""
        this.getTabProposal()
        this.modalReference.close()
      })
    }
  }

  satisfied() {
    const formData = new FormData();
    formData.append("IdeaID", this.ideaId.toString());
    formData.append("CreatedBy", this.userId.toString());
    formData.append("Comment", this.commentText);
    for (let item of this.file) {
      formData.append('UploadedFile', item);
    }
    const check =  this.validateComment()
    if(check === false) return;
    if(check) {
      this.todolist2Service.satisfied(formData).subscribe(res => {
        this.alertify.success("Successfully")
        this.file = []
        this.commentText = ""
        this.getTabProposal()
        this.modalReference.close()
      })
    }
  }

  dissatisfied() {
    const formData = new FormData();
    formData.append("IdeaID", this.ideaId.toString());
    formData.append("CreatedBy", this.userId.toString());
    formData.append("Comment", this.commentText);
    for (let item of this.file) {
      formData.append('UploadedFile', item);
    }
    const check =  this.validateComment()
    if(check === false) return;
    if(check) {

      this.todolist2Service.dissatisfied(formData).subscribe(res => {
        this.alertify.success("Successfully")
        this.file = []
        this.commentText = ""
        this.getTabProposal()
        this.modalReference.close()
      })
    }
  }
  getTabProposal() {
    this.todolist2Service.getTabProposal(this.locale).subscribe((res: any) => {
      console.log(res);
      if(this.accountGroupText === StatusCode.Spokesman) {
        let index = 1;
        this.data = res.filter(x => x.receiveID === this.userId && x.description !== 'N/A').map(x => {
          return {
            createdBy: x.createdBy,
            createdTime: x.createdTime,
            description: x.description,
            id: x.id,
            index: index++,
            issue: x.issue,
            name: x.name,
            comment: x.comment,
            isAnnouncement: x.isAnnouncement,
            receiveID: x.receiveID,
            sendID: x.sendID,
            statusName: x.statusName,
            suggession: x.suggession,
            title: x.title,
            type: x.type
          }
        })
        
        console.log(this.data)
      }
      if(this.accountGroupText === StatusCode.Propersal) {
        let index = 1;
        this.data = res.filter(x => x.createdBy === this.userId).map(x => {
          return {
            createdBy: x.createdBy,
            createdTime: x.createdTime,
            description: x.description,
            id: x.id,
            index: index++,
            issue: x.issue,
            isAnnouncement: x.isAnnouncement,
            name: x.name,
            receiveID: x.receiveID,
            sendID: x.sendID,
            comment: x.comment,
            statusName: x.statusName,
            suggession: x.suggession,
            title: x.title,
            type: x.type,

          }
        })
      }

      if(this.accountGroupText === StatusCode.Erick) {
        this.data = res.filter(x => x.description !== 'N/A')
      }
      // this.data = res
      this.spinner.hide()
    })
  }

  attackFile(data){
    this.showModal(this.fileModal)
    this.todolist2Service.getDownloadFilesIdea(data.id).subscribe((res: any) => {
      const files = res as any || [];
      this.files = files.map(x=> {
        return {
          name: x.name,
          path: this.env.fileUrl.replace('/api/', '') + x.path
        }
      });
    })
  }

  getIdeaHisById(id) {
    this.todolist2Service.getIdeaHisById(id, this.locale).subscribe((res: any) => {
      this.dataHis = res
    })
  }
  getTabProcessing() {
    this.todolist2Service.getTabProcessing(this.locale).subscribe((res: any) => {
      this.data = res
      this.spinner.hide()
    })
  }
  getTabErick() {
    this.todolist2Service.getTabErick(this.locale).subscribe((res: any) => {
      this.data = res
      this.spinner.hide()
    })
  }

  getTabAnnouncement() {
    this.todolist2Service.getTabAnnouncement(this.locale).subscribe((res: any) => {
      console.log(res);
      this.data = res
      this.spinner.hide()
    })
  }


  getTabClose() {
    this.todolist2Service.getTabClose(this.locale).subscribe((res: any) => {
      if (this.accountGroupText === StatusCode.Propersal) {
        let index = 1;
        this.data = res.filter(x => x.createdBy === this.userId).map(x => {
          return {
            createdBy: x.createdBy,
            createdTime: x.createdTime,
            description: x.description,
            id: x.id,
            index: index++,
            issue: x.issue,
            name: x.name,
            comment: x.comment,
            isAnnouncement: x.isAnnouncement,
            IsShowApproveTab: x.IsShowApproveTab,
            receiveID: x.receiveID,
            sendID: x.sendID,
            statusName: x.statusName,
            suggession: x.suggession,
            title: x.title,
            type: x.type
          }
        })
      } else {
        this.data = res
      }
      this.spinner.hide()
    })
  }

  getTabApproval() {
    this.todolist2Service.getTabApprove(this.locale).subscribe((res: any) => {
      if (this.accountGroupText === StatusCode.Propersal) {
        let index = 1;
        this.data = res.filter(x => x.createdBy === this.userId).map(x => {
          return {
            createdBy: x.createdBy,
            createdTime: x.createdTime,
            description: x.description,
            id: x.id,
            index: index++,
            issue: x.issue,
            name: x.name,
            comment: x.comment,
            isAnnouncement: x.isAnnouncement,
            receiveID: x.receiveID,
            sendID: x.sendID,
            statusName: x.statusName,
            suggession: x.suggession,
            title: x.title,
            type: x.type

          }
        })
      } else {
        this.data = res
      }
      this.spinner.hide()
    })
  }
  validateComment() {

    if (this.commentText === null || this.commentText === ''){
      this.alertify.warning(this.translate.instant('MESSAGE_COMMENT_TEXT'));
      return false;
    }
    return true;
  }

  validate() {

    if (this.toId === 0) {
      this.alertify.warning(this.translate.instant('MESSAGE_SAVE_TO_SUGGESTION'));
      return false;
    }

    if (this.titleText === null || this.titleText === ''){
      this.alertify.warning(this.translate.instant('MESSAGE_SAVE_TITLE_SUGGESTION'));
      return false;
    }

    if (this.issueText === null || this.issueText === ''){
      this.alertify.warning(this.translate.instant('MESSAGE_SAVE_ISSUE_SUGGESTION'));
      return false;
    }

    if (this.suggessionText === null || this.suggessionText === ''){
      this.alertify.warning(this.translate.instant('MESSAGE_SAVE_SUGGESTIONTEXT_SUGGESTION'));
      return false;
    }

    return true;
  }

  save() {
    const formData = new FormData();
    formData.append("SendID", this.userId.toString());
    formData.append("ReceiveID", this.toId.toString());
    formData.append("Title", this.titleText);
    formData.append("Suggession", this.suggessionText);
    formData.append("Issue", this.issueText);
    for (let item of this.file) {
      formData.append('UploadedFile', item);
    }
    const check =  this.validate()
    if(check === false) return;
    if(check) {
      this.todolist2Service.importSave(formData).subscribe((res: any) => {
        this.alertify.success('Upload Success!')
        this.getTabProposal()
        this.refreshText()
        this.modalReference.close();
      })

    } else {
      this.alertify.error('Server Error!')
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
    const check =  this.validate()
    if(check === false) return;

    if(check) {
      this.todolist2Service.importSubmit(formData).subscribe((res: any) => {
        this.alertify.success('Upload Success!')
        this.getTabProposal()
        this.refreshText()
        this.modalReference.close();
        // this.Topic_Name = null;
      })

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
      this.dataUser = data.filter(x => x.accountGroupSequence === StatusCode.Spokesman);
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
    this.modalReference.result.then((result) => {
      this.file = []
    }, (reason) => {
      this.file = []
    });
  }

  showModalDetails(modal,size){
    this.modalReference = this.modalService.open(modal, { size: size});
    // event click out side modal and close model
    this.modalReference.result.then((result) => {
      this.commentText = ""
      this.file = []
    }, (reason) => {
      this.commentText = ""
      this.file = []
    });
    // end event
  }

  showModalDetails2(model,data,size){
    const modalRef = this.modalService.open(model, { size: size, backdrop: 'static', keyboard: false });
    modalRef.componentInstance.data = data;
    modalRef.componentInstance.nameTitle = data.name;
    modalRef.componentInstance.issueTitle = data.title;
    modalRef.componentInstance.tab = this.tab;

    modalRef.result.then((result) => {
    }, (reason) => {
    });
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
    this.grid.search(this.name);
    // if(this.pendingTab)
    // if(this.buyingTab)
    //   this.gridBuying.search(this.name);
    // if(this.completeTab)
    //   this.gridComplete.search(this.name);
  }
  getAllTab(){
    const lang = localStorage.getItem('lang')
    this.accountGroupService.getAllTab(lang).subscribe(res => {
      if(this.accountGroupText === StatusCode.Spokesman) {
        this.tabData = res.filter(x => x.type !== StatusCode.ErickTab && x.type !== StatusCode.Approval)
      }
      if(this.accountGroupText === StatusCode.Propersal) {
        this.tabData = res.filter(x => x.type !== StatusCode.Processing && x.type !== StatusCode.ErickTab && x.type !== StatusCode.Approval)
      }

      if(this.accountGroupText === StatusCode.Erick) {
        this.tabData = res
      }
      if(this.tabData.length > 0) {
        this.tabData.forEach(element => {
          element.type ==  this.tab ? element.statues = true : element.statues = false
        });
      }
      this.spinner.hide()
    })
  }
  NO(index) {
    return (this.grid.pageSettings.currentPage - 1) * this.pageSettings.pageSize + Number(index) + 1;
  }

}
