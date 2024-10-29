using CourseAPI.Dto;
using CourseAPI.Dtos;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

List<GetCourseDto> courses = [
    new (1, "Note1"),
    new (2, "Note2"),
    new (3, "Note3")

];

app.MapGet("courses", () => courses).WithName("getCourses");

app.MapGet("course/{id}",(int id) => {
    return courses.Find(course => course.Id == id);
}).WithName("getCourseWithId");

app.MapPost("courses", (CreateCourseDto newCourse) => {
    int id = courses.Count + 1;
    GetCourseDto course = new (id, newCourse.Des);
    courses.Add(course);
    return Results.CreatedAtRoute("getCourseWithId", new {id = id}, course);
});

app.MapPut("course/{id}", (int id, CreateCourseDto updatedCourse) => {
    GetCourseDto? currCourse = courses.Find(course=> course.Id == id);
    if (currCourse == null) {
        return Results.NotFound();
    }
    GetCourseDto newCourse = new GetCourseDto(id, updatedCourse.Des);
    courses[id - 1] = newCourse;
    return Results.Ok();
});

app.MapDelete("course/{id}", (int id) => {
    int currId = courses.FindIndex(course => course.Id == id);
    if (currId == -1) {
        return Results.NoContent();
    }
    courses.RemoveAt(currId);
    return Results.Ok();
});

app.Run();
