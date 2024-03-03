import { AnnualBudget } from './annualBudget';
import { MonthlyBudget } from './monthlyBudget';
import { RefundBudget } from './refundBudget';

export class DepartmentMoreInfo {
  departmentId: number = 0;
  departmentCode?: string;
  departmentName?: string;
  deptHeadFirstName?: string;
  deptHeadLastName?: string;
  phonePrefix?: string;
  phoneNumber?: string;
  description?: string;
  isCurrent?: boolean = true;
  currentBudgetTypeId?: number = 1;
  monthlyCashRegister:any;
  budgetInformation:any;
  budgetType:string = "";
  totalExpensesAmount?:number;
  monthlyAmountForCalculatingPercentages?:number;
  amountWastedForCalculatingPercentages?:number;
}