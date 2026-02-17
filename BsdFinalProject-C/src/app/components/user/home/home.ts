import { Component, OnInit, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { SelectModule } from 'primeng/select';
import { TableModule } from 'primeng/table';
import { TagModule } from 'primeng/tag';
import { ToastModule } from 'primeng/toast';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { RippleModule } from 'primeng/ripple';
import { SelectItem } from 'primeng/api';
import { DataViewModule } from 'primeng/dataview';
import { SelectButtonModule } from 'primeng/selectbutton';
import { PanelModule } from 'primeng/panel';
import { GiftModel } from '../../../Models/gift';
import { GiftService } from '../../../Services/gift';
import { DonorService } from '../../../Services/donor';
import { DonorModel } from '../../../Models/donor';
import { CommonModule, CurrencyPipe } from '@angular/common';
import { ChangeDetectorRef } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { jwtDecode } from 'jwt-decode';
import { DialogModule } from 'primeng/dialog';
import { categoryModel } from '../../../Models/category';
import { CategoryService } from '../../../Services/category';
import { error, log } from 'console';
import { BasketService } from '../../../Services/basket';
import { CardService } from '../../../Services/card';
import { BasketModel } from '../../../Models/basket';
import { Router } from '@angular/router';
import { DrawerModule } from 'primeng/drawer';  // הוסף את היבוא הזה

// import { DropdownModule } from 'primeng/SelectModule';

// import { ProductService } from '@/service/productservice';
// import { Product } from '@/domain/product';
// import { Product } from '@/domain/product';
@Component({
  selector: 'app-user-home',
  imports: [DrawerModule, DataViewModule, DialogModule, SelectButtonModule, TagModule, ButtonModule, FormsModule, CommonModule, SelectModule, InputTextModule, PanelModule],

  templateUrl: './home.html',
  styleUrl: './home.scss',
})
export class Home {

  constructor(private cdr: ChangeDetectorRef) { }

  gifts: GiftModel[] = [];
  filteredGifts: GiftModel[] = [];
  giftSrv: GiftService = inject(GiftService);
  donorSrv: DonorService = inject(DonorService);
  cardSrv: CardService = inject(CardService);
  BasketService: BasketService = inject(BasketService);
  donors: DonorModel[] = [];
  donorsName: string[] = [];
  id: number = 0;
  name: string = "";
  description: string = "";
  cost: Number = 0;
  picture: string = "";
  categoryId: Number = 0;
  donorId: number = 0;
  winnerName: string = "";
  displayDialog: boolean = false;
  newGiftid!: number;
  newGiftname!: string;
  newGiftdescription?: string;
  newGiftcost!: number;
  newGiftpicture?: string;
  newGiftcategoryId!: number;
  newGiftdonorId!: number;
  headers: HttpHeaders = new HttpHeaders();
  categories: categoryModel[] = [];
  CategoryService: CategoryService = inject(CategoryService);
  layout: 'list' | 'grid' = 'list';
  selectedDonor: DonorModel | null = null;
  searchByGiftName: string = "";
  displayUpdateDialog: boolean = false;
  updateGiftName: string = "";
  updateGiftDescription: string = "";
  updateGiftCost: number = 0;
  updateGiftPicture: string = "";
  updateGiftCategoryId: Number = 0;
  categoryOptions: SelectItem[] = [];
  selectedCategory: number = 0;
  newbasket: BasketModel = new BasketModel;
  baskets: BasketModel[] = [];
  basketDrawerVisible: boolean = false;  // דגל לפתיחת חלון הצד
  router = inject(Router);
  paymentDialogVisible: boolean = false; // דגל לפתיחת חלון התשלום
  creditCardNumber: string = '';
  creditCardExpiry: string = '';
  creditCardCVV: string = '';

  options: SelectItem[] = [
    { label: 'List', value: 'list' },
    { label: 'Grid', value: 'grid' }
  ];

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
  ngOnInit() {
    try {
      this.headers = this.getHeaders();

      // טוען את כל התורמים
      this.donorSrv.getDonors().subscribe({
        next: (response: DonorModel[]) => {
          this.donors = response;
          this.donorsName = this.donors.map(d => d.name);
          console.log('✓ Donors loaded:', this.donors);
          this.cdr.markForCheck();

          // טוען את כל המתנות אחרי שהתורמים נטענו
          this.giftSrv.getAllGifts().subscribe({
            next: (giftResponse: GiftModel[]) => {
              this.gifts = giftResponse.map((gift: any) => {
                const converted = new GiftModel();
                converted.id = gift.id;
                converted.name = gift.name;
                converted.description = gift.description;
                converted.cost = gift.cost;
                converted.picture = gift.picture;
                converted.categoryId = gift.categoryId;
                converted.donorId = gift.donorId;
                converted.winnerName = gift.winnerName;
                console.log('Processing gift:', converted);
                // חפש את התורם על פי donorId במתנה והשווה ל-Id בתורם
                const donor = this.donorSrv.getOneDonor(converted.donorId).subscribe({
                  next: (donorResponse: DonorModel) => {
                    console.log('Found donor for gift:', donorResponse);
                    this.cdr.markForCheck();
                    converted.donorName = donorResponse.name;
                  },
                  error: (err) => {
                    console.error('Error finding donor for gift:', err);
                    converted.donorName = 'Unknown';
                  }
                });
                return converted;
              });

              this.cdr.markForCheck(); // עדכון ה-UI
              this.filteredGifts = this.gifts;
              console.log("✓ Gifts loaded:", this.gifts);
            },
            error: (err) => {
              console.error('✗ Error loading gifts:', err);
            }
          });
        },
        error: (err) => {
          console.error('✗ Error loading donors:', err);
        }
      });

    } catch (error) {
      console.log('✗ Initialization error:', error);
    }
    this.getAllBaskets(); // קריאה לפונקציה שמביאה את הסלים של המשתמש
  }

  // פונקציה לאישור התשלום
  confirmPayment() {
    if (!this.creditCardNumber || !this.creditCardCVV || !this.creditCardExpiry) {
      alert('אנא מלא את כל פרטי הכרטיס.');
      return;
    }

    // כאן אפשר להוסיף קריאה ל-API לעיבוד תשלום אמיתי
    try {
      this.cardSrv.createNewCards(this.baskets).subscribe({
        next: (response) => {
          console.log('Payment processed successfully:', response);
          this.cdr.markForCheck();
          alert('התשלום בוצע בהצלחה!');
          this.paymentDialogVisible = false;

          // לאחר תשלום מוצלח, אפשר לרוקן את הסל
          this.BasketService.deleteAllBasket(this.getUserIdFromToken(), this.headers).subscribe({
            next: () => {
              this.baskets = [];       // רוקן את הסל מקומית
              this.getAllBaskets();    // רענון הסל מהשרת
              this.paymentDialogVisible = false;
              this.creditCardNumber = '';
              this.creditCardCVV = '';
              this.creditCardExpiry = '';
              this.cdr.markForCheck(); // עדכון ה-UI לאחר ריקון הסל
            },
            error: (error) => {
              console.error('Error clearing basket after payment:', error);
            }
          });

        },
        error: (error) => {
          console.error('Error processing payment:', error);
          alert('אירעה שגיאה בתהליך התשלום. אנא נסה שוב.');
        }
      });
    } catch (error) {
      console.error('Unexpected error during payment:', error);
      alert('אירעה שגיאה בלתי צפויה בתהליך התשלום. אנא נסה שוב.');
    }
    // סגירת הדיאלוג ואיפוס השדות

  }

  // Filter gifts based on search criteria
  getAllBaskets() {
    const headers = this.getHeaders();
    this.BasketService.getAllMyBaskets(headers).subscribe(
      (data) => {
        this.baskets = [...data];  // יצירת reference חדש
        this.baskets.map(basket => {
          const gift = this.giftSrv.getGiftById(basket.giftId).subscribe({
            next: (giftResponse: GiftModel) => {
              basket.giftName = giftResponse.name;
              basket.cost = giftResponse.cost;
              this.cdr.markForCheck();
              this.cdr.detectChanges();
            },
            error: (err) => {
              console.error('Error fetching gift for basket:', err);
              basket.giftName = 'Unknown';
              basket.cost = 0;
            }
          });
        });
        console.log(this.baskets);
        this.cdr.markForCheck();

      },
      (error) => {
        console.error('Error fetching baskets:', error);
      }
    );
  }

  // פונקציה לפתיחת חלון הסל
  openBasketDialog() {
    this.basketDrawerVisible = true;  // פעולה שפותחת את החלון בצד ימין
  }

  // פונקציה למחיקת פריט מהסל
  removeFromBasket(basketId: number) {
    console.log("basketId sent to delete:", basketId); // 👈 בדיקה
    this.BasketService.deleteOneBasket(basketId, this.headers).subscribe(
      () => {
        this.getAllBaskets(); // קריאה נוספת כדי לוודא שהסלים מעודכנים
        this.cdr.markForCheck(); // עדכון ה-UI לאחר המחיקה
      },
      (error) => {
        console.error('Error deleting basket:', error);
      }
    );
  }

  openPaymentDialog() {
    if (this.baskets.length === 0) {
      alert('הסל ריק, אין מה לשלם!');
      return;
    }
    this.paymentDialogVisible = true;
  }

  filterGifts() {
    console.log('Filtering gifts with:');
    console.log('searchByGiftName:', this.searchByGiftName);
    console.log('selectedDonor:', this.selectedDonor?.id);
    console.log('selectedDonor:', this.selectedDonor);
    console.log('All gifts:', this.gifts);

    this.filteredGifts = this.gifts.filter(gift => {

      // Filter by gift name
      const nameMatch = gift.name.toLowerCase().includes(this.searchByGiftName.toLowerCase());

      return nameMatch;
    });

    console.log('Filtered gifts result:', this.filteredGifts);
    this.cdr.markForCheck();
  }

  // Called when gift name search input changes
  onGiftNameChange() {
    this.filterGifts();
  }

  // Called when donor selection changes

  // Clear all filters
  clearFilters() {
    this.searchByGiftName = "";
    this.selectedDonor = null;
    this.filteredGifts = this.gifts;
    this.cdr.detectChanges();
  }

  // Helper function to get donor name by ID
  getDonorName(donorId: number): string {
    console.log(`getDonorName called with donorId: ${donorId} (type: ${typeof donorId})`);

    // ודא שהמערך donors מאוכלס
    if (!this.donors || this.donors.length === 0) {
      console.warn('Donors data is not available yet');
      return 'Unknown';
    }

    const donor = this.donors.find(d => d.id === donorId);
    const result = donor ? donor.name : 'Unknown';

    console.log(`getDonorName result: ${result}`);
    return result;
  }

  // Get image with fallback
  getImageUrl(picture: string | undefined): string {
    return picture || 'assets/no-image.png';
  }

  // Format currency for display
  formatCost(cost: number): string {
    return new Intl.NumberFormat('he-IL', {
      style: 'currency',
      currency: 'ILS'
    }).format(cost);
  }
  // Additional methods for editing gifts can be added here
  addToBasket(gift: GiftModel) {
    console.log('Adding to basket:', gift);
    this.newbasket.giftId = gift.id;
    this.newbasket.userId = this.getUserIdFromToken(); // פונקציה שתוציא את ה-userId מה-token
    if (this.newbasket.userId === 0) {
      console.error('No valid user ID found');
      // הצג הודעה למשתמש שאין טוקן או שהטוקן לא תקין
      return;
    }

    this.BasketService.createNewBasket(this.newbasket, this.headers).subscribe({
      next: (response) => {
        console.log('Gift added to basket successfully:', response);

        this.getAllBaskets();
      },
      error: (error) => {
        console.error('Error adding gift to basket:', error);
        // כאן אפשר להוסיף הודעה למשתמש או לעדכן את ה-UI בהתאם
      }

    });
  }
  getUserIdFromToken(): number {
    const token = localStorage.getItem('token');
    if (!token) {
      console.error('No token found');
      return 0; // טיפול במקרה שאין טוקן
    }
    try {
      const decodedToken: any = jwtDecode(token);
      console.log('Decoded Token:', decodedToken); // הדפס את הטוקן המפוענח
      // בדוק אם ה-nameidentifier נמצא בנתיב אחר בטוקן
      console.log('Token Expiry:', decodedToken.exp);
      const tokenExpiry = decodedToken.exp;
      if (this.isTokenExpired(tokenExpiry)) {
        console.error('Token has expired');
        return 0;  // אל תמשיך בהוספה לעגלת הקניות
      }
      return decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'] || 0;
    } catch (error) {
      console.error('Invalid token:', error);
      return 0; // במקרה של טוקן לא תקין
    }
  }
  // הפונקציה הזו בודקת אם הטוקן פג תוקף
  isTokenExpired(expiry: number): boolean {
    const currentTime = Math.floor(Date.now() / 1000); // זמן נוכחי ב-epoch time (שניות)
    return currentTime > expiry;
  }

  // בדוק אם הטוקן פג תוקף

}


