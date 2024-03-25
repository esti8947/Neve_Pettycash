import { AbstractControl, ValidatorFn } from '@angular/forms';

export function passwordValidator(): ValidatorFn {
  return (control: AbstractControl): { [key: string]: any } | null => {
    const password = control.value;
    if (!password || password.length < 6) {
      return { 'invalidPasswordLength': true }; 
    }
    // Check if password contains at least one letter and one digit
    if (!/[a-zA-Z]/.test(password) || !/\d/.test(password)) {
      return { 'invalidPasswordFormat': true };
    }
    return null;
  };
}

export function emailValidator(): ValidatorFn {
  return (control: AbstractControl): { [key: string]: any } | null => {
    const email = control.value;
    // Regular expression for email validation
    const emailRegex = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
    if (!emailRegex.test(email)) {
      return { 'invalidEmailFormat': true }; 
    }
    return null;
  };
}
