export interface ApiResponse<T> {
  message: string;
  errors: string[];
  data: T;
  success: boolean;
  count?: number;
}
