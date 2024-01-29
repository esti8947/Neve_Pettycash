import { Component, OnInit } from '@angular/core';
import { MenuItem } from 'primeng/api';
import { TranslateService } from '@ngx-translate/core';
import { AuthService } from 'src/app/services/auth-service/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss'],
})
export class NavbarComponent implements OnInit {
  items: MenuItem[] | undefined;
  activeItem: MenuItem | undefined;
  currentUser: any;

  constructor(
    private authService: AuthService,
    private router: Router,
  ) {}
  ngOnInit() {
    this.currentUser = this.authService.getCurrentUser();
  }
  isActiveRoute(route: string): boolean {
    return this.router.isActive(route, true);
  }

  onActiveItemChange(event: MenuItem) {
    this.activeItem = event;
  }

  logOut() {
    this.authService.doLogout();
  }
}
