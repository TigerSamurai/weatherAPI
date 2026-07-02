# Weather API (With Redis Caching)

An ASP.NET Core Web API that fetches regional weather data using a **Cache-Aside Architecture**. 

To reduce external network overhead and stay within API rate limits, the system leverages an in-memory Redis database to cache weather summaries. 
If a cache miss occurs, the application falls back to the third-party provider, updates the cache, and returns the data.

---

## Architecture Flow

1. **Cache Hit**: Request arrives ──► Checked in Redis ──► Match found ──► Returns `200 OK` (under 15ms).
2. **Cache Miss**: Request arrives ──► Checked in Redis ──► Null ──► Fetches from Weather Provider ──► Serializes & Saves to Redis ──► Returns `200 OK`.
3. **Fault Tolerance**: Third-party timeouts or network drops are caught defensively, returning clean `404 Not Found` messages instead of internal crashes.

---

## Prerequisites

Before running the application locally, ensure you have the following installed:

* **.NET 8.0 SDK** (or later)
* **Docker Desktop** (for running the Redis container)
* A **Visual Crossing Weather API Key** (Free Tier)

---

## Getting Started Locally

### 1. Clone the Repository


### 2. Start the Local Redis Container

### Run the following Docker command to spin up your local Redis database engine:

docker run --name local-redis -p 6379:6379 -d redis

### 3. Configure Your API Key

### Open appsettings.json (or use secrets manager) and add your Visual Crossing secret token:

{
  "WeatherApi": {
    "ApiKey": "YOUR_ACTUAL_API_KEY"
  }
}

### 4. Build and Run the App

dotnet build
dotnet run --project WeatherAPI

### The server will boot up and start listening on http://localhost:5167.

### Sample request in WeatherAPI.http

GET http://localhost:5167/api/weather/berlin
Accept: application/json

### for the first run it will have a cache miss and go fetch the data from the server. On the second run it would instantly show the information.
