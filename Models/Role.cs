﻿using System;
using System.Collections.Generic;

namespace pizzashop.Models;

public partial class Role
{
    public Guid Roleid { get; set; }

    public string? Name { get; set; }

    public DateTime? CreatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual User? CreatedByNavigation { get; set; }

    public virtual User? UpdatedByNavigation { get; set; }

    public virtual ICollection<Userdetail> Userdetails { get; } = new List<Userdetail>();
}
