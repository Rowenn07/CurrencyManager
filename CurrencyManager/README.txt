# Currency Manager

Currency Manager is a service for handling currency conversions and maintaining a history of conversions.

## Table of Contents

- [Installation](#installation)
Install necessary packages:
Microsoft.Extensions.Caching.StackExchangeRedis
StackExchange.Redis
Microsoft.EntityFrameworkCore
Pomelo.EntityFrameworkCore.MySql
Newtonsoft.Json

- [Usage](#usage)
## Usage
1. Update the `appsettings.json` file with your database connection string and API keys.
2. Run the application

- [API](#api)
1. GET /api/currency/convert?fromCurrency=USD&toCurrency=EUR&amount=100
returns the converted amount

2. Get currency conversion history:
GET /api/currency/history
returns the currency conversion history

3. CRUD operations for currency conversion history

4. Schema for currency conversion history:

Note:
Project unfinished.
Redis implementation with database commented out until fixed.

To-do
Complete Redis and MySQL implementation with dev testing.
Code cleanup