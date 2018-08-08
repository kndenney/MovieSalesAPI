import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoginRoutingModule } from './login.routing.module';
import { FormsModule } from '@angular/forms';
import { MaterialModule } from './material.module';
import { CreateAccountComponent } from '../create-account/create-account.component';
import { CreateAccountRoutingModule } from './create-account.routing.module';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    MaterialModule,
    CreateAccountRoutingModule
  ],
  declarations: [ CreateAccountComponent ]
})
export class CreateAccountModule {}
