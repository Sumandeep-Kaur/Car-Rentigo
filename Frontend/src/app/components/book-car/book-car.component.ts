import { AfterViewInit, Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import * as moment from 'moment';
import { ToastrService } from 'ngx-toastr';
import { Car } from 'src/app/models/Car';
import { Rental } from 'src/app/models/Rental';
import { CarService } from 'src/app/services/car.service';
import { RentalService } from 'src/app/services/rental.service';

@Component({
  selector: 'app-book-car',
  templateUrl: './book-car.component.html',
  styleUrls: ['./book-car.component.css']
})
export class BookCarComponent implements OnInit, AfterViewInit {
  car: Car;
  bookForm!: FormGroup;
  todayDate = new Date();
  id: number = this.actRoute.snapshot.params['id'];
  rental: Rental  = JSON.parse(localStorage.getItem(this.id.toString()));

  constructor(public actRoute: ActivatedRoute, private carService: CarService, private formBuilder: FormBuilder,
    private router: Router, private rentalService: RentalService, private toastr: ToastrService) { }

  ngOnInit(): void {
    this.carService.getCar(this.id).subscribe((data: Car) => {
      this.car = data;
    });

    this.bookForm = this.formBuilder.group({
      carId: [this.rental.carId],
      userId: [this.rental.userId],
      rentDurationDays: [this.rental.rentDurationDays, Validators.required],
      rentalStartDate: [this.rental.rentalStartDate, Validators.required],
      rentalEndDate: [this.rental.rentalEndDate],
      totalCost: [this.rental.totalCost],
    });
  }

  ngAfterViewInit(): void {
    var urlParams = [];
    window.location.search.replace("?", "").split("&").forEach(function (e, i) {
        var p = e.split("=");
        urlParams[p[0]] = p[1];
    });

    // We have all the params now -> you can access it by name
    console.log(urlParams["loaded"]);

    if(urlParams["loaded"]) {}else{

        let win = (window as any);
        win.location.search = '?loaded=1';
        //win.location.reload('?loaded=1');
    }
  }

  get StartDate(): FormControl {
    return this.bookForm.get('rentalStartDate') as FormControl;
  }

  get RentDuration(): FormControl {
    return this.bookForm.get('rentDurationDays') as FormControl;
  }

  getUserId(): number {
    const user = JSON.parse(localStorage.getItem('user')!);
    return user.userId;
  } 

  openBookModal() {
    this.bookForm.patchValue({
      carId: this.rental.carId,
      userId: this.rental.userId,
      rentDurationDays: this.rental.rentDurationDays,
      rentalStartDate: this.rental.rentalStartDate,
      rentalEndDate: this.rental.rentalEndDate,
      totalCost: this.rental.totalCost
    });
  }

  addDays(days: number) {
    var startDate = new Date(this.bookForm.value.rentalStartDate);
    const date = moment(startDate, 'YYYY-MM-DD');
    const endDate = date.add(days, 'days'); 
    return endDate.format('YYYY-MM-DD');
  }

  updateBook() {
    this.bookForm.value.rentalEndDate = this.addDays(this.bookForm.value.rentDurationDays);
    this.bookForm.value.totalCost = this.car.price * this.bookForm.value.rentDurationDays;

    this.rental.rentalStartDate = this.bookForm.value.rentalStartDate;
    this.rental.rentDurationDays = this.bookForm.value.rentDurationDays;
    this.rental.rentalEndDate = this.bookForm.value.rentalEndDate;
    this.rental.totalCost = this.bookForm.value.totalCost;
  }


  bookCar() {
    console.log(this.bookForm.value);
    if (this.bookForm.valid) {
      this.rentalService.addRental(this.rental).subscribe({
        next:(response) => {
          if(response.status === "Success") {
            this.router.navigate([`/my-rentals/${this.getUserId()}`]);
            this.toastr.success("Car Booked Successfully.", "Success!");
          } else {
            console.log("Failuer");
            this.toastr.error("Some Error Occurred. Please Try Again", "Error!");
          }
        },
        error:() => {
          console.log("Error ocurred");
          this.toastr.error("Some Error Occurred. Cannot Book Car Now.", "Error!");
        }
      })
    }
  }

  cancelBook() {
    localStorage.removeItem(this.id.toString());
    this.router.navigate(['/home']);
  }
}
