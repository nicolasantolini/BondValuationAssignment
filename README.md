# Bond Valuation Assignment

This service accepts structured CSV bond position files, validates them, and computes present values and other valuation metrics. It returns results in JSON or CSV and exposes a small HTTP API for batch valuation and health checks. It is intended for portfolio managers and engineers who need deterministic, auditable bond valuations from CSV inputs.

### Features
- Parse structured CSV bond records
- Calculate bond present value (coupon and zero-coupon)
- Return output in JSON or CSV
- Track errors with root-cause details
- Docker image for easy deployment

## Prerequisites
- .NET SDK 8.0
- Docker (optional, for container runs)

## Quick start (local)
1. Clone and build
```bash
git clone https://github.com/nicolasantolini/BondValuationAssignment.git
cd BondValuationAssignment
dotnet restore
dotnet build
```

2. Run the application
```bash
dotnet run --project Application
```
Default local URLs:
- HTTPS: https://localhost:7263
- HTTP:  http://localhost:5166

## Health check
```bash
curl http://localhost:8080/health on Docker # or http://localhost:5166/health when running locally
```

## Docker
Build and run the container:
```bash
docker build -t bondvaluationservice -f Application/Dockerfile .
docker run -p 8080:8080 bondvaluationservice
curl http://localhost:8080/health
```


### API (all endpoints)
- GET /health
  - 200 OK
- POST /
  - Accepts: multipart/form-data file upload (CSV)
  - Returns: application/json with computed results and detailed errors, or text/csv without errors
- GET /swagger
  - OpenAPI / Swagger UI (when enabled) for interactive docs
 
## API Testing
**Live Endpoint:**  
`POST https://bond-valuation-api.orangedune-ba3ed3c8.northeurope.azurecontainerapps.io/api/BondValuation/`

### Sample CSV input
Provide a header row with these columns (example):
<img width="2368" height="88" alt="image" src="https://github.com/user-attachments/assets/8d14d987-9ebe-4dfe-abbd-b95543d7be14" />


### Sample JSON output
```json
{
  "results": [
    {
      "bondId": "B002",
      "type": "Bond",
      "presentValue": 458.45,
      "issuer": "National Utilities",
      "rating": "A",
      "yearsToMaturity": 5.05,
      "deskNotes": "Corporate bond"
    }
  ],
  "errors": [
    "Error parsing row 2 for Bond 'B001': Unknown bond type 'Inflation-Linked'. (Parameter 'Type')"
  ]
}
```

### Quick Test
You can use the **bond_positions_sample.csv** from this repo for the body.

```bash
# JSON response
curl -X POST "https://bond-valuation-api.orangedune-ba3ed3c8.northeurope.azurecontainerapps.io/api/BondValuation/" \
  -H "Content-Type: application/json" \
  --data-binary @bond_positions_sample.csv

# CSV response  
curl -X POST "https://bond-valuation-api.orangedune-ba3ed3c8.northeurope.azurecontainerapps.io/api/BondValuation/" \
  -H "Content-Type: text/csv" \
  --data-binary @bond_positions_sample.csv
```

## Configuration
- Ports: configured in appsettings.json or environment variables (e.g., ASPNETCORE_URLS)

## Error handling
- Validation errors are returned per-record with root-cause diagnostics (column, value, rule).
- HTTP error codes:
  - 400 Bad Request: malformed CSV
  - 500 Internal Server Error: unexpected errors (server-side)

## Testing
Run unit and integration tests:
```bash
dotnet test
```



