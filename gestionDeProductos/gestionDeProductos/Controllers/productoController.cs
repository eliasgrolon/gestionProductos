using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using gestionDeProductos.Models;

using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Cors;

namespace gestionDeProductos.Controllers
{
    [EnableCors("reglasCors")]
    [Route("api/[controller]")]
    [ApiController]
    public class productoController : ControllerBase
    {
        private readonly string cadenaSQL;
        public productoController(IConfiguration config)
        {
            cadenaSQL = config.GetConnectionString("cadenaSQL");
        }
        [HttpGet]
        [Route("lista")]
        public IActionResult Lista()
        {
            List<producto> lista = new List<producto>();
            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("listaProductos", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (var rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            lista.Add(new producto()
                            {

                                idProducto = Convert.ToInt32(rd["idProducto"]),
                                codigoBarra = rd["codigoBarra"].ToString(),
                                nombre = rd["nombre"].ToString(),
                                marca = rd["marca"].ToString(),
                                categoria = rd["categoria"].ToString(),
                                precio = Convert.ToDecimal(rd["precio"])
                            });
                        }
                    }
                }
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "OK", response = lista });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message, response = lista });
            }
        }

        [HttpGet]
        [Route("obtener/{idProducto:int}")]
        public IActionResult obtener(int idProducto)    
        {
            List<producto> lista = new List<producto>();
            producto producto = new producto();

            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("listaProductos", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (var rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            lista.Add(new producto()
                            {

                                idProducto = Convert.ToInt32(rd["idProducto"]),
                                codigoBarra = rd["codigoBarra"].ToString(),
                                nombre = rd["nombre"].ToString(),
                                marca = rd["marca"].ToString(),
                                categoria = rd["categoria"].ToString(),
                                precio = Convert.ToDecimal(rd["precio"])
                            });
                        }
                    }
                }
                producto = lista.Where(item => item.idProducto == idProducto).FirstOrDefault();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "OK", response = producto});
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message, response = producto});
            }
        }

        [HttpPost]
        [Route("guardar")]
        public IActionResult guardar([FromBody] producto objeto)
        {

            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("guardarProducto", conexion);
                    cmd.Parameters.AddWithValue("codigoBarra", objeto.codigoBarra);
                    cmd.Parameters.AddWithValue("nombre", objeto.nombre);
                    cmd.Parameters.AddWithValue("marca", objeto.marca);
                    cmd.Parameters.AddWithValue("categoria", objeto.categoria);
                    cmd.Parameters.AddWithValue("precio", objeto.precio);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.ExecuteNonQuery();
                }
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "GUARDADO CON EXITO" });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message });
            }
        }

        [HttpPut]
        [Route("editar")]
        public IActionResult editar([FromBody] producto objeto)
        {
            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("editarProducto", conexion);
                    cmd.Parameters.AddWithValue("idProducto", objeto.idProducto == 0 ? DBNull.Value : objeto.idProducto);
                    cmd.Parameters.AddWithValue("codigoBarra", objeto.codigoBarra is null ? DBNull.Value : objeto.codigoBarra);
                    cmd.Parameters.AddWithValue("nombre", objeto.nombre is null ? DBNull.Value : objeto.nombre);
                    cmd.Parameters.AddWithValue("marca", objeto.marca is null ? DBNull.Value : objeto.marca);
                    cmd.Parameters.AddWithValue("categoria", objeto.categoria is null ? DBNull.Value : objeto.categoria);
                    cmd.Parameters.AddWithValue("precio", objeto.precio == 0 ? DBNull.Value : objeto.precio);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.ExecuteNonQuery();
                }
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "EDITADO CON EXITO" });
            }
            catch(Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message });
            }
        }

        [HttpDelete]
        [Route("eliminar/{idProducto:int}")]
        public IActionResult eliminar(int idProducto)
        {
            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("eliminarProducto", conexion);
                    cmd.Parameters.AddWithValue("idProducto",idProducto);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.ExecuteNonQuery();
                }
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ELIMINADO CON EXITO" });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message });
            }
        }
    }
}
