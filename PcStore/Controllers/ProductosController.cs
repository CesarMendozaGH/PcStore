using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PcStore.Models; // Asegúrate de que este namespace coincida con el de tus modelos generados

namespace PcStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly PcStoreContext _context;

        // Constructor del controlador, inyecta el contexto de la base de datos
        public ProductosController(PcStoreContext context)
        {
            _context = context;
        }

        // GET: api/Productos
        // Este endpoint obtiene todos los productos y permite buscar por nombre y filtrar por nombre de categoría.
        // Cumple con: "Mostrar el catálogo completo de productos, consultando la información desde la API"
        // Cumple con: "Contar con un cuadro de búsqueda que permita buscar productos por su nombre..."
        // Cumple con: "Filtrar productos por categoría."
        //
        // Ejemplos de uso:
        // - GET api/Productos                                     -> Obtiene todos los productos.
        // - GET api/Productos?search=gamer                        -> Filtra productos por nombre o descripción que contengan "gamer".
        // - GET api/Productos?categoryName=Laptops                -> Filtra productos por el nombre de categoría "Laptops".
        // - GET api/Productos?search=teclado&categoryName=Accesorios -> Combina búsqueda por nombre y filtro por categoría.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Producto>>> GetProductos(
            [FromQuery] string search = null, // Parámetro opcional para búsqueda por nombre/descripción
            [FromQuery] string categoryName = null) // Parámetro opcional para filtrar por nombre de categoría
        {
            // Inicia una consulta sobre la tabla Productos
            var query = _context.Productos.AsQueryable();

            // Aplica el filtro de búsqueda por nombre o descripción si se proporciona el parámetro 'search'.
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(p => p.Nombre.Contains(search) || p.Descripcion.Contains(search));
            }

            // Aplica el filtro por nombre de categoría si se proporciona el parámetro 'categoryName'.
            // La comparación de categoría es insensible a mayúsculas/minúsculas para mayor flexibilidad.
            if (!string.IsNullOrEmpty(categoryName))
            {
                query = query.Where(p => p.Categoria.ToLower() == categoryName.ToLower());
            }

            // Ejecuta la consulta en la base de datos y devuelve la lista de productos.
            return await query.ToListAsync();
        }

        // GET: api/Productos/5
        // Obtiene un producto específico por su ID.
        [HttpGet("{id}")]
        public async Task<ActionResult<Producto>> GetProducto(int id)
        {
            // Busca el producto por ID. No se necesita Include ya que no hay propiedades de navegación.
            var producto = await _context.Productos.FindAsync(id);

            if (producto == null)
            {
                return NotFound(); // Retorna 404 Not Found si el producto no existe.
            }

            return producto; // Retorna el producto encontrado.
        }

        // PUT: api/Productos/5
        // Actualiza un producto existente.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProducto(int id, Producto producto)
        {
            // Verifica que el ID proporcionado en la URL coincida con el ID del producto en el cuerpo de la solicitud.
            if (id != producto.IdProducto)
            {
                return BadRequest(); // Retorna 400 Bad Request si los IDs no coinciden.
            }

            _context.Entry(producto).State = EntityState.Modified;

            // Si tu modelo 'Producto' tiene una propiedad 'FechaActualizacion' y quieres actualizarla automáticamente:
            // producto.FechaActualizacion = DateTime.Now;

            try
            {
                await _context.SaveChangesAsync(); // Guarda los cambios en la base de datos.
            }
            catch (DbUpdateConcurrencyException)
            {
                // Manejo de concurrencia: si el producto fue modificado o eliminado por otro usuario mientras se procesaba esta solicitud.
                if (!ProductoExists(id))
                {
                    return NotFound(); // Retorna 404 si el producto ya no existe en la base de datos.
                }
                else
                {
                    throw; // Lanza la excepción si es otro tipo de error de concurrencia no manejado.
                }
            }

            return NoContent(); // Retorna 204 No Content si la actualización fue exitosa.
        }

        // POST: api/Productos
        // Crea un nuevo producto.
        [HttpPost]
        public async Task<ActionResult<Producto>> PostProducto(Producto producto)
        {
            // Si tu modelo 'Producto' tiene una propiedad 'FechaCreacion' y quieres establecerla automáticamente:
            // producto.FechaCreacion = DateTime.Now;

            _context.Productos.Add(producto); // Añade el nuevo producto al contexto de Entity Framework Core.
            await _context.SaveChangesAsync(); // Guarda los cambios en la base de datos.

            // Retorna 201 CreatedAtAction, indicando que el recurso fue creado exitosamente.
            // También proporciona la URL para acceder al nuevo producto y el objeto del producto creado.
            return CreatedAtAction("GetProducto", new { id = producto.IdProducto }, producto);
        }

        // DELETE: api/Productos/5
        // Elimina un producto por su ID.
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProducto(int id)
        {
            // Busca el producto a eliminar por su ID.
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound(); // Retorna 404 si el producto no se encuentra.
            }

            _context.Productos.Remove(producto); // Marca el producto para ser eliminado del contexto.
            await _context.SaveChangesAsync(); // Guarda los cambios en la base de datos.

            return NoContent(); // Retorna 204 No Content si la eliminación fue exitosa.
        }

        // Método auxiliar privado para verificar si un producto existe por su ID.
        private bool ProductoExists(int id)
        {
            return _context.Productos.Any(e => e.IdProducto == id);
        }
    }
}
