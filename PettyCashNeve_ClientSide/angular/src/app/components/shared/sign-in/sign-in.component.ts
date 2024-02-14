import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Password } from 'primeng/password';
import { AuthService } from 'src/app/services/auth-service/auth.service';

@Component({
  selector: 'app-sign-in',
  templateUrl: './sign-in.component.html',
  styleUrls: ['./sign-in.component.scss'],
})
export class SignInComponent implements OnInit {
  signInForm!: FormGroup;
  submitted = false;

  constructor(
    public fb: FormBuilder,
    private authService: AuthService,
  ) {}

  ngOnInit() {
    this.signInForm = new FormGroup({
      'username':new FormControl('', Validators.required),
      'password': new FormControl('', Validators.required)
    });
   }

  loginUser() {
    this.submitted = true;
    this.authService.signIn(this.signInForm.value);
  }
}
