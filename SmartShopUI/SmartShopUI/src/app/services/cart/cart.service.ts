import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { BehaviorSubject, Observable, map, tap } from 'rxjs';
import { CartItem } from '../../models/cartItem.dto.';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class CartService {
  private apiUrl =`${environment.apiUrl}/api/cart`;

  private cartItemCount = new BehaviorSubject<number>(0);
  constructor(private http: HttpClient) {
    this.initializeCartCount();
  }

  private initializeCartCount(): void {
    const userId = Number(sessionStorage.getItem('userId'));
    if (userId) {
      this.getTotalQuantity(userId).subscribe(count => {
        this.cartItemCount.next(count);
      });
    }
  }

  addItemToCart(productId: number, quantity: number = 1): Observable<any> {
    const body = { productId, quantity };
    const token = sessionStorage.getItem('token');
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
    return this.http.post(`${this.apiUrl}/add`, body, { headers }).pipe(
      map(() => this.updateCartCount())
    );
  }

  getCart(userId: number): Observable<any> {
    const token = sessionStorage.getItem('token');
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });

    return this.http.get(`${this.apiUrl}/${userId}`, { headers });
  }

  removeItemFromCart(itemId: number): Observable<any> {
    const token = sessionStorage.getItem('token');
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
    return this.http.delete(`${this.apiUrl}/delete/${itemId}`, { headers }).pipe(
      map(() => this.updateCartCount())
    );
  }

  updateCartItem(itemId: number, quantity: number): Observable<any> {
    const body = { quantity };
    const token = sessionStorage.getItem('token');
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
    return this.http.put(`${this.apiUrl}/update/${itemId}`, body, { headers }).pipe(
      map(() => this.updateCartCount())
    );
  }

  getCartItemCount(): Observable<number> {
    return this.cartItemCount.asObservable();
  }

  getTotalQuantity(userId: number): Observable<number> {
    return this.getCart(userId).pipe(
      map((items: CartItem[]) => {
        return items.reduce((total, item) => total + item.quantity, 0);
      })
    );
  }
  
  updateCartCount(): void {
    const userId = Number(sessionStorage.getItem('userId'));
    if (userId) {
      this.getTotalQuantity(userId).subscribe(count => {
        this.cartItemCount.next(count);
      });
    }
  }

  clearCart(userId: number): Observable<any> {
    const token = sessionStorage.getItem('token');
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
    return this.http.delete(`${this.apiUrl}/clear/${userId}`, { headers }).pipe(
      tap(() => this.cartItemCount.next(0))
    );
  }

  getCartItemQuantity(productId: number): Observable<number> {
    const userId = Number(sessionStorage.getItem('userId'));
    return this.getCart(userId).pipe(
      map((items: CartItem[]) => {
        const item = items.find(i => i.productId === productId);
        return item ? item.quantity : 0;
      })
    );
  }
}
