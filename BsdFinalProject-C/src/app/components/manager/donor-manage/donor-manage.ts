import { Component, OnInit, inject, ChangeDetectorRef, NgZone } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { SelectModule } from 'primeng/select';
import { TableModule } from 'primeng/table';
import { TagModule } from 'primeng/tag';
import { ToastModule } from 'primeng/toast';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { RippleModule } from 'primeng/ripple';
import { SelectItem } from 'primeng/api';
import { DonorModel } from '../../../Models/donor';
import { DonorService } from '../../../Services/donor';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { DialogModule } from 'primeng/dialog';
import { GiftModel } from '../../../Models/gift';
import { HttpHeaders } from '@angular/common/http';
import { jwtDecode } from 'jwt-decode';
import { ViewEncapsulation } from '@angular/core';



@Component({
  selector: 'app-donor-manage',
   imports: [SelectModule, TableModule, TagModule, ToastModule, ButtonModule, InputTextModule, RippleModule, FormsModule, CommonModule, DialogModule],
  templateUrl: './donor-manage.html',
  styleUrl: './donor-manage.scss',
  encapsulation: ViewEncapsulation.None

})
export class DonorManage {
   
     donorSrv: DonorService = inject(DonorService );
     cdr: ChangeDetectorRef = inject(ChangeDetectorRef);
     ngZone: NgZone = inject(NgZone);
     donors:DonorModel[]=[];
     displayDialog: boolean = false;
     newDonorName: string = "";
     newDonorEmail: string = "";
     editingDonor: DonorModel | null = null;
     clonedDonors: Map<Number, DonorModel> = new Map();
     headers: HttpHeaders=new HttpHeaders();
    // gifts dialog state
    displayGiftsDialog: boolean = false;
    donorGifts: GiftModel[] = [];

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



