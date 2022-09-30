# NIN.Console

Documentation of parameter
## test



## kartlegging
* Removes existing relations between Grunntype objects and Kartleggingsenhet objects
* ..then uses the import_grunntyper_kartleggingsenheter_v{*}.csv-files for v2.2 and 2.3 to reconnect relations between Grunntype's and Kartleggingenhet's


## importnin



## complete



## unraven
* Command to export ravendata to json-files.
* Yet to be implemented


## import



## export



## migrate
* Extra params: none
* Triggers a dbContext.Database.Migrate()
* EF docs: "Gets all the migrations that are defined in the configured migrations assembly."
<see href="https://aka.ms/efcore-docs-migrations">Database migrations</see>

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


Yet to be clarified:
* Does the api use ravendb or do all versions use sqlserver?
* The purpose of the console app is to move all versions data into a single sqlserver instance?

