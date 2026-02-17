export class BasketModel  {
    id!: number;
    giftId!: number;
    userId!: number;
    giftName!: string;
    cost!: number;
}
export class CreateBasketDto { 
    giftId!: number;
    userId!: number;
}
