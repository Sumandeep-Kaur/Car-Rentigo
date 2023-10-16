import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import * as moment from 'moment';
import { ToastrService } from 'ngx-toastr';
import { Car } from 'src/app/models/Car';
import { Rental } from 'src/app/models/Rental';
import { CarService } from 'src/app/services/car.service';
import { RentalService } from 'src/app/services/rental.service';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})
export class AdminComponent implements OnInit {

  id: number = 0;
  public cars: Car[] = [];
  public rentals: Rental[] = [];
  public returnRequestedRentals: Rental[] = [];
  addMode: boolean = true;
  addForm !: FormGroup;
  editRentalForm !: FormGroup;
  editRentalCarName: string;

  constructor(private carService: CarService, private rentalService: RentalService, 
    private formBuilder: FormBuilder, private toastr: ToastrService) { }

  ngOnInit(): void {

    this.carService.getAllCars().subscribe((data: Car[]) => {
      this.cars = data;
    });

    this.rentalService.getAllRentals().subscribe((data: Rental[]) => {
      this.rentals = data;
    });

    this.rentalService.getReturnRequestedRentals().subscribe((data: Rental[]) => {
      this.returnRequestedRentals = data;
    });
    
    this.addForm = this.formBuilder.group({
      name: ["", [Validators.required]],
      brand: ["", [Validators.required]],
      model: ["", [Validators.required]],
      price: ["", [Validators.required]],
      color: ["", [Validators.required]],
      capacity: ["", [Validators.required]],
      fuelType: ["", [Validators.required]],
      isAvailable: ["", [Validators.required]],
      imageUrl: ["", [Validators.required, Validators.pattern("[^\\s]+(.*?)\\.(jpg|png|JPG|PNG)$")]],
    });

    this.editRentalForm = this.formBuilder.group({
      carId: [''],
      userId: [''],
      rentDurationDays: ['', Validators.required],
      rentalStartDate: ['', Validators.required],
      rentalEndDate: [''],
      totalCost: [''],
    });
  }

  get Name(): FormControl {
    return this.addForm.get('name') as FormControl;
  }

  get Brand(): FormControl {
    return this.addForm.get('brand') as FormControl;
  }

  get Model(): FormControl {
    return this.addForm.get('model') as FormControl;
  }

  get FuelType(): FormControl {
    return this.addForm.get('fuelType') as FormControl;
  }

  get Capacity(): FormControl {
    return this.addForm.get('capacity') as FormControl;
  }

  get Color(): FormControl {
    return this.addForm.get('color') as FormControl;
  }

  get ImageUrl(): FormControl {
    return this.addForm.get('imageUrl') as FormControl;
  }

  get Price(): FormControl {
    return this.addForm.get('price') as FormControl;
  }

  get StartDate(): FormControl {
    return this.editRentalForm.get('rentalStartDate') as FormControl;
  }

  get RentDuration(): FormControl {
    return this.editRentalForm.get('rentDurationDays') as FormControl;
  }

  openEditModal(car: Car) {
    this.id = car.id;
    this.addMode = false;
    this.addForm.patchValue({
      name: car.name,
      brand: car.brand,
      model: car.model,
      color: car.color,
      fuelType: car.fuelType,
      capacity: car.capacity,
      price: car.price,
      imageUrl: car.imageUrl,
      isAvailable: car.isAvailable
     });
  }

  openEditRentalModal(rental: Rental) {
    this.id = rental.id;
    this.editRentalCarName = rental.car.name;
    this.editRentalForm.patchValue({
      carId: rental.carId,
      userId: rental.userId,
      rentDurationDays: rental.rentDurationDays,
      rentalStartDate: rental.rentalStartDate,
      rentalEndDate: rental.rentalEndDate,
      totalCost: rental.car.price
    });
  }

  addCar() {
    if(this.addForm.valid) {
      this.addForm.value.isAvailable = this.addForm.value.isAvailable === 'true' ? true: false;
      this.carService.addCar(this.addForm.value).subscribe({
        next:(response) => {
          if(response.status === "Success") {
            this.addForm.reset();
            window.location.reload();
            this.toastr.success('Car Added Successfully!', 'Success!');
          } else {
            console.log("Failuer");
            this.toastr.error("Some Error Occurred! Cannot Add Car.", "Error!");
          }
        },
        error:() => {
          console.log("Error ocurred");
          this.toastr.error("Some Error Occurred! Cannot Add Car.", "Error!");
        }
      })
    }
  }

  updateCar() {
    if(this.addForm.valid) {
      this.addForm.value.isAvailable = this.addForm.value.isAvailable === 'true' ? true: false;
      this.carService.updateCar(this.id, this.addForm.value).subscribe({
        next:(response) => {
          if(response.status === "Success") {
            this.addForm.reset();
            window.location.reload();
            this.toastr.success("Car Updated Successfully!", "Success!");
          } else {
            console.log("Failuer");
          }
        },
        error:() => {
          console.log("Some Error ocurred");
          this.toastr.error("Some Error Occurred! Cannot Update Car.", "Error!");
        }
      })
    }
  }
  
  deleteCar(id: number) {
    this.carService.deleteCar(id).subscribe({
      next:(response) => {
        if(response.status === "Success") {
          window.location.reload;
          window.location.reload;
          this.toastr.success("Car Deleted Successfully!", "Success!");
        } else {
          console.log("Failure");
          this.toastr.error("Some Error Occurred! Cannot Delete Car.", "Error!");
        }
      },
      error: () => {
        console.log("Some Error Occurred");
        this.toastr.error("Some Error Occurred! Cannot Delete Car.", "Error!");
      }
    });
  }

  addDays(days: number) {
    var startDate = new Date(this.editRentalForm.value.rentalStartDate);
    const date = moment(startDate, 'YYYY-MM-DD');
    const endDate = date.add(days, 'days'); 
    return endDate.format('YYYY-MM-DD');
  }

  updateRental() {
    this.editRentalForm.value.rentalEndDate = this.addDays(this.editRentalForm.value.rentDurationDays);
      this.editRentalForm.value.totalCost *= this.editRentalForm.value.rentDurationDays;
    if (this.editRentalForm.valid) {
      this.rentalService.updateRental(this.id, this.editRentalForm.value).subscribe({
        next:(response) => {
          if(response.status === "Success") {
            this.editRentalForm.reset();
            window.location.reload();
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

  deleteRental(id: number) {
    this.rentalService.deleteRental(id).subscribe({
      next:(response) => {
        if(response.status === "Success") {
          window.location.reload;
          window.location.reload;
          this.toastr.success("Car Deleted Successfully!", "Success!");
        } else {
          console.log("Failure");
          this.toastr.error("Some Error Occurred! Cannot Delete Car.", "Error!");
        }
      },
      error: () => {
        console.log("Some Error Occurred");
        this.toastr.error("Some Error Occurred! Cannot Delete Car.", "Error!");
      }
    });
  }
}
