import { Component } from '@angular/core';
import { MatTableModule } from '@angular/material/table';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import {
  AbstractControl,
  FormBuilder,
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  ValidationErrors,
  ValidatorFn,
  Validators,
} from '@angular/forms';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { MatError, MatOption, MatSelect } from '@angular/material/select';
import { CommonModule } from '@angular/common';
import { provideNativeDateAdapter } from '@angular/material/core';
import { FlightSearchApiResult } from '../FlightSearchApiResult';

@Component({
  selector: 'app-flight-search',
  standalone: true,
  providers: [provideNativeDateAdapter()],
  imports: [
    MatTableModule,
    MatInputModule,
    MatButtonModule,
    MatSelect,
    MatOption,
    MatError,
    MatProgressSpinnerModule,
    MatDatepickerModule,
    ReactiveFormsModule,
    CommonModule,
    HttpClientModule,
  ],
  templateUrl: './flight-search.component.html',
  styleUrl: './flight-search.component.css',
})
export class FlightSearchComponent {
  searchForm: FormGroup;
  searchResults: FlightSearchApiResult[] = [];
  displayedColumns: string[] = [];
  loading = false;

  columnHeaderMap: { [key: string]: string } = {
    originLocationCode: 'Origin Location Code',
    destinationLocationCode: 'Destination Location Code',
    departureDate: 'Departure Date',
    departureTransfers: 'Departure Transfers',
    returnDate: 'Return Date',
    returnTransfers: 'Return Transfers',
    passengers: 'Passengers',
    currencyCode: 'Currency Code',
    price: 'Price',
  };

  constructor(private fb: FormBuilder, private http: HttpClient) {
    this.searchForm = this.fb.group({
      originLocationCode: new FormControl('', [
        Validators.required,
        Validators.minLength(3),
        Validators.maxLength(3),
      ]),
      destinationLocationCode: new FormControl('', [
        Validators.required,
        Validators.minLength(3),
        Validators.maxLength(3),
      ]),
      departureDate: new FormControl('', [
        Validators.required,
        this.futureDateValidator(),
      ]),
      returnDate: new FormControl('', [
        this.returnDateAfterDepartureValidator(),
      ]),
      adults: new FormControl('1'),
      currency: ['EUR'],
    });
  }

  futureDateValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const inputDate = new Date(control.value);
      const now = new Date();

      now.setHours(0, 0, 0, 0);

      return inputDate > now ? null : { futureDate: true };
    };
  }

  returnDateAfterDepartureValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const departureDate = control.parent?.get('departureDate')?.value;
      const returnDate = control.value;

      if (!departureDate || !returnDate) {
        return null;
      }

      const departure = new Date(departureDate);
      const returnDt = new Date(returnDate);

      return returnDt < departure ? { returnDateInvalid: true } : null;
    };
  }

  getHeaderLabel(column: string): string {
    return this.columnHeaderMap[column] || column;
  }

  onSearch() {
    this.loading = true;
    const formData = this.searchForm.value;

    this.http
      .post<any[]>('http://localhost:5114/api/FlightsSearch', {
        originLocationCode: formData.originLocationCode,
        destinationLocationCode: formData.destinationLocationCode,
        departureDate: formData.departureDate,
        returnDate: formData.returnDate ? formData.returnDate : null,
        adults: formData.adults,
        currencyCode: formData.currency,
      })
      .subscribe({
        next: (results) => {
          this.searchResults = results;
          this.loading = false;
          if (!formData.returnDate) {
            this.displayedColumns = [
              'originLocationCode',
              'destinationLocationCode',
              'departureDate',
              'departureTransfers',
              'passengers',
              'price',
              'currencyCode',
            ];
          } else {
            this.displayedColumns = [
              'originLocationCode',
              'destinationLocationCode',
              'departureDate',
              'departureTransfers',
              'returnDate',
              'returnTransfers',
              'passengers',
              'price',
              'currencyCode',
            ];
          }
        },
        error: (err) => {
          console.error(err);
          this.loading = false;
        },
      });
  }
}
