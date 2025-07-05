using System;
using System.Collections.Generic;

namespace PcStore.Models;

public partial class Producto
{
    public string? Nombre { get; set; }

    public string? Descripcion { get; set; }

    public double? Precio { get; set; }

    public string? UrlImagen { get; set; }

    public string? Categoria { get; set; }
}
