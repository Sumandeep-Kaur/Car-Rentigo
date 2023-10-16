import { Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { Router } from '@angular/router';
import * as moment from 'moment';
import { ToastrService } from 'ngx-toastr';
import { Car } from 'src/app/models/Car';
import { CarService } from 'src/app/services/car.service';

@Component({
  selector: 'app-cars-grid',
  templateUrl: './cars-grid.component.html',
  styleUrls: ['./cars-grid.component.css'],
})
export class CarsGridComponent implements OnInit {
  public allCars: Car[] = [];
  public searchedCars: Car[] = [];
  public car: string = '';
  bookForm!: FormGroup;
  searchForm!: FormGroup;
  todayDate = new Date();

  constructor(
    private carService: CarService,
    private formBuilder: FormBuilder,
    private router: Router,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.carService.getAllCars().subscribe((data: Car[]) => {
      this.allCars = data;
    });

    this.searchForm = this.formBuilder.group({
      searchBy: ['', Validators.required],
      searchValue: ['', Validators.required]
    });

    this.bookForm = this.formBuilder.group({
      carId: [''],
      userId: [''],
      rentDurationDays: ['', Validators.required],
      rentalStartDate: ['', Validators.required],
      rentalEndDate: [''],
      totalCost: [''],
    });
  }

  get SearchBy(): FormControl {
    return this.searchForm.get('searchBy') as FormControl;
  }

  get SearchValue(): FormControl {
    return this.searchForm.get('searchValue') as FormControl;
  }

  get StartDate(): FormControl {
    return this.bookForm.get('rentalStartDate') as FormControl;
  }

  get RentDuration(): FormControl {
    return this.bookForm.get('rentDurationDays') as FormControl;
  }

  isAuthenticated() {
    const user = localStorage.getItem('user');
    return user ? true : false;
  }

  getUserId(): number {
    const user = JSON.parse(localStorage.getItem('user')!);
    return user.userId;
  }

  sortCars() {
    this.allCars.sort((a, b) => a.price - b.price);
  }

  openBookModal(car: Car) {
    if (!this.isAuthenticated()) {
      alert('Login first to book Car');
      this.router.navigate(['/login']);
    }
    this.car = car.name;
    this.bookForm.patchValue({
      carId: car.id,
      totalCost: car.price,
      userId: this.getUserId(),
    });
  }

  addDays(days: number) {
    var startDate = new Date(this.bookForm.value.rentalStartDate);
    const date = moment(startDate, 'YYYY-MM-DD');
    const endDate = date.add(days, 'days'); 
    return endDate.format('YYYY-MM-DD');
  }


  bookCar() {
    if (this.bookForm.valid) {
      this.bookForm.value.rentalEndDate = this.addDays(this.bookForm.value.rentDurationDays);
      this.bookForm.value.totalCost *= this.bookForm.value.rentDurationDays;
      localStorage.setItem(this.bookForm.value.carId, JSON.stringify(this.bookForm.value));
      this.router.navigate([`/book/${this.bookForm.value.carId}`]);
    }
  }

  changeFilter(e: any) {
    this.SearchBy?.setValue(e.target.value, {
      onlySelf: true,
    });
  }

  searchCar() {
    if (this.searchForm.valid) {
      this.carService.searchCars(this.SearchBy.value, this.SearchValue.value).subscribe((data: Car[]) => {
        this.searchedCars = data;
        if(this.searchedCars.length === 0) {
          this.toastr.info("No Cars Found", "Info");
        }
      });
    }
  }
}
