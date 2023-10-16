import { Injectable } from '@angular/core';
import { Car } from '../models/Car';
import { Observable, catchError, throwError } from 'rxjs';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class CarService {
  private apiURL = 'https://localhost:44333/api/Car';

  constructor(private httpClient: HttpClient) { }

  getAllCars(): Observable<Car[]> {
    return this.httpClient
      .get<Car[]>(this.apiURL + '/all')
      .pipe(catchError(this.errorHandler));
  }

  getCar(id: number): Observable<Car> {
    return this.httpClient
      .get<Car>(this.apiURL + '/id?id=' + id)
      .pipe(catchError(this.errorHandler));
  }

  searchCars(searchBy: string, searchValue: string): Observable<Car[]> {
    return this.httpClient
      .get<Car[]>(this.apiURL + '/search?searchBy=' + searchBy + '&searchValue=' + searchValue)
      .pipe(catchError(this.errorHandler));
  }  

  addCar(Car: Car): Observable<any> {
    return this.httpClient.post<Car>(this.apiURL + '/add', Car)
      .pipe(catchError(this.errorHandler));
  }

  updateCar(id: number, Car: Car): Observable<any> {
    return this.httpClient.put<Car>(this.apiURL + '/edit/id?id=' + id, Car)
      .pipe(catchError(this.errorHandler));
  }

  deleteCar(id: number): Observable<any> {
    return this.httpClient
      .delete<Car>(this.apiURL + '/delete/id?id=' + id)
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
