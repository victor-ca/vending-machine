import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { API_ENDPOINT } from '../auth/endpoint';
import { Product } from '../model/product';
import { CoinBank } from './store/vending-machine.store';

@Injectable({
  providedIn: 'root',
})
export class VendingMachineService {
  constructor(
    @Inject(API_ENDPOINT) private readonly apiEndpoint: string,
    private readonly httpClient: HttpClient
  ) {}

  public getBank(): Observable<CoinBank> {
    return this.httpClient.get<CoinBank>(
      `${this.apiEndpoint}/vending-machine/coins`
    );
  }

  public insertCoin(denomination: number): Observable<CoinBank> {
    return this.httpClient.post<CoinBank>(
      `${this.apiEndpoint}/vending-machine/coins`,
      JSON.stringify(denomination)
    );
  }

  public reset(): Observable<unknown> {
    return this.httpClient.delete(`${this.apiEndpoint}/vending-machine/coins`);
  }
}
