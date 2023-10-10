import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Token } from '@angular/compiler';
import { Component, OnInit } from '@angular/core';
import { LoginResponse, OidcSecurityService } from 'angular-auth-oidc-client';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  constructor(
    public oidcSecurityService: OidcSecurityService,
    public http: HttpClient) {}

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

  callApi() {
    var httpOptions;
    const token = this.oidcSecurityService.getAccessToken().subscribe((token: any) => {
      httpOptions = {
        headers: new HttpHeaders({
          Authorization: 'Bearer ' + token,
        }),
      };
    });

    
    this.http.get("https://localdev-tls.me/api-product/product", httpOptions)
    .subscribe((data:any) => {
      console.log("api result:", data);
    });

   }
}
