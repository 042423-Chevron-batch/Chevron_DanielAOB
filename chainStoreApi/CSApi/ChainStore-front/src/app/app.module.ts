import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';


import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { RegisterUserComponent } from './register-user/register-user.component';
import { LogInUserComponent } from './log-in-user/log-in-user.component';
import { StoreLocationsComponent } from './store-locations/store-locations.component';


import { LocationSelectComponent } from './location-select/location-select.component';
import { GetproductsStoreComponent } from './getproducts-store/getproducts-store.component';
import { ProductSelectComponent } from './product-select/product-select.component';
import { CustOrderHistoryComponent } from './cust-order-history/cust-order-history.component';
import { CustOrderStatisticsComponent } from './cust-order-statistics/cust-order-statistics.component';
import { ToolbarComponent } from './toolbar/toolbar.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';


@NgModule({
  declarations: [
    AppComponent,
    RegisterUserComponent,
    LogInUserComponent,
    StoreLocationsComponent,
    LocationSelectComponent,
    GetproductsStoreComponent,
    ProductSelectComponent,
    CustOrderHistoryComponent,
    CustOrderStatisticsComponent,
    ToolbarComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    RouterModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }



