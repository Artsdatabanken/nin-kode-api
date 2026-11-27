# 04-swashbuckle: Upgrade Swagger tooling

Upgrade Swashbuckle.AspNetCore from 8.1.1 to 11.0.1 centrally. Review `apidiff/Swashbuckle.AspNetCore.apidiff.md`, handle transitive OpenAPI API changes and verify generated versioned Swagger documents.

## Research
- `NinKode.WebApi` consumes the centrally managed package. The umbrella API diff has no reported removals but omits transitive Microsoft.OpenApi changes. The upstream v10 migration guide explicitly identifies OpenAPI v2 breaking namespaces and custom filter APIs; OpenAPI 3.0 remains the default output.
- `Startup.cs`, `Helpers/Swagger/SwaggerOptions.cs`, `SwaggerDefaultValues.cs`, and three `Filters/Enum*` files use the affected OpenAPI model types. EnumSchemaFilter is registered at startup; other filters still need to compile.
- Baseline documents for seven API groups were captured from the running pre-migration application in `%TEMP%`. Compare operation paths, schema and responses after migration, including the legacy v2.1b group. The API versioning migration was reverted, so this task must work with existing versioning 5.1.0 or record a blocker.
- The first build attempt showed OpenAPI 3.11 removes the old `Microsoft.OpenApi.Models` and `.Any` namespaces and makes schema filter inputs `IOpenApiSchema`. The OpenAPI migration guide replaces `OpenApiAny` values with `JsonNode`; concrete mutable schemas must be handled explicitly. Retry by adapting filters and compare generated documents instead of stopping at the compiler errors.

**Done when**: The web application builds and Swagger documents preserve routes, schemas and response metadata, or a concrete blocker is documented.
