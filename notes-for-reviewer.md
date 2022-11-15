## Design considerations

I focused my design on keeping things simple and lean while decoupling different components in the API.

First, I created a Core library with the domain and some contracts for services to manage this domain.
Then provide a store access layer (EF Core in this case) and the implementations of the services to hook up everything in the API project.

Without more information, I consider this to cover a broad range of scenarios. I try not to over-engineering solutions following the yagni (you-ain't-gonna-need-it) principle but letting me evolve in a future with minimal changes.

With all components decoupled (maybe we could go further in this area) I implemented the changes requested.
Also, some tests were added (and modified) to assure new functionality while maintaining the current.

Regarding the tests I create a new project to test the Business project directly, where the functionality now resides, I usually do this to write short and better readiness tests. This doesn't imply the e2e tests be useless, but I prefer to narrow these to check mappings, input validations, etc.

About the last optional requirement to modify a rental, based on the domain designed, more research is needed to accommodate well the functionality.

## Additional ideas
Finally, I want to mention that, obviously, a lot more of improvements can be done, like:
- Authentication and authorization if needed.
- Standarize responses for errors.
- Caches
- Client autogeneration based on OpenAPI for different languages: .Net, Typescript...