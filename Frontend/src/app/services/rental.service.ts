import { Injectable } from '@angular/core';
import { Observable, catchError, throwError } from 'rxjs';
import { Rental } from '../models/Rental';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class RentalService {
  private apiURL = 'https://localhost:44333/api/Rental';

  constructor(private httpClient: HttpClient) { }

  getAllRentals(): Observable<Rental[]> {
    return this.httpClient
      .get<Rental[]>(this.apiURL + '/all')
      .pipe(catchError(this.errorHandler));
  }

  getUserRentals(userId: number): Observable<Rental[]> {
    return this.httpClient
      .get<Rental[]>(this.apiURL + '/userId?userId=' + userId)
      .pipe(catchError(this.errorHandler));
  }

  getReturnRequestedRentals(): Observable<Rental[]> {
    return this.httpClient
      .get<Rental[]>(this.apiURL + '/returns')
      .pipe(catchError(this.errorHandler));
  }

  addRental(rental: Rental): Observable<any> {
    return this.httpClient.post<Rental>(this.apiURL + '/add', rental)
      .pipe(catchError(this.errorHandler));
  }

  updateRental(id: number, rental: Rental): Observable<any> {
    return this.httpClient.put<Rental>(this.apiURL + '/edit/id?id=' + id, rental)
      .pipe(catchError(this.errorHandler));
  }

  deleteRental(id: number): Observable<any> {
    return this.httpClient
      .delete<Rental>(this.apiURL + '/delete/id?id=' + id)
      .pipe(catchError(this.errorHandler));
  }

  RequestRentalReturn(id: number): Observable<any> {
    return this.httpClient.put<Rental>(this.apiURL + '/return/id?id=' + id, null)
      .pipe(catchError(this.errorHandler));
  }
  
  errorHandler(error: {
    error: { message: string };
    status: any;
    message: any;
  }) {
    console.log(error);
    return throwError(
      () => new Error('Something bad happened; please try again later.')
    );
  }
}
