import { NgModule } from '@angular/core';
import { AuthModule } from 'angular-auth-oidc-client';

@NgModule({
    imports: [AuthModule.forRoot({
        config: {
              authority: 'http://localhost:62249',
              redirectUrl: window.location.origin,
              postLogoutRedirectUri: window.location.origin,
              clientId: 'angular-client',
              scope: 'openid pantry',
              responseType: 'code',
              silentRenew: true,
              useRefreshToken: true
          }
      })],
    exports: [AuthModule],
})
export class AuthConfigModule {}
