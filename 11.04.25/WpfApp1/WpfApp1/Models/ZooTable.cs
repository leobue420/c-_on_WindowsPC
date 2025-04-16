using System;
using System.Collections.Generic;

namespace WpfApp1.Models;

public partial class ZooTable
{
    public int Id { get; set; }

    public string Location { get; set; } = null!;

    public bool IsDeleted { get; set; }

    public virtual ICollection<ZooAnimal> ZooAnimals { get; set; } = new List<ZooAnimal>();
}
