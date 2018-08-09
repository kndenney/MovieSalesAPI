import { BrowserModule } from '@angular/platform-browser';
import { NgModule, ErrorHandler } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

// Components
import { AppComponent } from './app.component';
import { NavMenuComponent } from '../nav-menu/nav-menu.component';
import { CounterComponent } from '../counter/counter.component';
import { FetchDataComponent } from '../fetch-data/fetch-data.component';
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
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ErrorInterceptor } from '../error-handling/error-interceptor';
import { ErrorHandlers } from '../error-handling/error-handler';
import { MoviesService } from '../movies/services/movies.service';
import { CreateAccountService } from '../create-account/services/create-account.services';
import { User } from '../create-account/models/user';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    CounterComponent,
    FetchDataComponent,
    MoviesComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
     // { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'counter', component: CounterComponent },
      { path: 'fetch-data', component: FetchDataComponent },
      { path: 'movies', component: MoviesComponent, canActivate: [AuthGuard] },
      {
        path: 'login',
        loadChildren: '../modules/login.module#LoginModule'
      },
      {
        path: 'create',
        loadChildren: '../modules/create-account.module#CreateAccountModule'
      },
    ]),
    MaterialModule,
    NgbModule.forRoot(),
    FlexLayoutModule
  ],
  providers: [
    AuthGuard,
    AuthorizationService,
    MoviesService,
    CreateAccountService,
    User,
    TokenRequest,
    TokenResponses,
    ErrorHandlers,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: TokenInterceptor,
      multi: true
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: ErrorInterceptor,
      multi: true,
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
