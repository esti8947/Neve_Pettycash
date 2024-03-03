import { Injectable } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';

@Injectable({
  providedIn: 'root'
})
export class MonthNameService {
  constructor(private translateService: TranslateService) {}

  getMonthName(monthNumber: number | undefined): string {
    if (monthNumber == undefined) {
      return '';
    }
    return this.translateService.currentLang === 'en-US' ?
      this.monthNames[monthNumber - 1] :
      this.monthNamesHeb[monthNumber - 1];
  }

  private monthNames: string[] = [
    'January', 'February', 'March', 'April', 'May', 'June', 'July',
    'August', 'September', 'October', 'November', 'December'
  ];

  private monthNamesHeb: string[] = [
    'ינואר', 'פברואר', 'מרץ', 'אפריל', 'מאי', 'יוני', 'יולי',
    'אוגוסט', 'ספטמבר', 'אוקטובר', 'נובמבר', 'דצמבר'
  ];
}
