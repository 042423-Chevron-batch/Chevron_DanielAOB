import { DecimalPipe } from "@angular/common";

export interface Order {

    orderId: string;
    orderTime: Date;
    customerId: string;
    custFirstName: string;
    custUserName: string;
    custLastName: string
    custEmail: string;
    productId: string;
    prodName: string;
    orderedQuant: Int16Array;
    unitPrice: Float32Array
    totalPrice: Float64Array;
    storeId: string;
    storeName: string;
    storeLoc: string;
}

