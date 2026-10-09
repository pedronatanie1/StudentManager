# Student Manager

Console-based student management system in C#, built to practise OOP, validation and LINQ.

## Status

Work in progress. Layer 1 (domain models and student management) is partly done.

- [x] Domain models: Student, Course, StudentStatus
- [x] Storage behind an interface (IStudentStore, in-memory store)
- [x] Student service
- [x] Validation
- [ ] Console menu: add, view, update and remove students

## Features

- (none yet, this list grows as each layer is finished)

## Technologies

- C#
- .NET
- LINQ (planned)
- Git

## How to run

    git clone https://github.com/pedronatanie1/StudentManager.git
    cd StudentManager
    dotnet run --project StudentManager

## Planned

- Add, view, update and remove students
- Search, filter and sort
- Statistics
- Modules and grades

## Future improvements

SQL database, ASP.NET Core API, authentication, web interface (not part of this project).