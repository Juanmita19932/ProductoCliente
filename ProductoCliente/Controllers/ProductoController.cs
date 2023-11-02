using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProductoCliente.Model;
using ProductoCliente.Models;

namespace ProductoCliente.Controllers

   
{
    public class ProductoController : Controller
    {

            private readonly HttpClient _httpClient;
            private readonly UsuarioContext _context;

            public ProductoController(UsuarioContext context)
            {
                _httpClient = new HttpClient();
                _context = context;
            }

            public async Task<IActionResult> IndexProducto()
            {
                string url = "http://localhost:5088/api/productos";
                HttpResponseMessage response = await _httpClient.GetAsync(url);

                List<Producto> productoLista;
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    productoLista = JsonConvert.DeserializeObject<List<Producto>>(json);
                }
                else
                {
                    throw new Exception("Error al encontrar los productos");
                }

                return View(productoLista);
            }

            public IActionResult Create()
            {
                return View();
            }

            [HttpPost]
            public async Task<IActionResult> Create(Producto producto)
            {
                // if (ModelState.IsValid)
                {
                    string url = "http://localhost:5088/api/productos";
                    string json = JsonConvert.SerializeObject(producto);
                    HttpContent content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await _httpClient.PostAsync(url, content);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("IndexProducto");
                    }
                    else
                    {
                        throw new Exception("Error al crear el producto");
                    }
                }

                return View(producto);
            }

            public async Task<IActionResult> Edit(int id)
            {
                string url = $"http://localhost:5088/api/productos/{id}";
                HttpResponseMessage response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    Producto producto = JsonConvert.DeserializeObject<Producto>(json);

                    if (producto != null)
                    {
                        return View(producto);
                    }
                }

                throw new Exception("Error al encontrar el producto");
            }

            [HttpPost]
            public async Task<IActionResult> Edit(int id, Producto producto)
            {

                {
                    string url = $"http://localhost:5088/api/productos/{id}";
                    string json = JsonConvert.SerializeObject(producto);
                    HttpContent content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await _httpClient.PutAsync(url, content);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("IndexProducto");
                    }
                    else
                    {
                        throw new Exception("Error al editar el producto");
                    }
                }

                return View(producto);
            }

            public async Task<IActionResult> Delete(int id)
            {
                string url = $"http://localhost:5088/api/productos/{id}";
                HttpResponseMessage response = await _httpClient.DeleteAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("IndexProducto");
                }
                else
                {
                    throw new Exception("Error al eliminar el producto");
                }
            }
        }
    }