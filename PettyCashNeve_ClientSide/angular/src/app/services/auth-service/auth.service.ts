import { API_CONFIG } from 'src/app/config/api.config';
import { Injectable } from '@angular/core';
import { LoginUser } from 'src/app/models/loginUser';
import { Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import {
  HttpClient,
  HttpHeaders,
  HttpErrorResponse,
} from '@angular/common/http';
import { Router } from '@angular/router';
import { Department } from 'src/app/models/department';
import { UserInfo } from 'src/app/models/userInfo';
import { RegisterUser } from 'src/app/models/registerUser';

@Injectable({
  providedIn: 'root',
})
export class AuthService {

  private baseUrl = API_CONFIG.baseUrl;
  endpoint: string = `${this.baseUrl}/Account`;
  headers = new HttpHeaders().set('Content-Type', 'application/json');
  private _isManager: boolean = false;

  constructor(
    private http: HttpClient,
    public router: Router,
  ) {}

  //Sign-in
  signIn(user: LoginUser) {
    return this.http
      .post<any>(`${this.endpoint}/login`, user)
      .pipe(catchError((error) => {
        console.error('Login error', error);
        alert('Username or password is incorrect.');
        return throwError(error);
      })
      )
      .subscribe((res: any) => {
        this._isManager = res.isManager;
        localStorage.setItem('access_token', res.token);
        localStorage.setItem('current_user', JSON.stringify(res));
        console.log('currentUser', res);
        this._isManager?
        this.router.navigate(['home-manager']):
        this.router.navigate(['navbar']);
      });
    }

  getToken() {
    return localStorage.getItem('access_token');
  }

  getCurrentUser() {
    const userJson = localStorage.getItem('current_user');
    return userJson ? JSON.parse(userJson) : null;
  }

  updateCurrentUserDepartmentId(departmentId: number) {
    const currentUserJson = localStorage.getItem('current_user');
    if (currentUserJson) {
      const currentUser = JSON.parse(currentUserJson);
      currentUser.departmentId = departmentId;
      localStorage.setItem('current_user', JSON.stringify(currentUser));
    }
  }

  get isLoggedIn(): boolean {
    let authToken = localStorage.getItem('access_token');
    return authToken != null ? true : false;
  }

  doLogout() {
    let removeToken = localStorage.removeItem('access_token');
    if (removeToken == null) {
      this.router.navigate(['/login']);
    }
  }

  GetUserIdFromToken(): string | null {
    const token = this.getToken();
    if (token == null) {
      return null;
    }
    const payload = token.split('.')[1];
    const payloadDecodedJson = window.atob(payload);
    const payloadDecoded = JSON.parse(payloadDecodedJson);
    return payloadDecoded.jti;
  }

  registerUser(newUser:RegisterUser):Observable<any>{
    const url = `${this.endpoint}/Register`;
    return this.http.post<any>(url, newUser).pipe(
      catchError((error) => {
        console.error('Error in registerUser function:', error);
        return throwError(error);
      })
    )
  }
  
  deleteUser(username:string){
    const url = `${this.endpoint}/deleteUser/${username}`;
    return this.http.delete<any>(url).pipe(
      catchError((error)=>{
        console.error('Error in deleteUser function', error);
        return throwError(error);
      }),
    );
  }

  handleError(error: HttpErrorResponse) {
    let msg = '';
    if (error.error instanceof ErrorEvent) {
      //clientSideError
      msg = error.error.message;
    } else {
      //server-side error
      msg = `Error Code: ${error.status}\nMessage: ${error.message}`;
    }
    return throwError(msg);
  }
}
