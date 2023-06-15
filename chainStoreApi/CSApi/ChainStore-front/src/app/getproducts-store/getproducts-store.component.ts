import { Component } from '@angular/core';
import { ApiServices } from '../services/api.service';
import { ProdQuantRequest } from '../models/ProdQuantRequest';
import { ProductsPurchase } from '../models/ProductsPurchase';
import { NumberSymbol } from '@angular/common';

@Component({
  selector: 'app-getproducts-store',
  templateUrl: './getproducts-store.component.html',
  styleUrls: ['./getproducts-store.component.css']
})
export class GetproductsStoreComponent {

  constructor(private apiService: ApiServices) { }

  buyProduct: ProdQuantRequest = { selectProduct: '', orderQuant: 0 };

  productsPurchased: ProductsPurchase = {} as ProductsPurchase;


  onProdSelect() {
    this.apiService.ChooseProducts(this.buyProduct).subscribe(
      (response: ProductsPurchase) => {
        // Handle the response from the API
        this.productsPurchased = JSON.parse(JSON.stringify(response));

        console.log(this.productsPurchased);
      },
      (error: any) => {
        // Handle any errors that occurred during the API call
        console.error(error);
      }
    );
  }
}
