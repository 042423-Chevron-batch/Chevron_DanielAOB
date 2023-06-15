import { Component } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { LogIn } from '../models/LogIn';
import { ApiServices } from '../services/api.service';
import { LocationRequest } from '../models/LocationRequest';
import { LocationService } from '../shared/location.service';

@Component({
  selector: 'app-store-locations',
  templateUrl: './store-locations.component.html',
  styleUrls: ['./store-locations.component.css']
})
export class StoreLocationsComponent {


  storeLocations: LocationRequest[] = [];


  constructor(private apiService: ApiServices, private locationService: LocationService) { }


  onLocationClick(location: string) {
    this.locationService.setSelectedLocation(location);
  }

  getStoreLocations() {
    // Call the login method of the API service 
    this.apiService.Locations().subscribe(
      (response: any) => {
        this.storeLocations = response;
      },
      (error: any) => {
        // Handle any errors that occurred during the API call
        console.error(error);
      }
    );
  }
}

