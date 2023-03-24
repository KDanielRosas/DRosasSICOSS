using System;
using System.Collections.Generic;

namespace DL;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public string UserName { get; set; } = null!;

    public string? Password { get; set; }

    public virtual ICollection<Historial> Historials { get; } = new List<Historial>();
}
