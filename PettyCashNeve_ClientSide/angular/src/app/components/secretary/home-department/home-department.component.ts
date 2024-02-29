import { Component, OnInit } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { MonthlyCashRegister } from 'src/app/models/monthlyCashRegister';
import { AuthService } from 'src/app/services/auth-service/auth.service';
import { MontlyCashRegisterService } from 'src/app/services/montlyCashRegister-service/montly-cash-register.service';

@Component({
  selector: 'home-department',
  templateUrl: './home-department.component.html',
  styleUrls: ['./home-department.component.scss'],
})
export class HomeDepartmentComponent implements OnInit {
  monthlyRegisters: MonthlyCashRegister[] = []; 
  isMonthlyRegister: boolean = false;
  currentUser: any;

  constructor(
    private monthlyCashRegisterService: MontlyCashRegisterService,
    private authService: AuthService,
    private router:Router,) { }

  ngOnInit(): void {
    this.currentUser = this.authService.getCurrentUser();
    this.loadCurrentMonthlyCashRegister();

    this.router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        if (this.authService.isLoggedIn) {
          this.currentUser = this.authService.getCurrentUser();
          this.loadCurrentMonthlyCashRegister();
        }
      }
    });
  }
  
  loadCurrentMonthlyCashRegister() {
    if (this.currentUser.isManager) {
      this.monthlyCashRegisterService.getCurrentMontlyCashRegisterByUserId(this.currentUser.departmentId).subscribe(
        (data) => {
          if (data.success && data.data != null) {
            this.isMonthlyRegister = true;
            this.monthlyRegisters.push(...data.data);
            console.log(data.data)
          } else {
            // Handle the case where data is incorrect
            this.isMonthlyRegister = false;
          }
          console.log('currentMonthlyCashRegister', this.isMonthlyRegister);
        },
        (error) => {
          console.error('Error loading currentMonthlyCashRegister', error);
          this.isMonthlyRegister = false;
        }
      );
    } else {
      // Call service without departmentId for non-manager
      this.monthlyCashRegisterService.getCurrentMontlyCashRegisterByUserId().subscribe(
        (data) => {
          if (data.success && data.data != null) {
            this.isMonthlyRegister = true;
            this.monthlyRegisters.push(data.data);
          } else {
            // Handle the case where data is incorrect
            this.isMonthlyRegister = false;
          }
          console.log('currentMonthlyCashRegister', this.isMonthlyRegister);
        },
        (error) => {
          console.error('Error loading currentMonthlyCashRegister', error);
          this.isMonthlyRegister = false;
        }
      );
    }
  }

  reloadMonthlyRegisters() {
    this.monthlyRegisters = []; // Clear existing data
    this.loadCurrentMonthlyCashRegister(); // Reload data
  }
}
