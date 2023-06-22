# PayspaceTaxCalculator
Payspace Tax Calculator

Architecture
------------
1. Used Clean Architecture (previously known as onion/ports and adapters)
2. Documented using icePanel https://s.icepanel.io/nTForOphS8/AR9m
3. Used Visual Studio 2022
4. Used Mediator design pattern, implemented using MediatR library

Running the project
-------------------
1. Clone the project to your local
2. Run update-database in package manager console to run the database migrations
3. Run the integration tests to populate the tables (didn't have time to do seeding)
4. Set the solution to start-up multiple projects (the MVC project and the API)

Known issues
------------
1. No time to do logging, would have liked to log to the database
2. No time to do permissions, would have liked to integrate Microsoft graph api to configure claims for payroll officers and admin
