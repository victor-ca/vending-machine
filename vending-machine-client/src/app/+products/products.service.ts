import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { API_ENDPOINT } from '../auth/endpoint';
import { Product } from '../model/product';

@Injectable({
  providedIn: 'root',
})
export class ProductsServiceService {
  constructor(
    @Inject(API_ENDPOINT) private readonly apiEndpoint: string,
    private readonly httpClient: HttpClient
  ) {}

  public getOwned(): Observable<Product[]> {
    return this.httpClient.get<Product[]>(`${this.apiEndpoint}/products/owned`);
  }

  public createProduct(product: Product): Observable<Product> {
    return this.httpClient.post<Product>(
      `${this.apiEndpoint}/products`,
      product
    );
  }

  public deleteProduct(productName: string): Observable<Product> {
    return this.httpClient.delete<Product>(
      `${this.apiEndpoint}/products/${productName}`
    );
  }

  public updateProduct(
    originalName: string,
    update: Product
  ): Observable<Product> {
    return this.httpClient.put<Product>(
      `${this.apiEndpoint}/products/${originalName}`,
      update
    );
  }
}
