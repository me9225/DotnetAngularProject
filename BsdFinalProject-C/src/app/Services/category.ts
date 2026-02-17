import { Observable } from 'rxjs';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { categoryModel } from '../Models/category';
import { inject, Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class CategoryService {
    // Get all categories

    BASE_URL = 'https://localhost:7026/api/Categories';
    http: HttpClient = inject(HttpClient);
    constructor() { }

  
  getAllCategories( headers?: HttpHeaders): Observable<categoryModel[]> {
    return this.http.get<categoryModel[]>(this.BASE_URL);
  }

  // Get category by ID
  getCategoryById(id: number, headers?: HttpHeaders): Observable<categoryModel> {
    return this.http.get<categoryModel>(`${this.BASE_URL}/${id}`, { headers });
  }

}
