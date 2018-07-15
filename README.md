# ApiScanner

### Software requirements for building solution
You need to install the following
- [NodeJS v6+](https://nodejs.org/en/download/)
- [PostgreSQL](https://www.postgresql.org/download/) - more DB Engines in the future
- [VisualStudio 2017 update 15.3+](https://www.visualstudio.com/downloads/)
- [.NET Core 2.0+ SDK](https://dot.net/core)

### Build and run
- Open the solution in VisualStudio
- Set _ApiScanner.Web_ as startup project
- Build it; it will take some time first since it will get all node modules

Before running the project you need to apply database migrations with the following command inside _ApiScanner.DataAccess_

`dotnet ef database update --startup-project ../ApiScanner.Web`

Now you can run the project.

If you receive any rendering error you may need to fully compile webpack with

`webpack --config webpack.config.vendor.js`

You must have webpack installed:

`npm install -g webpack`
