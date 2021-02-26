# NET CORE 5 - JWT TOKEN

#### By providing security with jwt tokens, I enable users to send requests to their service addresses.

AuthServer is the main project and is the token provider. With certain rules of tokens received here, requests can be sent to other mini app api projects. In the main project, there are tokens received with membership and tokens received without membership. I use the Microsoft Identity architecture in the membership system. For user tokens that are not included in the membership system, I provide tokens according to the username and password information I have specified in the appSettings.json file.

![Screenshot_2](https://user-images.githubusercontent.com/54249736/109346531-e3ed6680-7882-11eb-8b68-b162fcaa8acd.png)

There are four libraries and four web API projects in total within the project.

## Libraries
![Screenshot_3](https://user-images.githubusercontent.com/54249736/109346864-565e4680-7883-11eb-8380-e6e9f07bd84f.png)

#### Uploaded Packages (SharedLibrary)
* Microsoft.AspNetCore.Authentication.JwtBearer

#### Uploaded Packages (AuthServer.Service)
* AutoMapper

#### Uploaded Packages (AuthServer.Data)
* Microsoft.EntityFrameworkCore
* Microsoft.EntityFrameworkCore.Design 
* Microsoft.EntityFrameworkCore.SqlServer
* Microsoft.EntityFrameworkCore.Tools

#### Uploaded Packages (AuthServer.Core)
* Microsoft.AspNetCore.Identity.EntityFrameworkCore

#### Uploaded Packages (AuthServer.API) MAIN PROJECT
* FluentValidation.AspNetCore
* Microsoft.AspNetCore.Authentication.JwtBearer
* Microsoft.EntityFrameworkCore.Design
* Swashbuckle.AspNetCore

#### Uploaded Packages (MiniApp1.API, MiniApp2.API, MiniApp3.API)
* Not Found.

#### Contact Addresses
##### Linkedin: [Send a message on Linkedin](https://www.linkedin.com/in/cihatsolak/) 
##### Twitter: [Go to @cihattsolak](https://twitter.com/cihattsolak)
##### Medium: [Read from medium](https://cihatsolak.medium.com/)
