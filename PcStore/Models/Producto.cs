using System;
using System.Collections.Generic;

namespace PcStore.Models;

public partial class Producto
{
    public int IdProducto { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public double Precio { get; set; }

    public string? UrlImagen { get; set; }

    public string Categoria { get; set; } = null!;

    public DateTime? FechaCreacion { get; set; }
}
