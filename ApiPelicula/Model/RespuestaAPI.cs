using System.Net;

namespace ApiPelicula.Model
{
    public class RespuestaAPI
    {
        public RespuestaAPI()
        {
            ErrorMessages = new List<string>();
        }
        public HttpStatusCode statusCode { get; set; }
        public bool isSuccess { get; set; } = true;

        public List<string> ErrorMessages { get; set;}

        public object Result { get; set; }
    }
}
