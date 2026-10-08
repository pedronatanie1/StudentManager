using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace StudentManager.Models
{
    internal record Course(
        String CourseId,
        string CourseName,
        Department Deparment,
        int Duration
        );

}
