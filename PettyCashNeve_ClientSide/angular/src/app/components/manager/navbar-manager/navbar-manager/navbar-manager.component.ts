import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { MenuItem } from 'primeng/api/menuitem';
import { AuthService } from 'src/app/services/auth-service/auth.service';
import { DepartmentService } from 'src/app/services/department-service/department.service';

@Component({
  selector: 'navbar-manager',
  templateUrl: './navbar-manager.component.html',
  styleUrls: ['./navbar-manager.component.scss']
})
export class NavbarManagerComponent {
  dropdownOptions: any[] = [
    { label: 'Option 1', value: 'option1' },
    { label: 'Option 2', value: 'option2' },
    // Add more options as needed
  ];
  items: MenuItem[] | undefined;
  activeItem: MenuItem | undefined;
  selectedDepartment: any;
  currentUser: any;

  constructor(
    private departmentService:DepartmentService,
    private authService:AuthService,
    private router: Router,
  ) {}
  ngOnInit() {
    this.loadDetails();
  }

  loadDetails(){
    this.selectedDepartment = this.departmentService.getSelectedDepartment();
    this.currentUser = this.authService.getCurrentUser();
  }
  isActiveRoute(route: string): boolean {
    return this.router.isActive(route, true);
  }

  onActiveItemChange(event: MenuItem) {
    this.activeItem = event;
  }

  logOut() {
    this.departmentService.deactivateSelectedDepartment();
    this.router.navigate(['/home-manager']);
  }
}
