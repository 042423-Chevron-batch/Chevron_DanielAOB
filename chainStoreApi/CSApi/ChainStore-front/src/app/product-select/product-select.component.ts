import { Component } from '@angular/core';
import { ApiServices } from '../api.service';
import { LocationStoreRequest } from '../models/LocationStoreRequest';
import { ProductDetails } from '../models/Productdetails';

@Component({
  selector: 'app-product-select',
  templateUrl: './product-select.component.html',
  styleUrls: ['./product-select.component.css']
})
export class ProductSelectComponent {


  constructor(private apiService: ApiServices) { }


  availableProducts: LocationStoreRequest = { SelectLocation: '', SelectStore: '' };

  productsInStore: ProductDetails[] = [];

  availProducts() {

    this.apiService.ProductsInStore(this.availableProducts).subscribe(
      (response: ProductDetails[]) => {
        this.productsInStore = JSON.parse(JSON.stringify(response));
        console.log(this.productsInStore);
      },
      (error: any) => {
        // Handle any errors that occurred during the API call
        console.error(error);
      }
    )
  }

}
