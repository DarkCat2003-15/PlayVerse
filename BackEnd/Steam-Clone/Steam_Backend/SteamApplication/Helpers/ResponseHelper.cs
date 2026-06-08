using SteamApplication.Models.Response;

namespace SteamApplication.Helpers
{
    public static class ResponseHelper
    {
        public static GenericResponse<T> Create<T>(T data, List<string>? errors = null, int? count = 0, string? message = null)
        {
            var response = new GenericResponse<T>
            {
                Data = data,
                Message = message ?? "Solicitud realizada correctamente",
                Errors = errors ?? [],
                Count = count ?? 0
            };

            return response;
        }





        public static GenericResponse<T> Fail<T>(string message)
        {
            return new GenericResponse<T>
            {
                Message = message,
                Success = false
            };
        }
    }
}
