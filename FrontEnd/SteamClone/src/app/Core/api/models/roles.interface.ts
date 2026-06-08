export interface Permission {
  id: string;
  code: string;
  module: string;
  action: string;
  name: string;
  description?: string | null;
  specificity: string;
}

export interface Role {
  id: string;
  name: string;
  description?: string | null;
  isActive: boolean;
  permissions: Permission[];
}

export interface UserRoleAssignment {
  userId: string;
  roleId: string;
  roleName: string;
  assignedAt: string;
  assignedBy?: string | null;
}

export interface UserAccount {
  userId: string;
  email: string;
  username: string;
  createdAt: string;
  statusId: number;
  statusName: string;
  isActive: boolean;
  roles: string[];
}
