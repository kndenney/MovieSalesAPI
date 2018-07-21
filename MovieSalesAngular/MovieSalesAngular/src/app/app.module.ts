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
import { MoviesComponent } from '../movies/movies.component';

// JSON Web Token Interceptor to add Authorization header to HTTP calls
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { TokenInterceptor } from '../shared/authorization/tokeninterceptor';
import { LoginComponent } from '../login/login.component';
import { AuthGuard } from '../shared/authorization/auth.guard';
import { AuthorizationService } from '../shared/authorization/services/authorization.service';
import { TokenRequest } from '../shared/authorization/models/tokenrequest';
import { TokenResponse, TokenResponses } from '../shared/authorization/models/tokenresponse';
import { MaterialModule } from '../modules/material.module';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { FlexLayoutModule, BREAKPOINT} from '@angular/flex-layout';
import { ErrorHandlers } from './error-handler';
import { ServerErrorsInterceptor } from '../shared/error/interceptors/server.error.interceptor';
import { NotificationService } from '../shared/notification/services/notification.service';
import { ErrorsService } from '../shared/error/services/error.service';
import { HttpService } from '../shared/error/services/http.service';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { RequestInterceptor } from './request-interceptor';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent,
    ErrorComponent,
    MoviesComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'counter', component: CounterComponent },
      { path: 'fetch-data', component: FetchDataComponent },
      { path: 'home', component: HomeComponent },
      { path: 'error', component: ErrorComponent },
      { path: 'notonline', component: ErrorComponent },
      { path: 'movies', component: MoviesComponent, canActivate: [AuthGuard] },
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
    // These could probably be put into a error module?
    NotificationService,
    ErrorsService,
    HttpService,
    TokenRequest,
    TokenResponses,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: TokenInterceptor,
      multi: true
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: RequestInterceptor, // ServerErrorsInterceptor,
      multi: true,
    },
    {
      provide: ErrorHandlers,
      useClass: ErrorHandlers,
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
