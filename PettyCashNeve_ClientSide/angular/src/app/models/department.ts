import { AnnualBudget } from './annualBudget';
import { MonthlyBudget } from './monthlyBudget';
import { RefundBudget } from './refundBudget';

export class Department {
  Id?: number;
  Code?: string;
  RoleId?: string;
  Name?: string;
  DeptHead?: string;
  PhoneNum?: string;
  Description?: string;
  StartDate?: Date;
  EndDate?: Date;
  Gender?: string;
  IsCurrent?: boolean;
  NDBAttendance?: boolean;
  DELimited?: boolean;
  AnnualTuition?: number;
  annualBudgets?: AnnualBudget[];
  monthlyBudgets?: MonthlyBudget[];
  refundBudgets?: RefundBudget[];
}
