import { BrowserModule } from '@angular/platform-browser';
import { NgModule, ErrorHandler } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

// Components
import { AppComponent } from './app.component';
import { NavMenuComponent } from '../nav-menu/nav-menu.component';
import { HomeComponent } from '../home/home.component';
import { CounterComponent } from '../counter/counter.component';
import { FetchDataComponent } from '../fetch-data/fetch-data.component';
import { ErrorComponent } from '../shared/error/components/error.component';

// JSON Web Token Interceptor to add Authorization header to HTTP calls
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { TokenInterceptor } from '../shared/authorization/tokeninterceptor';
import { LoginComponent } from '../login/login.component';
import { AuthGuard } from '../shared/authorization/auth.guard';
import { AuthorizationService } from '../shared/authorization/services/authorization.service';
import { TokenRequest } from '../shared/authorization/models/tokenrequest';
import { TokenResponse } from '../shared/authorization/models/tokenresponse';
import { MaterialModule } from './material.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { FlexLayoutModule, BREAKPOINT} from '@angular/flex-layout';
import { ErrorsHandler } from '../shared/error/error-handler';
import { ServerErrorsInterceptor } from '../shared/error/interceptors/server.error.interceptor';
import { NotificationService } from '../shared/error/services/notification.service';
import { ErrorsService } from '../shared/error/services/error.service';
import { HttpService } from '../shared/error/services/http.service';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent,
    LoginComponent,
    ErrorComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    BrowserAnimationsModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'counter', component: CounterComponent },
      { path: 'fetch-data', component: FetchDataComponent },
      { path: 'home', component: HomeComponent },
      { path: 'error', component: ErrorComponent },
      {
        path: 'login',
        loadChildren: '../modules/login.module#LoginModule'
      },
    ]),
    MaterialModule,
    NgbModule.forRoot(),
    FlexLayoutModule
  ],
  providers: [
    AuthGuard,
    AuthorizationService,
    //These could probably be put into a error module?
    NotificationService,
    ErrorsService,
    HttpService,
    
    TokenRequest,
    TokenResponse,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: TokenInterceptor,
      multi: true
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: ServerErrorsInterceptor,
      multi: true,
    },
    {
      provide: ErrorHandler,
      useClass: ErrorsHandler,
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
