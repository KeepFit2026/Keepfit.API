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
        public const string GetProgramsWitoutNotExercises = baseUrl + "exercises/{id:guid}/without-programs";
        public const string AddExerciseToProgram = baseUrl + "exercises/{exerciseId:guid}/programs/{programId:guid}";
        
    }

    public class Programs
    {
        public const string GetAllPrograms = baseUrl + "programs";
        public const string GetProgram = baseUrl + "programs/{id:guid}";
        public const string DeleteProgram = baseUrl + "programs/{id:guid}";
        public const string CreateProgram = baseUrl + "programs";
        public const string GetExercisesFromProgram = baseUrl + "programs/{programId:guid}/exercises";
    }
}