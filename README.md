# Bond Valuation Assignment

Every Monday morning the portfolio management desk receives a fresh file with bond positions—some already held, some under review as potential investments. Before the weekly investment meeting, these positions must be valued consistently so managers can compare them fairly. 

This solution guarantees the accurate valuation of bonds record from a structured and correct CSV file, returning the computed data either in JSON or in CSV format. Furthermore, an exception handling system enables users to track errors in the parsing and calculation processes to their root.


## Features

- CSV Bond Records Parsing
- Bond Present Value Calculation
- Error Tracking to Root Cause
- Output in JSON or CSV Format


## Run Locally

```bash
    # Clone the repository
    git clone https://github.com/nicolasantolini/BondValuationAssignment.git
    cd BondValuationAssignment

    # Restore dependencies
    dotnet restore

    # Build the solution
    dotnet build

    # Run tests
    dotnet test

    # Run the application
    dotnet run --project Application
```

The API will be available at https://localhost:7263 and http://localhost:5166

### Running Docker Locally
```bash
    # Build the Docker image
    docker build -t bondvaluationservice -f Application/Dockerfile .

    # Run the container
    docker run -p 8080:8080 bondvaluationservice

    # Access the API
    curl http://localhost:8080/health
```
