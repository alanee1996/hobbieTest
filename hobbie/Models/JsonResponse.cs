using System;
namespace hobbie.Models
{
    public class JsonResponse
    {
        public string message { get; set; }
        /// <summary>
        /// usually 0 = failed , 1 = success
        /// </summary>
        public int code { get; set; }
        public object data { get; set; }

        public static JsonResponse success(object data = null, string message = "Request success", int code = 1)
        {
            return new JsonResponse
            {
                message = message,
                data = data,
                code = code
            };
        }

        public static JsonResponse failed(object data = null, string message = "Request failed", int code = 0)
        {
            return new JsonResponse
            {
                message = message,
                data = data,
                code = code
            };
        }
    }
}
