import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MonthlyCashRegister } from 'src/app/models/monthlyCashRegister';
import { AdditionalActionsService } from 'src/app/services/additional-actions-service/additional-actions.service';
import { AuthService } from 'src/app/services/auth-service/auth.service';
import { CustomMessageService } from 'src/app/services/customMessage-service/custom-message.service';
import { MontlyCashRegisterService } from 'src/app/services/montlyCashRegister-service/montly-cash-register.service';

@Component({
  selector: 'close-monthly-activities',
  templateUrl: './close-monthly-activities.component.html',
  styleUrls: ['./close-monthly-activities.component.scss']
})
export class CloseMonthlyActivitiesComponent implements OnInit{

  MonthlyRegisterValue: MonthlyCashRegister = new MonthlyCashRegister();
  currentUsr:any;

  constructor(
    private additionalActionsService:AdditionalActionsService,
    private customMessageService:CustomMessageService,
    private monthlyCashRegisterService:MontlyCashRegisterService,
    private authService:AuthService,
    private router:Router){}
  
  ngOnInit(): void {
    this.getMonthlyRegister();
  }

  getMonthlyRegister(){
    this.currentUsr = this.authService.getCurrentUser();
    this.MonthlyRegisterValue = this.monthlyCashRegisterService.getCurrentMothlyRegister();
    
    // Check if MonthlyCashRegisterMonth is defined before calling getMonthName
    if (this.MonthlyRegisterValue.monthlyCashRegisterMonth !== undefined) {
    }

    console.log(this.MonthlyRegisterValue);
  }

  closeMonthlyActivities(){
    if(this.currentUsr.isManager){
      
    }
    if (this.MonthlyRegisterValue.refundAmount === 0) {
      // If refundAmount is 0, show an error and return
      this.customMessageService.showErrorMessage('Refund amount cannot be 0');
      return;
    }
    
    this.additionalActionsService.closeMonthlyActivities().subscribe(
      (respose) =>{
        console.log(respose);
        this.customMessageService.showSuccessMessage("expenses approved successfully")
        this.monthlyCashRegisterService.deactivateMonthlyCashRegister();
        this.router.navigate(['/navbar-secretary/home-secretary']);
      },
      (error) =>{
        console.error('An error occurred while approve expenses: ', error);
        this.customMessageService.showErrorMessage('An error occurred while approve expenses');
      }
    )
  }
}
