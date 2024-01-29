import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, FormBuilder } from '@angular/forms';
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

  constructor(
    public fb: FormBuilder,
    private authService: AuthService,
    private router: Router,
  ) {
    this.signInForm = this.fb.group({
      username: [''],
      password: [''],
    });
  }

  ngOnInit() {}

  loginUser() {
    this.authService.signIn(this.signInForm.value);
  }
}
