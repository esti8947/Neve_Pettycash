export class NewExpenseModel {
  expenseId:number = 0;
  expenseCategoryId: number | undefined;
  eventsId: number | undefined;
  updatedBy: string|undefined;
  departmentId:number|undefined;
  expenseAmount: number |undefined;
  storeName: string|undefined;
  expenseDate: string|undefined;
  refundMonth: number|undefined;
  isLocked: boolean = false;
  isApproved: boolean = false;
  buyerId: number|undefined;
  isActive: boolean = true;
  invoiceScan: string = "";
  notes:string = "";
}
