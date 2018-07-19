import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { Observable, Subject, asapScheduler, pipe, of, from, interval, merge, fromEvent } from 'rxjs';

//Components
import { AppComponent } from './app.component';
import { NavMenuComponent } from '../nav-menu/nav-menu.component';
import { HomeComponent } from '../home/home.component';
import { CounterComponent } from '../counter/counter.component';
import { FetchDataComponent } from '../fetch-data/fetch-data.component';

//JSON Web Token Interceptor to add Authorization header to HTTP calls
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { TokenInterceptor } from '../shared/authorization/tokeninterceptor';
import { LoginComponent } from '../login/login.component';
import { AuthGuard } from '../shared/auth.guard';
import { AuthorizationService } from '../shared/authorization/services/authorization.service';
import { TokenRequest } from '../shared/authorization/models/tokenrequest';
import { TokenResponse } from '../shared/authorization/models/tokenresponse';
import { MaterialModule } from './material.module';
import { BrowserAnimationsModule } from '../../node_modules/@angular/platform-browser/animations';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent,
    LoginComponent
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
      { path: 'login', component: LoginComponent }
    ]),
    MaterialModule
  ],
  providers: [
    AuthGuard,
    AuthorizationService,
    TokenRequest,
    TokenResponse,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: TokenInterceptor,
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
