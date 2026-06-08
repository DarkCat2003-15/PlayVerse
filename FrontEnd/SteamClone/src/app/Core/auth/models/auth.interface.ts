export interface LoginRequest {
  email: string;
  password: string;
}

export interface SignUpRequest {
  email: string;
  username: string;
  password: string;
}

export interface UpdateAccountRequest {
  email: string;
  username: string;
}

export interface AuthUser {
  userId: string;
  username: string;
  email: string;
  statusId: number;
  statusName: string;
  isActive: boolean;
  roles: string[];
  permissions: string[];
}

export interface LoginResponse {
  token: string;
  refreshToken: string;
  userId: string;
  email: string;
  username: string;
  statusId: number;
  statusName: string;
  isActive: boolean;
  roles: string[];
  permissions: string[];
}

export type RefreshTokenResponse = LoginResponse;
