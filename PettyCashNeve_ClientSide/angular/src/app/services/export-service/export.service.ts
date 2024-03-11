import { Injectable } from '@angular/core';
import * as XLSX from 'xlsx';
import { saveAs } from 'file-saver';

@Injectable({
  providedIn: 'root'
})
export class ExportService {
  constructor() {}

  // exportToExcel(data: any[], filename: string): void {
  //   const worksheet: XLSX.WorkSheet = XLSX.utils.json_to_sheet(data);
  //   const workbook: XLSX.WorkBook = { Sheets: { 'data': worksheet }, SheetNames: ['data'] };
  //   const excelBuffer: any = XLSX.write(workbook, { bookType: 'xlsx', type: 'array' });
  //   const file = new Blob([excelBuffer], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=UTF-8' });
  //   saveAs(file, filename + '.xlsx');
  // }
  exportToExcel(expenses: string) {
    // Parse the JSON string to an array of objects
    const data = JSON.parse(expenses).map((item: any) => ({
      buyerName: item.buyerName,
      eventCategoryName: item.eventCategoryName,
      eventName: item.eventName,
      'expense.expenseId': item.expense.expenseId,
      'expense.expenseCategoryId': item.expense.expenseCategoryId,
      'expense.eventsId': item.expense.eventsId,
      'expense.departmentId': item.expense.departmentId,
      'expense.updatedBy': item.expense.updatedBy,
      expenseCategoryName: item.expenseCategoryName,
      expenseCategoryNameHeb: item.expenseCategoryNameHeb
    }));
  
    const columns = Object.keys(data[0]);
    const worksheet = XLSX.utils.json_to_sheet(data, { header: columns });
    const workbook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(workbook, worksheet, 'Sheet1');
    XLSX.writeFile(workbook, 'data.xlsx');
  }
  
 
  

  getColumns(data: any[]): string[] {
    const columns: any[] = [];
    data.forEach(row => {
      Object.keys(row).forEach(col => {
        if (!columns.includes(col)) {
          columns.push(col);
        }
      });
    });
    return columns;
  }
}
