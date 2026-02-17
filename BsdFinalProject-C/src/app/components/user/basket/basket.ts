import { Component, OnInit } from '@angular/core';
import { BasketService } from '../../../Services/basket';
import { BasketModel } from '../../../Models/basket';
import { CreateBasketDto } from '../../../Models/basket';
import { HttpHeaders } from '@angular/common/http';
import { jwtDecode } from 'jwt-decode';

@Component({
  selector: 'app-basket',
  imports: [],
  templateUrl: './basket.html',
  styleUrl: './basket.scss',
})
export class Basket implements OnInit{
  baskets: BasketModel[] = [];
  headers: HttpHeaders=new HttpHeaders();
  constructor(private basketService: BasketService) { }
  private getHeaders(): HttpHeaders {
    const token = localStorage.getItem('token');
    let userRole = '';
    if (token) {
      const decodedToken: any = jwtDecode(token);
      userRole = decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] || '';
    }
    return new HttpHeaders({
      'User-Role': userRole,
      'Authorization': token ? `Bearer ${token}` : ''
    });
  }
  ngOnInit(): void {
    this.headers = this.getHeaders();  
    this.getAllBaskets();
  }

   getAllBaskets() {
    const headers = this.getHeaders();
    this.basketService.getAllMyBaskets(headers).subscribe(
      (data) => {
        this.baskets = data;
      },
      (error) => {
        console.error('Error fetching baskets:', error);
      }
    );
  }

  // פונקציה ליצירת סל חדש
  createBasket() {
    const newBasket = new CreateBasketDto(); // לדוגמה, giftId = 101, userId = 1
    this.basketService.createNewBasket(newBasket,this.headers).subscribe(
      (data) => {
        console.log('New basket created:', data);
        this.getAllBaskets(); // עדכון הסלים אחרי יצירת סל חדש
      },
      (error) => {
        console.error('Error creating basket:', error);
      }
    );
  }

  // פונקציה למחיקת סל
  deleteBasket(id: number) {
    this.basketService.deleteOneBasket(id).subscribe(
      (data) => {
        console.log('Basket deleted:', data);
        this.getAllBaskets(); // עדכון הסלים אחרי מחיקה
      },
      (error) => {
        console.error('Error deleting basket:', error);
      }
    );
  }
}