# NIN.Console

Documentation of parameter
## complete

Create and import sourcedata into new database

Requirements: 
- connectionString in appsettings.json:
```json
{
  "ConnectionStrings": {
    "Default": "data source=localhost;initial catalog=nin;Integrated Security=SSPI;MultipleActiveResultSets=True;App=EntityFramework"
  }
}
```
Ravendb databases must exists from old service on ravendb 3.5 localhost:
- SOSINiNv1
- SOSINiNv2
- SOSINiNv2.0b
- SOSINiNv2.1
- SOSINiNv2

