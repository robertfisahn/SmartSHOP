import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { RegisterUserDto } from '../../models/registerUser.dto';
import { CartService } from '../cart/cart.service';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})

export class AccountService {
  private apiUrl = `${environment.apiUrl}/api/account`;

  constructor(private http: HttpClient, private cartService: CartService) { }

  login(email: string, password: string): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/login`, { email, password }).pipe(
      tap(response => {
        if (response && response.token) {
          sessionStorage.setItem('token', response.token);
          sessionStorage.setItem('userId', response.userId);
          sessionStorage.setItem('userEmail', response.userEmail);

          this.cartService.updateCartCount(); 
        }
      })
    );
  }

  register(user: RegisterUserDto): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/registration`, user );
  }

  logout(): void {
    sessionStorage.removeItem('token');
    sessionStorage.removeItem('userId');
    sessionStorage.removeItem('userEmail');
  }

  isLoggedIn(): boolean {
    return !!sessionStorage.getItem('token');
  }

  getUserEmail(): string | null {
    return sessionStorage.getItem('userEmail');
  }

}
