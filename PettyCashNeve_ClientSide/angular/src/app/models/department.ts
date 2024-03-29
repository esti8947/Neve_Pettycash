import { AnnualBudget } from './annualBudget';
import { MonthlyBudget } from './monthlyBudget';
import { RefundBudget } from './refundBudget';

export class Department {
  departmentId?: number;
  departmentCode?: string;
  departmentName?: string;
  deptHeadFirstName?: string;
  deptHeadLastName?: string;
  phonePrefix?: string;
  phoneNumber?: string;
  description?: string;
  isCurrent?: boolean = true;
  currentBudgetTypeId?: number = 1;
}