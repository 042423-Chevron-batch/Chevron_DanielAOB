import { Component } from '@angular/core';
import { ApiServices } from '../services/api.service';
import { ProdQuantRequest } from '../models/ProdQuantRequest';

@Component({
  selector: 'app-getproducts-store',
  templateUrl: './getproducts-store.component.html',
  styleUrls: ['./getproducts-store.component.css']
})
export class GetproductsStoreComponent {

  constructor(private apiService: ApiServices) { }

  buyProduct: ProdQuantRequest = { SelectLocation: '', SelectedStore: '', SelectProduct: '', OrderQuant: 0 };
  isProductAvailable: boolean = false;

  onProdSelect() {
    this.apiService.ChooseProducts(this.buyProduct).subscribe(
      (response: boolean) => {
        // Handle the response from the API
        this.isProductAvailable = JSON.parse(JSON.stringify(response));
        console.log(this.isProductAvailable);

      },
      (error: any) => {
        // Handle any errors that occurred during the API call
        console.error(error);
      }
    );
  }
}
