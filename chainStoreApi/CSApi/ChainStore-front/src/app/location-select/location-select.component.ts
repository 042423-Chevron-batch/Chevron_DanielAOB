import { Component } from '@angular/core';
import { LocationRequest } from '../models/LocationRequest';
import { ApiServices } from '../services/api.service';
import { StoreList } from '../models/StoreList';

@Component({
  selector: 'app-location-select',
  templateUrl: './location-select.component.html',
  styleUrls: ['./location-select.component.css']
})
export class LocationSelectComponent {

  constructor(private apiService: ApiServices) { }

  selectedLocation: LocationRequest = { SelectLocation: '' };


  Stores: StoreList[] = [];
  location: string = '';

  onLocationSelectClick() {


    // Call the login method of the API service and pass the login object
    this.apiService.SelectLocation(this.selectedLocation).subscribe(
      (response: StoreList[]) => {
        //this.Stores = response;
        this.Stores = JSON.parse(JSON.stringify(response));
        this.location = this.selectedLocation.SelectLocation;
        console.log(this.Stores);

      },
      (error: any) => {
        // Handle any errors that occurred during the API call
        console.error(error);
      }
    );
  }

}
