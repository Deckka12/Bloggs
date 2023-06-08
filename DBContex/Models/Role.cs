﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBContex.Models
{
    public class Role: IdentityRole
    {
        public int ids { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
