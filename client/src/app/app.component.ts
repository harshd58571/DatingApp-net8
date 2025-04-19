import { NgFor } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet,NgFor],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  // title = 'client';
  title = 'client';
 http= inject(HttpClient) 
users:any;
 ngOnInit() {
    this.http.get('http://localhost:5106/api/user').subscribe({
next: Response => this.users = Response,
error: error => console.error('There was an error!', error),
complete: () => console.log('request has completed')

    })
  }
}
