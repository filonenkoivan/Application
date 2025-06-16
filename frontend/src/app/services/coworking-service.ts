import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';
import { CoworkingDTO } from '../models/coworking-card-model';

@Injectable({ providedIn: 'root' })
export class CoworkingServices {
  constructor(private http: HttpClient) {}

  GetCoworkings(): Observable<CoworkingDTO[]> {
    return this.http
      .get<{ data: CoworkingDTO[] }>('http://localhost:5086/coworking')
      .pipe(map((response) => response.data));
  }
}
