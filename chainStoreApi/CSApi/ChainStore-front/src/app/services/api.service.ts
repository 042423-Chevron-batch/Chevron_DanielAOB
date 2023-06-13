import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ChainStore } from '../environment';
import { LogIn } from '../models/LogIn';
import { Register } from '../models/Register';
import { Person } from '../models/Person';
import { LocationRequest } from '../models/LocationRequest';
import { LocationStoreRequest } from '../models/LocationStoreRequest';
import { ProdQuantRequest } from '../models/ProdQuantRequest';



@Injectable({
  providedIn: 'root'
})
export class ApiServices {
  constructor(private http: HttpClient) { }




  registration(Registration: Register): Observable<any> {
    const headers = { 'content-type': 'application/json' }
    const body = JSON.stringify(Registration);
    console.log(body)
    return this.http.post(ChainStore.Register, Registration);

  }


  login(Login: LogIn): Observable<any> {
    const headers = { 'content-type': 'application/json' }
    const body = JSON.stringify(Login);
    console.log(body)
    return this.http.post(ChainStore.logIn, Login);
  }

  //get list of locations
  Locations(): Observable<any> {
    const headers = { 'content-type': 'application/json' }
    console.log('getPeople ' + ChainStore.StoreLocations)
    return this.http.get<LocationRequest[]>(ChainStore.StoreLocations);

  }
  //get list of stores based on chosen location
  SelectLocation(ChooseLocation: LocationRequest): Observable<any> {

    const headers = { 'content-type': 'application/json' }
    const body = JSON.stringify(ChooseLocation);
    console.log(body)
    return this.http.post(ChainStore.ChooseLocation, ChooseLocation);
  }



  //get products in store

  ProductsInStore(getProducts: LocationStoreRequest): Observable<any> {

    const headers = { 'content-type': 'application/json' }
    const body = JSON.stringify(getProducts);
    //console.log(body)
    return this.http.post(ChainStore.AvailableProducts, getProducts);
  }

  //buy product 
  ChooseProducts(SelectProdquant: ProdQuantRequest): Observable<any> {

    const headers = { 'content-type': 'application/json' }
    const body = JSON.stringify(SelectProdquant);
    console.log(body)
    return this.http.post(ChainStore.ChooseProducts, SelectProdquant);
  }

  OrderHis(Orderhist: LogIn): Observable<any> {

    const headers = { 'content-type': 'application/json' }
    const body = JSON.stringify(Orderhist);
    console.log(body)
    return this.http.post(ChainStore.CustOrderHist, Orderhist);
  }
}







