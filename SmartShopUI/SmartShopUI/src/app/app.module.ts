import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { HashLocationStrategy, LocationStrategy } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AppRoutingModule } from './app-routing.module';
import { AccountService } from './services/account/account.service';

import { AppComponent } from './app.component';
import { ProductListComponent } from './components/product/product-list/product-list.component';
import { HomeComponent } from './home/home.component';
import { ProductDetailsComponent } from './components/product/product-details/product-details.component';
import { AccountRegisterComponent } from './components/account/account-register/account-register.component';
import { AccountLoginComponent } from './components/account/account-login/account-login.component';
import { AccountDetailsComponent } from './components/account/account-details/account-details.component';
import { CartComponent } from './components/cart/cart.component';
import { OrderCreateComponent } from './components/order/order-create/order-create.component';
import { OrderConfirmationComponent } from './components/order/order-confirmation/order-confirmation.component';
import { OrderDetailsComponent } from './components/order/order-details/order-details.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    ProductListComponent,
    ProductDetailsComponent,
    AccountRegisterComponent,
    AccountLoginComponent,
    AccountDetailsComponent,
    CartComponent,
    OrderCreateComponent,
    OrderConfirmationComponent,
    OrderDetailsComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule
  ],
  providers: [{ provide: LocationStrategy, useClass: HashLocationStrategy }],
  bootstrap: [AppComponent]
})
export class AppModule {
  constructor(public accountService: AccountService) { }
}
