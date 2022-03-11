import { PlanIdeaService } from './../../../../../_core/_service/planIdea.service';
import { filter } from 'rxjs/operators';
import { PlanIdea } from './../../../../../_core/_model/planIdea';
import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { GridComponent, IEditCell } from '@syncfusion/ej2-angular-grids';
import { StatusName } from 'src/app/_core/enum/JobType';
import { Todolist2Service } from 'src/app/_core/_service/todolist2.service';
import { AlertifyService } from 'src/app/_core/_service/alertify.service';
import { MessageConstants } from 'src/app/_core/_constants/system';

@Component({
  selector: 'app-announcementModal',
  templateUrl: './announcementModal.component.html',
  styleUrls: ['./announcementModal.component.scss']
})
export class AnnouncementModalComponent implements OnInit {

  @Input() data: any;
  @Input() nameTitle: any;
  @Input() issueTitle: any;
  @ViewChild('grid') grid: GridComponent;
  dataHis: any
  wrapSettings= { wrapMode: 'Content' };
  editSettings = { showDeleteConfirmDialog: false, allowEditing: true, allowAdding: true, allowDeleting: true, mode: 'Normal' };
  StatusName = StatusName
  toolbarOptions = ['Add','Update','Delete','Cancel'];
  locale: string;
  planIdeaData: PlanIdea = {
    id: 0,
    ideaID: 0,
    plan: null,
    description: null,
    statusPlanID: 0,
    createdBy: 0,
    statusPlanName: null,
    createdTime: null
  }
  planIdeaDataAddOrUpdate: PlanIdea = {
    id: 0,
    ideaID: 0,
    plan: null,
    description: null,
    statusPlanID: 0,
    createdBy: 0,
    statusPlanName: null,
    createdTime: null
  }
  planStatusData: any
  factoryHeadCommentData: any
  public dpParams: IEditCell;
  dataAdd = [];
  fields: object = { text: 'name', iconCss: 'icon', value: 'id' };
  fakeStatus: string = null
  isActionBegin: boolean = false
  titleModal: string = ''
  constructor(
    public activeModal: NgbActiveModal,
    private alertify: AlertifyService,
    public todolist2Service: Todolist2Service,
    public planIdeaService: PlanIdeaService,
  ) { 
    this.locale = localStorage.getItem('lang')
    
  }

  ngOnInit() {
    this.getIdeaHisById(this.data.id)
    this.getPlanIdea(this.data.id)
    this.getPlanStatus()
    this.getFactoryHeadComment()
  }
  getFactoryHeadComment() {
    this.todolist2Service.getFactoryHeadComment(this.data.id).subscribe((res) => {
      this.factoryHeadCommentData = res
    })
  }
  getPlanStatus() {
    this.todolist2Service.getPlanStatus().subscribe((res) => {
      this.planStatusData = res
    })
  }
  onChangeStatus(args ,data) {
    this.planIdeaData.statusPlanID = args.value
    if(data.Id !== undefined) {
      for (let item in this.grid.dataSource) {
        if(this.grid.dataSource[item].id === args.data.id) {
          this.grid.dataSource[item].description = args.data.description
          this.grid.dataSource[item].plan = args.data.plan
          this.grid.dataSource[item].statusPlanID = args.value
        }
      }
    }
    // this.planIdeaData.statusPlanID = args.value
  }
  savePlan() {
    this.grid.editModule.endEdit()
    if (this.planIdeaDataAddOrUpdate.statusPlanID === 0 && this.isActionBegin) {
      this.alertify.warning('Please select status');
    } else {
      this.planIdeaService.submitPlanIdea(this.data.id).subscribe(res => {
        if(res) {
          this.alertify.success('success')
          this.planIdeaService.changeMessage("Reload complete Tab");
        }else {
          this.alertify.error('fail on submit')
        }
      })
      this.activeModal.dismiss()
    }
    // this.todolist2Service.planIdeaAddOrUpdate(this.dataAdd).subscribe(res => {
    //   if(res) {
    //     this.alertify.success('success')
    //     this.getPlanIdea(this.data.id)
    //   }else {
    //     this.alertify.error('fail on save')
    //   }
    // })
  }
  create() {
    this.todolist2Service.planIdeaCreate(this.dataAdd).subscribe(res => {
      if(res) {
        this.getPlanIdea(this.data.id)
        this.dataAdd = []
        this.planIdeaDataAddOrUpdate = {} as PlanIdea;
      }
    })
  }

  update() {
    this.todolist2Service.planIdeaUpdate(this.dataAdd).subscribe(res => {
      if(res)
        this.getPlanIdea(this.data.id)
        this.dataAdd = [] 
        this.planIdeaDataAddOrUpdate = {} as PlanIdea;
    })
  }
 
  actionBegin(args) {
    
    if (args.requestType === "beginEdit" ) {
      this.planIdeaDataAddOrUpdate.description = args.rowData.description
      this.planIdeaDataAddOrUpdate.plan = args.rowData.plan
      this.planIdeaDataAddOrUpdate.createdBy = args.rowData.createdBy
      this.planIdeaDataAddOrUpdate.statusPlanID = args.rowData.statusPlanID
      this.planIdeaDataAddOrUpdate.ideaID = args.rowData.ideaID
    }
    if(args.requestType === 'save' && args.action === 'add')  {
      this.isActionBegin = true
      if (this.planIdeaDataAddOrUpdate.statusPlanID === 0) {
        this.alertify.warning('Please select status');
        args.cancel = true
      } else {
        this.planIdeaDataAddOrUpdate.description = args.data.description
        this.planIdeaDataAddOrUpdate.ideaID = this.data.id
        this.planIdeaDataAddOrUpdate.plan = args.data.plan
        this.dataAdd.push(this.planIdeaDataAddOrUpdate)
        this.create()
      }

    }
    if(args.requestType === 'save' && args.action === 'edit') {
      this.isActionBegin = true
      this.planIdeaDataAddOrUpdate.id = args.data.id
      this.planIdeaDataAddOrUpdate.description = args.data.description
      this.planIdeaDataAddOrUpdate.ideaID = this.data.id
      this.planIdeaDataAddOrUpdate.plan = args.data.plan
      this.planIdeaDataAddOrUpdate.createdTime = args.data.createdTime
      this.dataAdd.push(this.planIdeaDataAddOrUpdate)
      this.update()
      // for (let item in this.grid.dataSource) {
      //   if(this.grid.dataSource[item].id === args.data.id) {
      //     this.grid.dataSource[item].description = args.data.description
      //     this.grid.dataSource[item].plan = args.data.plan
      //   }
      // }
      // this.dataAdd = this.grid.dataSource as any;
    }
    if (args.requestType === 'delete') {
      if (args.data[0].id === undefined) {
        // this.alertify.success("成功刪除");
      } else {
        this.delete(args.data[0].id);
      }
    }
  }

  delete(id) {
    this.todolist2Service.planIdeaDelete(id).subscribe(
      (res) => {
        if (res) {
          this.getPlanIdea(this.data.id);
          this.alertify.success("成功刪除");
        } else {
          this.alertify.warning(MessageConstants.SYSTEM_ERROR_MSG)
        }
      },
      (err) => this.alertify.warning(MessageConstants.SYSTEM_ERROR_MSG)
    );
  }

  getIdeaHisById(id) {
    this.todolist2Service.getIdeaHisById(id, this.locale).subscribe((res: any) => {
      this.dataHis = res
    })
  }

  getPlanIdea(id) {
    this.todolist2Service.getPlanOkIdea(id).subscribe((res) => {
      this.planIdeaData = res
      // this.dataHis = res
    })
  }

}
