import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Rental } from 'src/app/models/Rental';
import { RentalService } from 'src/app/services/rental.service';

@Component({
  selector: 'app-user-rentals',
  templateUrl: './user-rentals.component.html',
  styleUrls: ['./user-rentals.component.css'],
})
export class UserRentalsComponent implements OnInit {
  public rentals: Rental[] = [];
  id: number = this.actRoute.snapshot.params['id'];

  constructor(
    private rentalService: RentalService,
    private actRoute: ActivatedRoute,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.rentalService.getUserRentals(this.id).subscribe((data: Rental[]) => {
      this.rentals = data;
    });
  }

  getUserId(): number {
    const user = JSON.parse(localStorage.getItem('user')!);
    return user.userId;
  }

  requestForReturn(id) {
    this.rentalService.RequestRentalReturn(id).subscribe({
      next: (response) => {
        if (response.status === 'Success') {
          window.location.reload();
          this.toastr.success('Your Request For Return Is Added Successfully.', 'Success!');
        } else {
          console.log('Failuer');
          this.toastr.error('Some Error Occurred. Please Try Again', 'Error!');
        }
      },
      error: () => {
        console.log('Error ocurred');
        this.toastr.error('Some Error Occurred. Cannot Put Request For Return Now.', 'Error!');
      },
    });
  }
}
