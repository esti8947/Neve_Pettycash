import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { CustomMessageService } from 'src/app/services/customMessage-service/custom-message.service';
import { MontlyCashRegisterService } from 'src/app/services/montlyCashRegister-service/montly-cash-register.service';

@Component({
  selector: 'add-monthly-cash-register',
  templateUrl: './add-monthly-cash-register.component.html',
  styleUrls: ['./add-monthly-cash-register.component.scss']
})
export class AddMonthlyCashRegisterComponent implements OnInit{
  formGroup!: FormGroup;
  validForm: boolean = true;
  months:any[]=[];
  addMonthlyRegisterDialog:boolean = false;

  constructor(
    private formBuilder: FormBuilder,
    private monthlyCashRegisterService: MontlyCashRegisterService,
    private customMessageService:CustomMessageService,
    private router:Router
  ){}

  ngOnInit(): void {
    this.initializeForm();
    this.loadMonths();
  }

  loadMonths() {
    const currentDate = new Date();
    const currentMonth = currentDate.getMonth();
  
    const monthNames = [
      'January', 'February', 'March', 'April', 'May', 'June',
      'July', 'August', 'September', 'October', 'November', 'December'
    ];
  
    this.months = Array.from({ length: 12 }, (_, index) => ({
      value: index + 1, // Months are 1-based in JavaScript Date object
      name: monthNames[index]
    }));
  }
  


  initializeForm() {
    const currentDate = new Date();
    const currentMonth = currentDate.getMonth() + 1;
    const currentYear = currentDate.getFullYear(); 

    this.formGroup = this.formBuilder.group({
      selectedMonth: new FormControl<number>(currentMonth ,Validators.required),
      currentYear: new FormControl<number>(currentYear, Validators.required),
    });
  }

  addMonthlyRegister(){
    this.addMonthlyRegisterDialog = true;
  }

  async saveMonthlyRegister(){
    // this.validForm = !this.validForm;
    if (this.validForm) {
      const selectedMonthValue = this.formGroup.value.selectedMonth;

      const newMonthlyRegister = {
        monthlyCashRegisterId: 0,
        updatedBy: "string",
        monthlyCashRegisterName: "string",
        monthlyCashRegisterMonth: selectedMonthValue.value,
        monthlyCashRegisterYear: this.formGroup.value.currentYear,
        amountInCashRegister: 0,
        refundAmount: 0,
        isActive: true
      };
  
      console.log("New Monthly Register:", newMonthlyRegister);
  
      try {
        const response = await this.monthlyCashRegisterService.addMonthlyRegister(newMonthlyRegister).toPromise();
        console.log('Monthly register added successfully:', response);
        this.customMessageService.showSuccessMessage('Monthly register added successfully');
        this.formGroup.reset();
        this.addMonthlyRegisterDialog = false;
  
        const currentMonthlyCashRegisterResponse = await this.monthlyCashRegisterService.getCurrentMontlyCashRegisterByUserId().toPromise();
  
        console.log('Current Monthly Cash Register:', currentMonthlyCashRegisterResponse);
  
        this.router.navigate(['/navbar/home-department']);
      } catch (error) {
        console.error('An error occurred while adding the monthly register:', error);
        this.customMessageService.showErrorMessage('An error occurred while adding the monthly register');
      }
    }
  }
}