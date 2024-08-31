import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ProductDto } from '../../models/product.dto';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  private apiUrl = `${environment.apiUrl}/api/product`;

  constructor(private http: HttpClient) { }

  getAllProducts(): Observable<ProductDto[]> {
    return this.http.get<ProductDto[]>(`${this.apiUrl}/all`);
  }

  getProductById(productId: number): Observable<ProductDto> {
    return this.http.get<ProductDto>(`${this.apiUrl}/${productId}`)
  }
}
