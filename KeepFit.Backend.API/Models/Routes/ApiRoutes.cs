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
        public const string GetProgramsFromExercise = baseUrl + "exercises/{id:guid}/programs";
    }

    public class Programs
    {
        public const string GetAllPrograms = baseUrl + "programs";
        public const string GetProgram = baseUrl + "programs/{id:guid}";
        public const string DeleteProgram = baseUrl + "programs/{id:guid}";
        public const string CreateProgram = baseUrl + "programs";
        public const string AddExerciseToProgram = baseUrl + "programs/{programId:guid}/exercises/{exerciseId:guid}";
    }
}