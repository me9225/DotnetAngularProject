import { Routes } from '@angular/router';
import { LoginComponent } from './components/login/login';
import { Home as UserHome } from './components/user/home/home';
import { Home as ManagerHome } from './components/manager/home/home';
import { RegisterComponent}  from './components/register/register';
import { Random } from './components/manager/random/random';
import { CardsManage } from './components/manager/cards-manage/cards-manage';
import { GiftManage } from './components/manager/gift-manage/gift-manage';
import { DonorManage } from './components/manager/donor-manage/donor-manage';
import { Basket } from './components/user/basket/basket';

export const routes: Routes = [
  { path: '', component: LoginComponent },
  { path: 'login', component: LoginComponent },
  { path: 'user/home', component: UserHome },
  { path: 'user/basket', component: Basket },
  { path: 'manager/home', component: ManagerHome },
  { path: 'register', component: RegisterComponent },
  { path: 'manager/donors', component: DonorManage },
  { path: 'manager/gifts', component: GiftManage },
  { path: 'manager/cards', component: CardsManage },
  { path: 'manager/rand', component: Random },
  { path: '**', redirectTo: '' }
];
