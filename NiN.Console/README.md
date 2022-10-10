# NIN.Console

Documentation of parameter
## test



## kartlegging
* Removes existing relations between Grunntype objects and Kartleggingsenhet objects
* ..then uses the import_grunntyper_kartleggingsenheter_v{*}.csv-files for v2.2 and 2.3 to reconnect relations between Grunntype's and Kartleggingenhet's


## importnin



## complete
* Generates structure
* Imports all versions of input-files (raven soon to be only json/csv files)
* Also imports all variaty-levels


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

## Requirements: 
- connectionString in appsettings.json:
```json
{
  "ConnectionStrings": {
    "Default": "data source=localhost;initial catalog=nin;Integrated Security=SSPI;MultipleActiveResultSets=True;App=EntityFramework"
  }
}
```

### Configuration
-user secrets, secrets.json:
These properties are expected (example values added):
'''
{
  "SOSINiNv1Json": "CsvFiles/SOSINiNv1.json",
  "SOSINiNv2Json": "CsvFiles/SOSINiNv2.json",
  "SOSINiNv2.1Json": "CsvFiles/SOSINiNv2.1.json",
  "SOSINiNv2.1BJson": "CsvFiles/SOSINiNv2.1B.json",
  "SOSINiNv2.2Json": "CsvFiles/SOSINiNv2.2.json"
}
'''
Ravendb databases must exists from old service on ravendb 3.5 localhost:
- SOSINiNv1
- SOSINiNv2
- SOSINiNv2.0b
- SOSINiNv2.1


Yet to be clarified:
* Does the api use ravendb or do all versions use sqlserver?
* The purpose of the console app is to move all versions data into a single sqlserver instance?


# Vocabulary

## Taxonomy :
the branch of science concerned with classification, 
especially of organisms; systematics.

## Variety :
a taxonomic category that ranks below subspecies (where present) or species, 
its members differing from others of the same subspecies or 
species in minor but permanent or heritable characteristics. 
Varieties are more often recognized in botany, 
in which they are designated in the style Apium graveolens var. dulce.