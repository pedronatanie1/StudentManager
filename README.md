# Student Manager

Console-based student management system in C#, built to practise OOP, validation and LINQ.

## Status

Feature complete for the project specification. Data is held in memory, so it resets each time the program starts.

## Features

- Add, view, update and remove students
- View a single student's details
- Validation of names, email, date of birth, year of study and marks
- Search by student ID, name or email (case-insensitive)
- Filter by course, year of study and status
- Sort by first name, last name, ID, date of birth or year of study (ascending or descending)
- Modules and grades: record a mark for a student in a module
- Academic performance per student: average, highest and lowest mark, modules passed and failed
- Statistics by status and by course
- Invalid input never crashes the program

## Technologies

- C#
- .NET
- LINQ
- Git

## How to run

    git clone https://github.com/pedronatanie1/StudentManager.git
    cd StudentManager
    dotnet run --project StudentManager

The program starts with a few sample students, so you can try every option straight away.

## Design notes

- **Storage behind an interface.** `StudentService` depends on `IStudentStorage`, not on a specific class. Storage is currently in memory (`InMemoryStudentStorage`), and it could be swapped for JSON or a database without changing the rest of the code.
- **A dictionary keyed by student ID.** IDs must be unique and lookup by ID is the most common operation, so `TryAdd` rejects duplicates and finds students without scanning a list.
- **Validation in two layers.** `StudentValidator` checks user input and returns a friendly message, because bad input is normal. The `Student` constructor still throws for invalid values as a safety net, because reaching it with bad data would be a bug.
- **Encapsulation.** Student properties have private setters. A student can only change through methods like `ChangeStatus`, `ChangeYear` and `ChangeCourse`, which check the new values.
- **Marks belong to a student and a module together,** so each result is a `ModuleResult` object holding both, instead of fields like `Grade1`, `Grade2`.
- **The UI is separate.** Only `MainMenu` and `ConsoleInput` use the console. The service and models never print anything.

## Example usage

    ================================
           STUDENT MANAGER
    ================================
     1. Add student
     2. View students
     ...
    Select an option: 10

    ===== Student Statistics =====
    Total students: 8

    Active          5
    Suspended       1
    Graduated       1
    Withdrawn       1

## Future improvements

SQL database, ASP.NET Core API, authentication, web interface (not part of this project).
