# Flight Search Application

This is a full-stack application that allows users to search for flights based on various parameters.  
It is built using **.NET** for the backend, **Angular** for the frontend, and **Redis** for caching.

---

## Table of Contents
1. [Requirements](#requirements)
2. [Setup Instructions](#setup-instructions)
   - Backend Setup (.NET)
   - Frontend Setup (Angular)
   - Run Redis
3. [Usage](#usage)

---

## Requirements

Ensure the following tools are installed on your system:
- **.NET 8 SDK**: [Download .NET 8](https://dotnet.microsoft.com/download)
- **Node.js & npm**: [Download Node.js](https://nodejs.org)
- **Angular CLI**: Install using `npm install -g @angular/cli`
- **Redis**: Install and configure Redis from [Redis Documentation](https://redis.io/)

---

## Setup Instructions

### Backend Setup (.NET)

1. Navigate to the backend directory:
   ```bash
   cd FlightSearch
2. Restore dependencies and run the project:
   ```bash
   dotnet restore
   dotnet run
3. The backend will run on http://localhost:5114 by default.

### Frontend Setup (Angular)
1. Navigate to the frontend directory:
   ```bash
   cd FlightSearchApp
2. Install the dependencies:
   ```bash
   npm install
3. Start the development server:
   ```bash
   ng serve
### Run Redis
1. Start the Redis server using the following command:
   ```bash
   redis-server
2. Redis will run on localhost:6379 by default.

## Usage
Once all services are up and running:
1. Open the frontend at http://localhost:4200.
2. Use the provided search interface to filter and find flights based on next parameters:
 - OriginLocationCode
 - DestinationLocationCode
 - DepartureDate
 - ReturnDate
 - Adults
 - CurrencyCode
3. Results will be displayed in a table format.
