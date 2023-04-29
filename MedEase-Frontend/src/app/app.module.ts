import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { NavbarComponent } from './components/layout/navbar/navbar.component';
import { FooterComponent } from './components/layout/footer/footer.component';
import { HomeComponent } from './components/home/home/home.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CarouselComponent } from './components/home/carousel/carousel.component';
import { CarouselModule } from 'ngx-owl-carousel-o';
import { ProfileComponent } from './components/Patient/profile/profile.component';
import { AppointmentsComponent } from './components/Patient/appointments/appointments.component';
import { InsuranceComponent } from './components/Patient/insurance/insurance.component';
import { DoctorDetailsComponent } from './components/Doctor/doctor-details/doctor-details.component';
import { AppointementsComponent } from './components/Doctor/appointements/appointements.component';


@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    FooterComponent,
    HomeComponent,
    CarouselComponent,
    ProfileComponent,
    AppointmentsComponent,
    InsuranceComponent,
    DoctorDetailsComponent,
    AppointementsComponent,
   

  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    CarouselModule,
    BrowserAnimationsModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
  ],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule {}
