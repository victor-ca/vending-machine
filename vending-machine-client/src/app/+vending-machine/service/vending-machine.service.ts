import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { API_ENDPOINT } from 'src/app/auth/endpoint';
import { Product } from 'src/app/model/product';
import { CoinBank } from '../store/vending-machine.store';
import { PurchaseResult } from './purchase-result';
@Injectable({
  providedIn: 'root',
})
export class VendingMachineService {
  constructor(
    @Inject(API_ENDPOINT) private readonly apiEndpoint: string,
    private readonly httpClient: HttpClient
  ) {}

  public getBank(): Observable<CoinBank> {
    return this.httpClient.get<CoinBank>(`${this.apiEndpoint}/coins`);
  }

  public loadProducts(): Observable<Product[]> {
    return this.httpClient.get<Product[]>(
      `${this.apiEndpoint}/products/for-sale`
    );
  }

  purchase(
    productName: string,
    desiredAmount: number
  ): Observable<PurchaseResult> {
    return this.httpClient.post<PurchaseResult>(`${this.apiEndpoint}/buy`, {
      productName,
      desiredAmount,
    });
  }

  public insertCoin(denomination: number): Observable<CoinBank> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
    });
    return this.httpClient.post<CoinBank>(
      `${this.apiEndpoint}/deposit`,
      JSON.stringify(denomination),
      { headers }
    );
  }

  public reset(): Observable<unknown> {
    return this.httpClient.delete(`${this.apiEndpoint}/coins`);
  }
}
