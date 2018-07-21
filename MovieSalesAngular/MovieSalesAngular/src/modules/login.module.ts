import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoginRoutingModule } from './login.routing.module';
import { LoginComponent } from '../login/login.component';
import { FormsModule } from '@angular/forms';
import { MaterialModule } from './material.module';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    MaterialModule,
    LoginRoutingModule
  ],
  declarations: [ LoginComponent ]
})
export class LoginModule {}
