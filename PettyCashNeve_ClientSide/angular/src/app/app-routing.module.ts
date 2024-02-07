import { Component, NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ChooseLanguageComponent } from './components/shared/choose-language/choose-language.component';
import { HomeDepartmentComponent } from './components/secretary/home-department/home-department.component';
import { AddEventComponent } from './components/secretary/add-event/add-event.component';
import { AddExpenseComponent } from './components/secretary/add-expense/add-expense.component';
import { ExpenseReportComponent } from './components/shared/expense-report/expense-report.component';
import { SignInComponent } from './components/shared/sign-in/sign-in.component';
import { UserProfileComponent } from './components/shared/user-profile/user-profile.component';
import { AuthGuard } from './guards/auth.guard';
import { PreviousExpensesComponent } from './components/shared/previous-expenses/previous-expenses.component';
import { HomeManagerComponent } from './components/manager/home-manager/home-manager/home-manager.component';
import { DynamicNavbarComponent } from './components/shared/dynamic-navbar/dynamic-navbar.component';

const routes: Routes = [
  // { path: '', component: HomeManagerComponent },
  { path: '', component: SignInComponent },
  { path: 'login', component: SignInComponent },
  { path: 'choose-language', component: ChooseLanguageComponent },
  { path: 'home-manager', component:HomeManagerComponent},
  { path: 'navbar', component: DynamicNavbarComponent,
    children:[
      { path: '', redirectTo: 'home-department', pathMatch: 'full' },
      { path: 'home-department', component: HomeDepartmentComponent },
      { path: 'add-event', component: AddEventComponent },
      { path: 'add-expense', component: AddExpenseComponent },
      { path: 'expense-report', component: ExpenseReportComponent },      
      { path: 'previous-expenses', component: PreviousExpensesComponent},
    ]}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
