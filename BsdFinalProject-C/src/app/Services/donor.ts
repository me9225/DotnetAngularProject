import { Observable } from 'rxjs';
import { LoginModel } from '../Models/login';
import {DonorModel} from '../Models/donor'
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
@Injectable({
  providedIn: 'root',
})
export class DonorService {
  BASE_URL = 'https://localhost:7026/api/Donors';
  http: HttpClient = inject(HttpClient);
  constructor() { }

  createDonor(item:DonorModel, headers?: HttpHeaders): Observable<DonorModel> {
    return this.http.post<DonorModel>(this.BASE_URL, item, { headers });
  }

  updateDonor(item: DonorModel, headers?: HttpHeaders): Observable<DonorModel> {
    return this.http.put<DonorModel>(`${this.BASE_URL}`, item, { headers });
  }

  getDonors(): Observable<DonorModel[]> {
     return this.http.get<DonorModel[]>(`${this.BASE_URL}`);
  }

  getOneDonor(id: number): Observable<DonorModel> {
     return this.http.get<DonorModel>(`${this.BASE_URL}/${id}`);
  }

  deleteDonor(id: Number, headers?: HttpHeaders): Observable<DonorModel> {
    return this.http.delete<DonorModel>(`${this.BASE_URL}/${id}`, { headers });
  }

  getDonorGifts(donorId: Number ,headers?: HttpHeaders): Observable<any[]> {
    return this.http.get<any[]>(`${this.BASE_URL}/${donorId}/gifts`, { headers });
  }

  
}
