import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { AuthService } from 'src/app/services/auth-service/auth.service';
import { BudgetImformationService } from 'src/app/services/budget-information-service/budget-imformation.service';

@Component({
  selector: 'budget-information',
  templateUrl: './budget-information.component.html',
  styleUrls: ['./budget-information.component.scss'],
})
export class BudgetInformationComponent implements OnInit{
  budgetInformation:any;
  budgetType:string = "";
  currentLang:string = "en-US"
  currentUser:any;
  departmentId:number | undefined

  constructor(
    private budgetInformationService:BudgetImformationService,
    private translateService:TranslateService,
    private authService:AuthService,
  ){}

  async ngOnInit(): Promise<void> {
    this.currentUser = await this.authService.getCurrentUser();
    this.getBudgetInformation();
  }

  getBudgetInformation(){
    if(this.currentUser.isManager){
      console.log(this.currentUser);
      this.departmentId = this.currentUser.departmentId;
    }
    this.budgetInformationService.getBudgetInformation(this.departmentId).subscribe(
      (data) => {
        this.budgetInformation = data.data;
        console.log(this.budgetInformation);
        this.currentLang = this.translateService.currentLang;
        if(this.budgetInformation.annualBudget != null){
          
          this.budgetType = "annualBudget";
        }
        else{
          if(this.budgetInformation.monthlyBudget != null){
            this.budgetType = "monthlyBudget";
          }
          else{
            this.budgetType = "refundBudget"
          }
        }
      },
      (error) => {
        console.error('An error occurred:', error);
      },
    );
  }

  getBudgetType(): string {
    return this.translateService.currentLang === 'en-US' ?
      `budget type - ${this.budgetInformation.budgetType.budgetTypeName}` :
      `סוג תקציב - ${this.budgetInformation.budgetType.budgetTypeNameHeb}`;
  }

  getSpentPercentage(totalAmount: number, annualBudgetCeiling: number): number {
    return (totalAmount / annualBudgetCeiling) * 100;
  }
}
