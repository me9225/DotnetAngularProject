import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CardsManage } from './cards-manage';

describe('CardsManage', () => {
  let component: CardsManage;
  let fixture: ComponentFixture<CardsManage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CardsManage]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CardsManage);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
