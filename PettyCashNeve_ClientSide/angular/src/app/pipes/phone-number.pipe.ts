import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'phoneNumber'
})
export class PhoneNumberPipe implements PipeTransform {

  transform(number:any) {
    number = number.charAt(0) != 0 ? "0" + number : "" + number;
  
    let newStr = "";
    let i = 0;
  
    for (; i < Math.floor(number.length / 2) - 1; i++) {
      newStr = newStr + number.substr(i * 2, 2) + "-";
    }
  
    return newStr + number.substr(i * 2);
  }
}

  @Pipe({
    name: 'budgetYearFormat'
  })
  export class BudgetYearFormatPipe implements PipeTransform {
  
    transform(number: any) {
      // Format the number as 0000-0000
      // Assuming number is a string of length 8
      return number.slice(0, 4) + '-' + number.slice(4);
    }
} 
