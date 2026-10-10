using StudentManager.Data;
using StudentManager.Services;
using StudentManager.UI;

// Program.cs only wires things together. All the real work lives in other classes
var storage = new InMemoryStudentStorage();
var service = new StudentService(storage);

SeedData.Load(service);

new MainMenu(service).Run();
