import { ChangeDetectorRef, Component, OnInit, inject } from '@angular/core';
import { GiftModel } from '../../../Models/gift';
import { GiftService } from '../../../Services/gift';
import { WinnerService } from '../../../Services/winner';
import { CommonModule } from '@angular/common';
import { HttpHeaders } from '@angular/common/http';
import { jwtDecode } from 'jwt-decode';
import { NgModule } from '@angular/core';
import { TableModule } from 'primeng/table'; // הוספת המודול
import { FormsModule } from '@angular/forms';
import { SelectModule } from 'primeng/select';
import { TagModule } from 'primeng/tag';
import { ToastModule } from 'primeng/toast';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { RippleModule } from 'primeng/ripple';
import { SelectItem } from 'primeng/api';
import { DonorModel } from '../../../Models/donor';
import { DonorService } from '../../../Services/donor';
import { HttpClient } from '@angular/common/http';
import { DialogModule } from 'primeng/dialog';

@Component({
  selector: 'app-random',
  imports: [SelectModule, TableModule, ButtonModule, CommonModule],
  templateUrl: './random.html',
  styleUrl: './random.scss',
})
export class Random implements OnInit {
  gifts: GiftModel[] = [];
  giftService = inject(GiftService);
  winnerService = inject(WinnerService);
  changeDetectorRef = inject(ChangeDetectorRef);
  headers: HttpHeaders = new HttpHeaders();

  private getHeaders(): HttpHeaders {
    const token = localStorage.getItem('token');
    let userRole = '';
    if (token) {
      const decodedToken: any = jwtDecode(token);
      userRole =
        decodedToken[
          'http://schemas.microsoft.com/ws/2008/06/identity/claims/role'
        ] || '';
    }
    return new HttpHeaders({
      'User-Role': userRole,
      Authorization: token ? `Bearer ${token}` : '',
    });
  }

  ngOnInit(): void {
    this.headers = this.getHeaders();
    this.loadGifts(); // מבצע את הקריאה האסינכרונית כדי למלא את המערך

    // נוסיף צופה לתוצאה של loadGifts (אחרי ש-HTTP חוזר)
    // נדאג ש-next יקרה רק אחרי שהמתנות נטענות
  }

  loadGifts(): void {
    this.giftService.getAllGifts().subscribe((r) => {
      this.gifts = r; // מעדכן את המערך
      this.changeDetectorRef.detectChanges(); // זה יגרום ל-Angular לבדוק את השינויים ולעדכן את התצוגה

      // אחרי שגמרנו עם ה-HTTP call והמערך התמלא, עכשיו נבצע את קריאות ה-API לכל מתנה
      this.gifts.forEach((g) => {
        this.giftService.getAllCards(g.id, this.headers).subscribe({
          next: (cards) => {
            g.tickets = cards; // עדכון הכרטיסים
            console.log(g.tickets);
            this.changeDetectorRef.detectChanges(); // הוספנו כאן כדי לעדכן את ה-view לאחר שינוי הנתונים
          },
          error: (err) => {
            console.error('Error fetching cards for gift', err);
          },
        });
      });
    });
  }

  drawWinner(gift: GiftModel): void {
    if (gift.winnerName !=" ") {
      alert("למתנה זו כבר יש זוכה"+gift.winnerName+"!")
      console.log("winner exist",`${gift.winnerName}`);
      
      return;
    }
    this.winnerService.addWinner(gift.id, this.headers).subscribe({
      next: (w) => {
        gift.winnerName = w?.winnerName;
        this.changeDetectorRef.detectChanges();
        console.log();
        
        // כדי לוודא שהשינוי ייתפס על ידי Angular
      },
      error: (err) => {
        if (err.status === 404) {
          // במקרה של שגיאת NotFound (אין רוכשים)
          alert('אין רוכשים עבור המתנה הזו!');
        } else if (err.status === 400) {
          // במקרה של שגיאת BadRequest (שגיאה אחרת)
          alert('שגיאה בהוספת הזוכה: ' + err.error.message);
        } else {
          // טיפול בשגיאות כלליות אחרות
          alert('אירעה שגיאה לא צפויה.');
        }
      },
    });
  }

  buyersCount(gift: GiftModel): number {
    return gift.tickets ? gift.tickets.length : 0;
  }

  // פונקציה למחיקת כל הזוכים
  deleteAllWinners(): void {
    this.winnerService.deleteAllWinners(this.headers).subscribe({
      next: (success) => {
        if (success) {
          // מחיקת הזוכים מה-UI
          this.gifts.forEach((gift) => (gift.winnerName = ' '));
          this.changeDetectorRef.detectChanges(); // וודא שהתצוגה תעדכן
        } else {
          // במקרה של כישלון במחיקה
          alert('שגיאה במחקת הזוכים');
        }
      },
      error: (err) => {
        // טיפול בשגיאות ספציפיות (אם אין זוכים, או שגיאה אחרת)
        if (err.status === 404) {
          alert('אין זוכים למחוק');
        } else if (err.status === 400) {
          alert('שגיאה בבקשה: ' + err.error.message); // אם יש הודעה ספציפית מהשרת
        } else {
          alert('אירעה שגיאה לא צפויה');
        }
      },
    });
  }
}
