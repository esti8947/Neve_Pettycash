import { AnnualBudget } from "./annualBudget";
import { MonthlyBudget } from "./monthlyBudget";

export class NewYear{
    newYear:number | undefined;
    departmentId:number | undefined;
    budgetTypeId:number | undefined;
    annualBudget:AnnualBudget | undefined;
    monthlyBudget:MonthlyBudget | undefined;
}