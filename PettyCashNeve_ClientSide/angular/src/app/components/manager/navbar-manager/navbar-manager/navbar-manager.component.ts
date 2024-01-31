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
  names: any[] = [
    { label: 'Option 1', link: '/option1' },
    { label: 'Option 2', link: '/option2' },
    { label: 'Option 3', link: '/option3' },
    { label: 'Option 4', link: '/option4' },
    { label: 'Option 5', link: '/option5' }
  ];
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

  async loadDetails(){
    this.selectedDepartment = await this.departmentService.getSelectedDepartment();
    this.currentUser = await this.authService.getCurrentUser();
    console.log(this.currentUser, this.selectedDepartment);
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
