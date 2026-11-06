import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { UserProfile } from '../../features/profile/models/user-profile.model';
import { UpdateProfileRequest } from '../../features/profile/models/update-profile-request.model';

import { environment } from '../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class ProfileService {
  private readonly http = inject(HttpClient);

  private readonly base = environment.apiUrl;

  profile() {
    return this.http.get<UserProfile>(`${this.base}/api/users`);
  }

  updateProfile(data: UpdateProfileRequest) {
    return this.http.put<void>(`${this.base}/api/users/profile`, data);
  }

  updatePassword(data: { currentPassword: string; newPassword: string }) {
    return this.http.put<void>(`${this.base}/api/users/password`, data);
  }

  deleteAccount() {
    return this.http.delete<void>(`${this.base}/api/users`);
  }
}
