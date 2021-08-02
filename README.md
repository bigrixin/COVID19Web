# COVID19Web

This application is an unofficial website that is used to check confirmed cases of COVID-19 by postcode in New South Wales, Australia. My friend said it is easy to use and fast. Hope you like it. ^_^

The application used technologies are below:

C#，ASP .NET, Web API, Autofac, Unit Test, Azure, Read XML and JSON Data from a URL.
The COVID-19 data are extracted based the open data DATA.NSW
This data is available to the public under the Creative Commons Attribution (CC-BY) licence.
NSW case data are extracted via Webpage from NSW HEALTH
The Postcode to the suburb name function via WebAPI from BM
(Due to the Visit Rate limiting applies, sometimes the suburbs name can not be retrieved.)
:) another better solution is using Google API.

- The system will automatically stop if the app service quota exceeds due to a large number of visits. It maybe would be taking up to 24 hours to reset.
