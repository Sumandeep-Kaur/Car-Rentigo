import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, throwError } from 'rxjs';
import { User } from '../models/User';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private apiURL = 'https://localhost:44333/api/User';

  constructor(private httpClient: HttpClient) { }

  register(user: User): Observable<any> {
    return this.httpClient
      .post<any>(this.apiURL + '/register', user)
      .pipe(catchError(this.errorHandler));
  }

  login(user: any): Observable<any> {
    return this.httpClient
      .post<any>(this.apiURL + '/login', user)
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
