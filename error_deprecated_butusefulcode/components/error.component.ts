import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ErrorsService } from '../services/error.service';

@Component({
  selector: 'app-error',
  templateUrl: './error.component.html',
  styleUrls: ['./error.component.css']
})
export class ErrorComponent implements OnInit {
  public routeParams;
  public data;
  public dataFromError: string;
  public _errorService: ErrorsService;

  constructor(
    private activatedRoute: ActivatedRoute,
    private errorsService: ErrorsService
  ) {
  }

  ngOnInit() {
    // this.routeParams = this.activatedRoute.snapshot.queryParams;
    // this.data = this.activatedRoute.snapshot.data;
    this.data = this.errorsService.dataFromError;
    
    alert(this.data.name);
  }
}