    ngOnInit():void {
     //הבאת רשימת התורמים מהשרת
     try {
         this.headers = this.getHeaders();  
         this.donorSrv.getDonors().subscribe({       
          next: (response: DonorModel[]) => {
          console.log('הנתונים התקבלו:', response);
          console.log('מספר תורמים:', response.length);
            setTimeout(() => {
              // המרת הנתונים לפורמט הנכון
              const convertedDonors = response.map((donor: any) => {
                const converted = new DonorModel();
                converted.id = donor.id || donor.Id;
                converted.name = donor.name || donor.Name;
                converted.Email = donor.email || donor.Email;
                console.log('Converted donor:', converted);
                return converted;
              });
              this.donors = convertedDonors;
              console.log('Final donors array:', this.donors);
              this.cdr.markForCheck();
            }, 0);
           },
          error: (err) => {
            console.log('Login error:', err);
      }  
        })
       } catch {
         alert('הבקשה נכשלה');
       }
    }

// ngOnInit(): void {
//     alert(1);
//     this.donorSrv.getDonors().subscribe({
//       next: (response: DonorModel[]) => {
//         alert(3);
//         this.donors = response;
//       }
//     });
//   }

//מחיקת תורם - עדכון אופטימיסטי של ה-UI ומחיקה בשרת
deleteDonor(donor: DonorModel) {
  if (!confirm(`האם אתה בטוח שברצונך למחוק את התורם: ${donor.name}?`)) {
    return;
  }
  // שמור עותק לביטול במקרה של שגיאה
  const previous = [...this.donors];
  // עדכון מיידי של ה-UI (אופטימיסטי)
  this.donors = this.donors.filter(d => d.id !== donor.id);
  // דחיית סימון שינוי כדי לא לגרום ל-ExpressionChangedAfterItHasBeenCheckedError
  setTimeout(() => this.cdr.markForCheck(), 0);

  this.donorSrv.deleteDonor(donor.id,this.headers).subscribe({
    next: (response) => {
      console.log('התורם נמחק בהצלחה', response);
    },
    error: (err) => {
      console.log('שגיאה במחיקת התורם', err);
      // החזר את ה-UI למצב הקודם אם המחיקה נכשלה
      this.donors = previous;
      setTimeout(() => this.cdr.markForCheck(), 0);
      alert('שגיאה במחיקת התורם: ' + (err.error?.message || err.statusText || err.message));
    }
  });
}

//שמירת שינויים בתורם
async editDonor(donor: DonorModel) {
  // כאן תוכל לפתוח טופס עריכה או לבצע פעולות אחרות
  console.log('עריכת תורם:', donor);
  // לדוגמה, נניח שאתה רוצה לשמור את השינויים מיד
  await this.saveChanges(donor);
}

saveChanges(donor: DonorModel) {
  // שלח עדכון לשרת, ועדכן את ה-UI לאחר אישור
  this.donorSrv.updateDonor(donor,this.headers).subscribe({
    next: (response) => {
      console.log('העדכון בוצע בהצלחה', response);
      // דחיית סיום מצב העריכה כדי למנוע ExpressionChangedAfterItHasBeenCheckedError
      setTimeout(() => this.cdr.markForCheck(), 0);
    },
    error: (err) => {
      console.log('שגיאה בשמירת השינויים', err);
      alert('שגיאה בשמירת השינויים: ' + (err.error?.message || err.message));
    }
  });
}

openAddDonorDialog() {
  this.newDonorName = "";
  this.newDonorEmail = "";
  this.displayDialog = true;
}

closeDialog() {
  this.displayDialog = false;
}

addNewDonor() {
  if (!this.newDonorName.trim() || !this.newDonorEmail.trim()) {
    alert('אנא מלא את כל השדות');
    return;
  }
    // create payload shaped for the API (lowercase keys)
    //יצירת אובייקט מסוג donormodel עם הנתונים מהטופס
    const payload = new DonorModel();
    payload.name = this.newDonorName.trim();
    payload.Email = this.newDonorEmail.trim();
      
    console.log('שולח יצירת תורם עם הנתונים:', JSON.stringify(payload));
    this.donorSrv.createDonor(payload,this.headers).subscribe({
      next: (response) => {
        console.log('תורם חדש נוצר בהצלחה:', response);
        // normalize response and add to list
        const converted = new DonorModel();
        const r: any = response;
        converted.id = r.id || r.Id;
        converted.name = r.name || r.Name;
        converted.Email = r.email || r.Email;
        // עדכון ה-UI מיידית
        this.donors = [...this.donors, converted];
        // סגור הדיאלוג לאחר עיגון שינוי (הימנעות מבעיות בדיקה)
        setTimeout(() => {
          this.closeDialog();
          this.cdr.markForCheck();
        }, 0);
      },
      error: (err) => {
        console.log('שגיאה בהוספת תורם:', err);
        setTimeout(() => this.closeDialog(), 0);
        alert('שגיאה בהוספת התורם: ' + (err.error?.message || err.message));
      }
    });
}

onEditInit(donor: DonorModel) {
  console.log('התחיל עריכה לתורם:', donor);
  this.clonedDonors.set(donor.id, JSON.parse(JSON.stringify(donor)));
  this.editingDonor = donor;
}

onEditSave(donor: DonorModel) {
  console.log('שולח לעדכון תורם מלא:', JSON.stringify(donor));
  console.log('ID:', donor.id, 'Name:', donor.name, 'Email:', donor.Email);
  // call update and defer UI state reset to avoid ExpressionChangedAfterItHasBeenCheckedError
  this.donorSrv.updateDonor(donor,this.headers).subscribe({
    next: (response) => {
      console.log('התורם עודכן בהצלחה:', response);
      // remove clone and exit edit mode in next macrotask
     
        this.clonedDonors.delete(donor.id );
        this.editingDonor = null;
        setTimeout(() => this.cdr.markForCheck(), 0);
      
    },
    error: (err) => {
      console.log('שגיאה בעדכון התורם:', err);
      console.log('Response status:', err.status);
      console.log('Response body:', JSON.stringify(err.error));
      alert('שגיאה בעדכון התורם: ' + (err.error?.message || err.message));
    }
  });
  
}

onEditCancel(donor: DonorModel) {
  const cloned = this.clonedDonors.get(donor.id);
  if (cloned) {
    donor.name = cloned.name;
    donor.Email = cloned.Email;
    this.clonedDonors.delete(donor.id);
  }
  this.editingDonor = null;
}

// הצגת מתנות של תורם
viewDonorGifts(donor: DonorModel) {
  if (!donor || !donor.id) return;
  this.donorSrv.getDonorGifts(donor.id,this.headers).subscribe({
    next: (gifts) => {
      this.donorGifts = gifts || [];
      // פתיחת הדיאלוג אחרי שהנתונים התקבלו
      this.displayGiftsDialog = true;
      setTimeout(() => this.cdr.markForCheck(), 0);
    },
    error: (err) => {
      console.error('שגיאה בקבלת מתנות לתורם', err);
      alert('שגיאה בקבלת מתנות לתורם: ' + (err.error?.message || err.message));
    }
  });
}
    a(){
       try {
         alert(1)
         this.donorSrv.getDonors().subscribe({
           next: (response: DonorModel[]) => {
            this.donors=[...response];
        }})
       } catch (err) {
         alert('הבקשה נכשלה');
       }
    }

   

}
