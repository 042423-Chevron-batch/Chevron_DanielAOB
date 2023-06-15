import { Injectable } from '@angular/core';
import { LocationRequest } from '../models/LocationRequest';

@Injectable({
  providedIn: 'root'
})
export class LocationService {
  selectedLocation: LocationRequest = { SelectLocation: '' };

  constructor() { }

  setSelectedLocation(location: string) {
    this.selectedLocation.SelectLocation = location;
  }

  getSelectedLocation(): LocationRequest {
    return this.selectedLocation;
  }
}
