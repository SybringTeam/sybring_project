﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace sybring_project.Models.Db
{
    public class Status
    {
      
        [Key]
        public int? Id { get; set; }
        public string? Name { get; set; }



        // Navigation properties
               
        public virtual ICollection<User> Users { get; set; }

    }
}
