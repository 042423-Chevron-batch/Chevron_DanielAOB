import { DecimalPipe } from "@angular/common";

export interface Order {

    OrderId: string;
    OrderTime: Date;
    CustomerId: string;
    CustFirstName: string;
    CustUserName: string;
    CustLastName: string
    CustEmail: string;
    ProductId: string;
    Prodname: string;
    OrderedQuant: Int16Array;
    UnitPrice: Float32Array
    TotalPrice: Float64Array;
    StoreId: string;
    StoreName: string;
    StoreLoc: string;
}

