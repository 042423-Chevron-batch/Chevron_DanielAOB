import { NgModule, inject } from '@angular/core';
import { RouterModule, Router, Routes, CanActivateFn, RouterStateSnapshot, ActivatedRouteSnapshot } from '@angular/router';
import { LogInUserComponent } from './log-in-user/log-in-user.component';
import { RegisterUserComponent } from './register-user/register-user.component';
import { StoreLocationsComponent } from './store-locations/store-locations.component';
import { LocationSelectComponent } from './location-select/location-select.component';
import { ProductSelectComponent } from './product-select/product-select.component';
import { GetproductsStoreComponent } from './getproducts-store/getproducts-store.component';
import { CustOrderHistoryComponent } from './cust-order-history/cust-order-history.component';
import { AuthService } from './services/auth.service';



const canActivateMain: CanActivateFn =
  (route: ActivatedRouteSnapshot, state: RouterStateSnapshot) => {
    const router = inject(Router);
    if (inject(AuthService).isAuthenticated()) {
      return true;
    } else {
      router.navigate(['log-in-user']);
      return false;
    }
  };


const routes: Routes = [
  { path: 'log-in-user', component: LogInUserComponent },
  { path: 'register-user', component: RegisterUserComponent },
  { path: 'store-locations', component: StoreLocationsComponent, canActivate: [canActivateMain] },
  { path: 'location-select', component: LocationSelectComponent },
  { path: 'product-select', component: ProductSelectComponent },
  { path: 'getproducts-store', component: GetproductsStoreComponent },
  { path: 'cust-order-history', component: CustOrderHistoryComponent },
  // Other routes...

  // Add a default route to redirect to a specific component if no route matches
  { path: '', redirectTo: '/log-in-user', pathMatch: 'full' },
];



@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }







