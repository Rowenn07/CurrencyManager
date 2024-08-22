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
MySql.Data
Moq
xunit.extensibility.core
xunit.assert

- [Usage](#usage)
## Usage
1. Ensure MySQL db is setup and running.
1. Update the `appsettings.json` file with your database connection string
2. Ensure REDIS is setup and started.
2. Run the application

- [API](#api)
1. GET /api/currency/convert?fromCurrency=USD&toCurrency=EUR&amount=100
returns the converted amount

2. Get currency conversion history:
GET /api/currency/history
returns the currency conversion history

3. CRUD operations for currency conversion history

4. Schema for currency conversion history

Note:
In progress.
Final tests on database.
Code cleanup.