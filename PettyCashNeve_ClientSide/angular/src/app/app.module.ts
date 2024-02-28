import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import {
  HttpClientModule,
  HttpClient,
  HTTP_INTERCEPTORS,
} from '@angular/common/http';
import { TranslateModule, TranslateLoader } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

import { MatIconModule } from '@angular/material/icon';

import { ButtonModule } from 'primeng/button';
import { TableModule } from 'primeng/table';
import { DialogModule } from 'primeng/dialog';
import { PaginatorModule } from 'primeng/paginator';
import { ToastModule } from 'primeng/toast';
import { TabMenuModule } from 'primeng/tabmenu';
import { InputTextModule } from 'primeng/inputtext';
import { DropdownModule } from 'primeng/dropdown';
import { InputNumberModule } from 'primeng/inputnumber';
import { CalendarModule } from 'primeng/calendar';
import { TabViewModule } from 'primeng/tabview';
import { CardModule } from 'primeng/card';
import { PasswordModule } from 'primeng/password';
import { ScrollerModule } from 'primeng/scroller';
import { OrderListModule } from 'primeng/orderlist';
import { ConfirmPopupModule } from 'primeng/confirmpopup';
import { ListboxModule } from 'primeng/listbox';
import { TooltipModule } from 'primeng/tooltip';
import { DataViewModule } from 'primeng/dataview';
import { ConfirmDialogModule } from 'primeng/confirmdialog';


import { HomeManagerComponent } from './components/manager/home-manager/home-manager/home-manager.component';
import { HomeDepartmentComponent } from './components/secretary/home-department/home-department.component';
import { ChooseLanguageComponent } from './components/shared/choose-language/choose-language.component';
import { ExpenseReportComponent } from './components/shared/expense-report/expense-report.component';
import { ConfirmationService, MessageService } from 'primeng/api';
import { MonthlyCashRegisterComponent } from './components/shared/monthly-cash-register/monthly-cash-register.component';
import { BudgetInformationComponent } from './components/shared/budget-information/budget-information.component';
import { AddExpenseComponent } from './components/secretary/add-expense/add-expense.component';
import { AddEventComponent } from './components/secretary/add-event/add-event.component';
import { CreateUpdateEventCategoryComponent } from './components/secretary/create-update-event-category/create-update-event-category.component';
import { SignInComponent } from './components/shared/sign-in/sign-in.component';
import { UserProfileComponent } from './components/shared/user-profile/user-profile.component';
import { AuthService } from './services/auth-service/auth.service';
import { AuthInterceptor } from './services/auth-service/authconfig.interceptor';
import { PreviousExpensesComponent } from './components/shared/previous-expenses/previous-expenses.component';
import { CustomMessageService } from './services/customMessage-service/custom-message.service';
import { AddMonthlyCashRegisterComponent } from './components/secretary/add-monthly-cash-register/add-monthly-cash-register.component';
import { ChooseDepartmentComponent } from './components/manager/choose-department/choose-department/choose-department.component';
import { AddDepartmentComponent } from './components/manager/add-department/add-department/add-department.component';
import { AddExpenseCategoryComponent } from './components/manager/add-expenseCategory/add-expense-category/add-expense-category.component';
import { DynamicNavbarComponent } from './components/shared/dynamic-navbar/dynamic-navbar.component';
import { LockExpensesButtonComponent } from './components/manager/lock-expenses-button/lock-expenses-button.component';
import { UsersOfDepartmentComponent } from './components/manager/users-of-department/users-of-department.component';
import { PhoneNumberPipe } from './pipes/phone-number.pipe';

export function HttpLoaderFactory(http: HttpClient) {
  return new TranslateHttpLoader(http, './assets/i18n/', '.json');
}

@NgModule({
  declarations: [
    AppComponent,
    HomeManagerComponent,
    HomeDepartmentComponent,
    ChooseLanguageComponent,
    ExpenseReportComponent,
    MonthlyCashRegisterComponent,
    BudgetInformationComponent,
    AddExpenseComponent,
    AddEventComponent,
    CreateUpdateEventCategoryComponent,
    SignInComponent,
    UserProfileComponent,
    PreviousExpensesComponent,
    AddMonthlyCashRegisterComponent,
    ChooseDepartmentComponent,
    AddDepartmentComponent,
    AddExpenseCategoryComponent,
    DynamicNavbarComponent,
    LockExpensesButtonComponent,
    UsersOfDepartmentComponent,
    PhoneNumberPipe,
    
  ],
  imports: [
    BrowserModule,
    FormsModule,
    AppRoutingModule,
    MatIconModule,
    BrowserAnimationsModule,
    ReactiveFormsModule,
    HttpClientModule,
    ButtonModule,
    TableModule,
    PaginatorModule,
    ToastModule,
    TabMenuModule,
    InputTextModule,
    DropdownModule,
    InputNumberModule,
    CalendarModule,
    DialogModule,
    TabViewModule,
    CardModule,
    PasswordModule,
    ScrollerModule,
    OrderListModule,
    ConfirmPopupModule,
    ListboxModule,
    TooltipModule,
    DataViewModule,
    ConfirmDialogModule,
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: HttpLoaderFactory,
        deps: [HttpClient],
      },
    }),
  ],
  providers: [
    MessageService,
    ConfirmationService,
    CustomMessageService,
    AuthService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true,
    },
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
