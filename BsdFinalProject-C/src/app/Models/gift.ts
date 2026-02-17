import { CardModel } from "./card";

export class GiftModel {
  id!: number;
  name!: string;
  description?: string;
  cost!: number;
  picture?: string;
  categoryId!: number;
  donorId!: number; // קשר בין המתנה לתורם
  winnerName?: string;
  tickets?: CardModel[];
  donorName?: string; // שדה נוסף לשם התורם
}