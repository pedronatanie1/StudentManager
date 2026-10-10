using System;
using System.Collections.Generic;
using System.Text;

namespace StudentManager.Models
{
    internal static class ModuleCatalogue
    {
        public static IReadOnlyList<Module> Modules { get; } =
        [
            new Module("C101", "Programming", 20),
            new Module("C102", "Databases", 20),
            new Module("C103", "Algorithms", 20),
            new Module("C104", "Web Development", 20)
        ];
    }
}
