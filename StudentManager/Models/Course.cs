namespace StudentManager.Models
{
    /*
    * Course is represented as a record because it is currently treated
    * as a value-like object, with value-based equality and non-destructive
    * copying using the 'with' expression.
    */
    internal record Course(
        string CourseId,
        string CourseName,
        Department Department,
        int Duration
        );

}
