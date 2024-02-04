import { Component, NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ChooseLanguageComponent } from './components/shared/choose-language/choose-language.component';
import { HomeSecretaryComponent } from './components/secretary/home-secretary/home-secretary.component';
import { AddEventComponent } from './components/secretary/add-event/add-event.component';
import { AddExpenseComponent } from './components/secretary/add-expense/add-expense.component';
import { ExpenseReportComponent } from './components/shared/expense-report/expense-report.component';
import { SignInComponent } from './components/shared/sign-in/sign-in.component';
import { UserProfileComponent } from './components/shared/user-profile/user-profile.component';
import { AuthGuard } from './guards/auth.guard';
import { NavbarComponent } from './components/secretary/navbar/navbar.component';
import { PreviousExpensesComponent } from './components/shared/previous-expenses/previous-expenses.component';
import { HomeManagerComponent } from './components/manager/home-manager/home-manager/home-manager.component';
import { NavbarManagerComponent } from './components/manager/navbar-manager/navbar-manager/navbar-manager.component';
import { DynamicNavbarComponent } from './components/shared/dynamic-navbar/dynamic-navbar.component';

const routes: Routes = [
  // { path: '', component: HomeManagerComponent },
  { path: '', component: SignInComponent },
  { path: 'login', component: SignInComponent },
  { path: 'choose-language', component: ChooseLanguageComponent },
  { path: 'navbar', component: DynamicNavbarComponent,
    children:[
      { path: '', redirectTo: 'home-secretary', pathMatch: 'full' },
      { path: 'home-secretary', component: HomeSecretaryComponent },
      { path: 'add-event', component: AddEventComponent },
      { path: 'add-expense', component: AddExpenseComponent },
      { path: 'expense-report', component: ExpenseReportComponent },      
      { path: 'previous-expenses', component: PreviousExpensesComponent},
    ]},
  {
    path: 'navbar-secretary',
    component: NavbarComponent,
    children: [
      { path: '', redirectTo: 'home-secretary', pathMatch: 'full' },
      { path: 'home-secretary', component: HomeSecretaryComponent },
      { path: 'add-event', component: AddEventComponent },
      { path: 'add-expense', component: AddExpenseComponent },
      { path: 'expense-report', component: ExpenseReportComponent },      
      { path: 'previous-expenses', component: PreviousExpensesComponent},
    ],
  },
  { path: 'home-manager', component:HomeManagerComponent},
  { path: 'navbar-manager', 
    component:NavbarManagerComponent,
    children:[
      { path: '', redirectTo: 'home-secretary', pathMatch: 'full' },
      { path: 'home-secretary', component: HomeSecretaryComponent },
      { path: 'expense-report', component: ExpenseReportComponent },   
      { path: 'previous-expenses', component: PreviousExpensesComponent},
    ],
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
