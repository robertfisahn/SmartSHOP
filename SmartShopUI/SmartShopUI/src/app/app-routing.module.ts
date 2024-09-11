import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProductListComponent } from './components/product/product-list/product-list.component';
import { HomeComponent } from './home/home.component';
import { ProductDetailsComponent } from './components/product/product-details/product-details.component';
import { AccountLoginComponent } from './components/account/account-login/account-login.component';
import { AccountRegisterComponent } from './components/account/account-register/account-register.component';
import { AccountDetailsComponent } from './components/account/account-details/account-details.component';
import { CartComponent } from './components/cart/cart.component';
import { OrderCreateComponent } from './components/order/order-create/order-create.component';
import { OrderConfirmationComponent } from './components/order/order-confirmation/order-confirmation.component';
import { OrderDetailsComponent } from './components/order/order-details/order-details.component';

const routes: Routes = [
  { path: 'order-details/:orderId', component: OrderDetailsComponent },
  { path: 'order-confirmation', component: OrderConfirmationComponent },
  { path: 'order-create', component: OrderCreateComponent },
  { path: 'cart', component: CartComponent },
  { path: 'account-details', component: AccountDetailsComponent },
  { path: 'registration', component: AccountRegisterComponent },
  { path: 'login', component: AccountLoginComponent },
  { path: 'product-details/:id', component: ProductDetailsComponent },
  { path: 'product/all', component: ProductListComponent },
  { path: '', component: HomeComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
