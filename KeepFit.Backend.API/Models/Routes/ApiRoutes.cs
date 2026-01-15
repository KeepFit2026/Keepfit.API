namespace KeepFit.Backend.API.Models.Routes;

public class ApiRoutes
{
    public const string baseUrl = "/api/v1/"; 

    public class Exercises
    {
        public const string GetProgramsFromExercise = "{id:guid}/programs";
        public const string GetProgramsWitoutNotExercises = "{id:guid}/without-programs";
        public const string AddExerciseToProgram = "{exerciseId:guid}/programs/{programId:guid}";
    }

    public class Programs
    {
        public const string GetExercisesFromProgram = "{programId:guid}/exercises";
    }

    public class Users
    {
        public const string GetAllusers = ""; 
    }
}