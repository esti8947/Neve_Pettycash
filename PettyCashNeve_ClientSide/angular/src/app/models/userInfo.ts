import { Department } from './department';
import { Role } from './role';

export class UserInfo {
  username: string | undefined;
  department: Department | undefined;
  isManager?: boolean;
  loggedIn?: boolean;
  token?: string;
  roles: Role[] | undefined;
}
