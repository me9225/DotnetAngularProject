
import { Observable } from 'rxjs';
import { GiftModel } from '../Models/gift';
import { CardModel } from '../Models/card';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class GiftService {
     BASE_URL = 'https://localhost:7026/api/Gifts'; 
      http: HttpClient = inject(HttpClient);

  constructor() {}

  getAllGifts(headers?: HttpHeaders): Observable<GiftModel[]> {
    return this.http.get<GiftModel[]>(this.BASE_URL, { headers });
  }

  getGiftById(id: number, headers?: HttpHeaders): Observable<GiftModel> {
    return this.http.get<GiftModel>(`${this.BASE_URL}/${id}`, { headers });
  }

  createGift(gift: GiftModel, headers?: HttpHeaders): Observable<GiftModel> {
    return this.http.post<GiftModel>(this.BASE_URL, gift, { headers });
  }

  updateGift(gift: GiftModel, headers?: HttpHeaders): Observable<GiftModel> {
    return this.http.put<GiftModel>(`${this.BASE_URL}/${gift.id}`, gift, { headers });
  }

  deleteGift(id: number,headers?: HttpHeaders): Observable<boolean> {
    return this.http.delete<boolean>(`${this.BASE_URL}/${id}`);
  }

  getGiftsByCategory(categoryId: number): Observable<GiftModel[]> {
    return this.http.get<GiftModel[]>(
      `${this.BASE_URL}/category/${categoryId}`
    );
  }

  getGiftsByCost(price1: number, price2: number): Observable<GiftModel[]> {
    return this.http.get<GiftModel[]>(
      `${this.BASE_URL}/cost/${price1}/${price2}`
    );
  }
  getAllCards(giftId: number, headers?: HttpHeaders): Observable<CardModel[]> {
  return this.http.get<CardModel[]>(`${this.BASE_URL}/allCards/${giftId}`, { headers });
}
}
