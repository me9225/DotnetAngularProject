import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { inject } from '@angular/core';


@Injectable({
  providedIn: 'root',
})
export class WinnerService {
  BASE_URL = 'https://localhost:7026/api/Winners';
  http: HttpClient = inject(HttpClient);
  addWinner(giftId: number, headers?: HttpHeaders): Observable<any> {
    return this.http.post<any>(`${this.BASE_URL}?giftId=${giftId}`, {}, { headers });
  }

  getAllWinners(headers?: HttpHeaders): Observable<any[]> {
    return this.http.get<any[]>(this.BASE_URL, { headers });
  }

  deleteAllWinners(headers?: HttpHeaders): Observable<boolean> {
    return this.http.delete<boolean>(this.BASE_URL, { headers });
  }

}
