namespace KeepFit.Backend.API.Models.Routes;

public class ApiRoutes
{
    public const string baseUrl = "/api/v1/";
    public class Exercises
    {
        public const string GetAllExercises = baseUrl + "exercises";
        public const string GetExercises = baseUrl + "exercises/{id:guid}";
        public const string DeleteExercises = baseUrl + "exercises/{id:guid}";
        public const string CreateExercises = baseUrl + "exercises";
    }

    public class Programs
    {
        public const string GetAllPrograms = baseUrl + "programs";
        public const string GetProgram = baseUrl + "programs/{id:guid}";
        public const string DeleteProgram = baseUrl + "programs/{id:guid}";
        public const string CreateProgram = baseUrl + "programs";
    }
}