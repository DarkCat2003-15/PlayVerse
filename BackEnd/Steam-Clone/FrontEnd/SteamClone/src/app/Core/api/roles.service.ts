import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';
import { environment } from '../../../environment/environment';
import { ApiResponse } from './models/api.interface';
import { Permission, Role, UserAccount, UserRoleAssignment } from './models/roles.interface';

@Injectable({ providedIn: 'root' })
export class RolesService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = environment.apiUrl;

  getRoles(): Observable<Role[]> {
    return this.http
      .get<ApiResponse<Role[]>>(`${this.apiUrl}/roles`)
      .pipe(map((response) => response.data ?? []));
  }

  getPermissions(): Observable<Permission[]> {
    return this.http
      .get<ApiResponse<Permission[]>>(`${this.apiUrl}/roles/permissions`)
      .pipe(map((response) => response.data ?? []));
  }

  getMyRoles(): Observable<Role[]> {
    return this.http
      .get<ApiResponse<Role[]>>(`${this.apiUrl}/roles/me`)
      .pipe(map((response) => response.data ?? []));
  }

  assignRole(userId: string, roleId: string): Observable<UserRoleAssignment> {
    return this.http
      .post<ApiResponse<UserRoleAssignment>>(`${this.apiUrl}/roles/assign`, { userId, roleId })
      .pipe(map((response) => response.data));
  }

  removeRole(userId: string, roleId: string): Observable<boolean> {
    return this.http
      .delete<ApiResponse<boolean>>(`${this.apiUrl}/roles/users/${userId}/roles/${roleId}`)
      .pipe(map((response) => response.data));
  }

  getUsers(search = ''): Observable<UserAccount[]> {
    let params = new HttpParams().set('limit', 100);

    if (search.trim()) {
      params = params.set('username', search.trim());
    }

    return this.http
      .get<ApiResponse<UserAccount[]>>(`${this.apiUrl}/user`, { params })
      .pipe(map((response) => response.data ?? []));
  }
}
