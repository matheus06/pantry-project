import { NgModule } from '@angular/core';
import { AuthModule } from 'angular-auth-oidc-client';
import { environment } from 'src/environments/environment';


@NgModule({
    imports: [AuthModule.forRoot({
        config: {
              authority: `${environment.identityUrl}`,
              redirectUrl: window.location.origin,
              postLogoutRedirectUri: window.location.origin,
              clientId: 'angular-client',
              scope: 'openid pantry product',
              responseType: 'code',
              silentRenew: false,
              useRefreshToken: false
          }
      })],
    exports: [AuthModule],
})
export class AuthConfigModule {}
