import { HttpClient, HttpParams } from '@angular/common/http';
import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { SelectItem } from 'primeng/api';
import { ButtonModule } from 'primeng/button';
import { SelectButtonModule } from 'primeng/selectbutton';
import { TableModule } from 'primeng/table';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';


@Component({
  selector: 'app-root',
  imports: [
    RouterOutlet,
    ButtonModule,
    TableModule,
    SelectButtonModule,
    FormsModule,
    CommonModule
  ],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App {
  constructor(private http: HttpClient) {
  }
  protected readonly title = signal('TTSS-Game-Analysis.Web');

  activities = signal([]);
  churnings = signal([]);
  isAdmin = signal(true);

  options: SelectItem[] = [
    { label: 'Player', value: 'A' },
    { label: 'Admin', value: 'B' },
  ];
  selectedOption: string | undefined = 'B';

  onChaneRole() {
    this.isAdmin.set(this.selectedOption === 'B');
    this.loadActivities();
    this.loadChurnings();
  }

  getHeader() {
    return {
      'Content-Type': '*/*',
      'Authorization': !this.isAdmin()
        ? 'Bearer eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJhbmlydXQwMTMiLCJuYW1lIjoiQW5pcnV0IENoYW9nbGEiLCJyb2xlcyI6WyJQTEFZRVIiXX0.XeCWs37KxhTAP2Wx5OdqIb5hrikYO7Z_eYtpV2ELW3vzJ93EqXsyejbxsUFbfljGFcOvRj7fZCQWQn2mSzYznj8T-gMZwNVmnc0VPGmfELGRBWKOVcs9qais5W7tEiYs6mOivxzmmLDZSckGWQIc-8u09PALudm4yIbDBv-BpN7DQ6nEHLzJhFzxLkfLe96abmhn87Um829ye3CK25irLz1o0Idk0S8BX7cdZBpn4yOrpvI706e34fXVXvbcKtpzPmjEhpTBkpAlhYoNK-AB7xUWKxGJXgLqmPaD09GFesP52tY-zkha2WC0Tb4Qc4qVgvKGlD_qMr2-M2cqk-ZlXg'
        : 'Bearer eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJhbmlydXQwMDEiLCJuYW1lIjoiQW5pcnV0IENoYW9nbGEiLCJyb2xlcyI6WyJBRE1JTiJdfQ.X5aeORR_7PttXtQAau3wgj0Sg2dl16SvYrHK-c2BNyJIPZ2G0RwUsMB7ABjtWHblfNYDD4mGi92t01FEBDmLsdmlQNGOF-odgPf1MBhzHjdCIcr75zT3WPKtjZ5qmuNs_9VRXDjlhOpDNYXMatDIAOdVKYXB3IFWr7rc0WAyLb65UQd2xcBkOXj4VpPjS6H3OWhi79e1AZk5oKgruHFI2Dg5jv-Kk9JNLlJc-_j8yhYUE4ll_KtsDPXvZHpS_9qqLoFdApCQhZTL7HFvmEViZW6-DzPgZWSNfsxvFsmcW63tMbqRY-0UttPaWjXfaTE_noub_Nluz6bFiCpYJkf_8w'
    }
  }

  ngOnInit() {
    this.loadActivities();
    this.loadChurnings();
  }

  loadActivities() {
    this.http.get<any>('https://localhost:7222/players/activities/1', {
      headers: this.getHeader()
    }).subscribe((data: any) => {
      console.log(data);
      this.activities.set(data);
    }, (error: any) => {
      this.activities.set([]);
    });
  }

  loadChurnings() {
    this.http.get<any>('https://localhost:7222/players/churnings', {
      headers: this.getHeader()
    }).subscribe((data: any) => {
      console.log(data);
      this.churnings.set(data);
    }, (error: any) => {
      this.churnings.set([]);
    });
  }
}
