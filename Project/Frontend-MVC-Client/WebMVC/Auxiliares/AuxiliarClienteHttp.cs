using System.Net.Http.Headers;

namespace WebMVC.Auxiliares
{
    public class AuxiliarClienteHttp
    {

        public static HttpResponseMessage EnviarSolicitud(string url, string verbo, object obj = null, string token = null)
        {
            HttpClient cliente = new HttpClient();
            cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            Task<HttpResponseMessage> tarea = null;

            if (verbo == "GET") {             
                tarea = cliente.GetAsync(url);
            }
            else if (verbo == "POST") {
                tarea = cliente.PostAsJsonAsync(url, obj);
            }
            else if (verbo == "DELETE")
            {
                tarea = cliente.DeleteAsync(url);
            }
            else if (verbo == "PUT")
            {
                cliente.PutAsJsonAsync(url, obj);
            }

            tarea.Wait();
            return tarea.Result;
        }


        public static string ObtenerBody(HttpResponseMessage respuesta)
        {
            var tarea = respuesta.Content.ReadAsStringAsync();
            tarea.Wait();
            return tarea.Result;
        }
    }
}
