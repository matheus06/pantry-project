import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { LoginResponse, OidcSecurityService } from 'angular-auth-oidc-client';
import { environment } from 'src/environments/environment';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  constructor(
    public oidcSecurityService: OidcSecurityService,
    public http: HttpClient) {}
    public apiResponse : any;

  ngOnInit() {
    this.oidcSecurityService.checkAuth().subscribe((  loginResponse : LoginResponse) =>  this.handleLogin(loginResponse));
  }

  handleLogin(loginResponse: LoginResponse) {
    console.log('is authenticated', loginResponse)
    const { isAuthenticated, userData, accessToken, idToken, configId } = loginResponse;
    if(!isAuthenticated)
      this.oidcSecurityService.authorize();
  }

  logout() {
    console.log('logout')
    this.oidcSecurityService
    .logoff()
    .subscribe((result: any) => console.log(result));
  }

  callProductApi() {
    var httpOptions;
    const token = this.oidcSecurityService.getAccessToken().subscribe((token: any) => {
      httpOptions = {
        headers: new HttpHeaders({
          Authorization: 'Bearer ' + token,
        }),
      };
    });

    
    this.http.get(`${environment.productApiUrl}/product`, httpOptions)
    .subscribe((data:any) => {
      this.apiResponse = data;
      console.log("api result:", data);
    });

   }
}
