export interface FlightSearchApiResult {
  OriginLocationCode: string;
  DestinationLocationCode: string;
  DepartureDate: Date;
  ReturnDate: Date;
  DepartureTransfers: number;
  ReturnTransfers: number;
  Passengers: number;
  CurrencyCode: string;
  Price: number;
}
