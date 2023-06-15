export interface ProductsPurchase {
  orderId: string;
  orderTime: Date;
  productId: string;
  productname: string;
  description: string;
  orderedQuant: number;
  unitPrice: number;
  totalPrice: number;
  store: {
    storeId: string;
    storeName: string;
    storeLoc: string;
  };
}



