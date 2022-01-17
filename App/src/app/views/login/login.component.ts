import { SystemLanguageService } from './../../_core/_service/systemLanguage.service';
import { PermissionService } from 'src/app/_core/_service/permission.service';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { AlertifyService } from '../../_core/_service/alertify.service';
import { Router, ActivatedRoute } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';
import { UserForLogin } from 'src/app/_core/_model/user';
import { environment } from 'src/environments/environment';
import { RoleService } from 'src/app/_core/_service/role.service';
import { IRole, IUserRole } from 'src/app/_core/_model/role';
import { IBuilding } from 'src/app/_core/_model/building';
import { AuthenticationService } from 'src/app/_core/_service/authentication.service';
import { Subscription } from 'rxjs';
import { FunctionSystem } from 'src/app/_core/_model/application-user';
import { NgxSpinnerService } from 'ngx-spinner';
import { Authv2Service } from 'src/app/_core/_service/authv2.service';
@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit, OnDestroy {
  busy = false;
  username = '';
  password = '';
  loginError = false;
  private subscription: Subscription;

  user: UserForLogin = {
    username: '',
    password: '',
    systemCode: environment.systemCode
  };
  uri: any;
  level: number;

  remember = false;
  functions: FunctionSystem[];
  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private authService: Authv2Service,
    private spinner: NgxSpinnerService,
    private cookieService: CookieService,
    private permissionService: PermissionService,
    private alertifyService: AlertifyService,
    private systemLanguageService: SystemLanguageService
  ) {
    if (this.cookieService.get('remember') !== undefined) {
      if (this.cookieService.get('remember') === 'Yes') {
        this.user = {
          username: this.cookieService.get('username'),
          password: this.cookieService.get('password'),
          systemCode: environment.systemCode
        };
        this.username = this.cookieService.get('username');
        this.password = this.cookieService.get('password');
        this.remember = true;
        this.login();
      }
    }
    this.uri = this.route.snapshot.queryParams.uri || '/transaction/todolist2';
  }
  role: number;
  ngOnInit(): void {
    const accessToken = localStorage.getItem('token');
    const refreshToken = localStorage.getItem('refresh_token');
    if (accessToken && refreshToken && this.route.routeConfig.path === 'login') {
      const uri = decodeURI(this.uri) || '/transaction/todolist2';
      this.router.navigate([uri]);
    }
  }
  onChangeRemember(args) {
    this.remember = args.target.checked;
  }
  authentication() {
    return this.authService
      .login(this.username, this.password).toPromise();
  }
  async login() {
    if (!this.username || !this.password) {
      return;
    }
    this.spinner.show();
    this.busy = true;
    try {

      const data = await this.authentication();
      const currentLang = localStorage.getItem('lang');



      if (this.remember) {
        this.cookieService.set('remember', 'Yes');
        this.cookieService.set('username', this.user.username);
        this.cookieService.set('password', this.user.password);
        this.cookieService.set('systemCode', this.user.systemCode.toString());
      } else {
        this.cookieService.set('remember', 'No');
        this.cookieService.set('username', '');
        this.cookieService.set('password', '');
        this.cookieService.set('systemCode', '');
      }

      this.systemLanguageService.getLanguages(localStorage.getItem('lang') || 'zh').subscribe(res => {
        localStorage.setItem('languages', JSON.stringify(res));
      })
      if (currentLang) {
        localStorage.setItem('lang', currentLang);
      } else {
        localStorage.setItem('lang', 'zh');
      }
      localStorage.setItem('user', JSON.stringify(data.user));
      localStorage.setItem('token', data.token);
      this.router.navigate(['/transaction/todolist2']);

      // console.log('end getActionInFunctionByRoleID');
      this.alertifyService.success('Login Success!!');
      this.busy = false;
      this.spinner.hide();
    } catch (error) {
      this.spinner.hide();
      this.busy = true;
      this.loginError = true;
    }
  }
  getMenu(userid) {
    this.permissionService.getMenuByUserPermission(userid).toPromise();
  }
  ngOnDestroy(): void {
    this.subscription?.unsubscribe();
  }

  checkRole(): boolean {
    const uri = decodeURI(this.uri);
    const permissions = this.functions.map(x => x.url);
    for (const url of permissions) {
      if (uri.includes(url)) {
        return true;
      }
    }
    return false;

  }
}
