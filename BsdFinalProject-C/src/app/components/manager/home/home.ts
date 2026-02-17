import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MenubarModule } from 'primeng/menubar';
import { MenuItem } from 'primeng/api';
import { Router } from '@angular/router';
import { GiftManage } from '../gift-manage/gift-manage';
import { CardsManage } from '../cards-manage/cards-manage';
import { Random } from '../random/random';
import { DonorManage } from '../donor-manage/donor-manage';

@Component({
  selector: 'app-manager-home',
  imports: [CommonModule, MenubarModule, GiftManage, CardsManage, Random, DonorManage],
  templateUrl: './home.html',
  styleUrl: './home.scss',
})
export class Home implements OnInit {
items: MenuItem[] = [];
selectedComponent: string = 'home'; // הקומפוננטה המוצגת כרגע

 ngOnInit() {
        this.items = [
            {
                label: 'דף הבית',
                icon: 'pi pi-fw pi-home',
                command: () => this.selectedComponent = 'home'
            },
            {
                label: 'ניהול תורמים',
                icon: 'pi pi-fw pi-users',
                command: () => this.selectedComponent = 'donors'
            },
            {
                label: 'ניהול מתנות',
                icon: 'pi pi-fw pi-gift',
                command: () => this.selectedComponent = 'gifts'
            },
            {
                label: 'ניהול כרטיסים',
                icon: 'pi pi-fw pi-ticket',
                command: () => this.selectedComponent = 'cards'
            },
            {
                label: '🎁הגרלת מתנות',
                command: () => this.selectedComponent = 'random'
            }
        ];
}
}

