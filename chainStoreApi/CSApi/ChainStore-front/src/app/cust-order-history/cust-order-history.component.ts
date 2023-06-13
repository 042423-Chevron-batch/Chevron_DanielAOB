import { Component } from '@angular/core';
import { LogIn } from '../models/LogIn';
import { Order } from '../models/Order';
import { ApiServices } from '../services/api.service';

@Component({
  selector: 'app-cust-order-history',
  templateUrl: './cust-order-history.component.html',
  styleUrls: ['./cust-order-history.component.css']
})
export class CustOrderHistoryComponent {



  constructor(private apiService: ApiServices) { }


  AccesOrderHist: LogIn = { UserName: '', Password: '' };


  Orderdetails: Order[] = [];


  ShowOdres() {

    this.apiService.OrderHis(this.AccesOrderHist).subscribe(
      (response: Order[]) => {
        // Handle the response from the API
        this.Orderdetails = JSON.parse(JSON.stringify(response));

        console.log(this.Orderdetails);
      },
      (error: any) => {
        // Handle any errors that occurred during the API call
        console.error(error);
      }
    )
  }

}
