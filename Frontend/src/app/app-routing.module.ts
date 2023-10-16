import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './components/home/home.component';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { AdminComponent } from './components/admin/admin.component';
import { BookCarComponent } from './components/book-car/book-car.component';
import { UserRentalsComponent } from './components/user-rentals/user-rentals.component';
import { authGuard } from './guards/auth.guard';

const routes: Routes = [
  {path: '', redirectTo: "/home", pathMatch:"full"},
  {path: 'home', component: HomeComponent},
  {path: 'login', component: LoginComponent},
  {path: 'register', component: RegisterComponent},
  {path: 'book/:id', component: BookCarComponent, canActivate: [authGuard]},
  {path: 'my-rentals/:id', component: UserRentalsComponent, canActivate: [authGuard]},
  {path: 'admin', component: AdminComponent, canActivate: [authGuard]},
  {path: '**', component: HomeComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
