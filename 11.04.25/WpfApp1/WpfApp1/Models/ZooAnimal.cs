using System;
using System.Collections.Generic;

namespace WpfApp1.Models;

public partial class ZooAnimal
{
    public int Id { get; set; }

    public int ZooId { get; set; }

    public int AnimalId { get; set; }

    public virtual Animal Animal { get; set; } = null!;

    public virtual ZooTable Zoo { get; set; } = null!;
}
