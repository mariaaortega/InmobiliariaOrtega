using System;
using System.ComponentModel.DataAnnotations;


namespace InmobiliariaOrtega.Models
{
    public abstract class Entidad
    {
        [Key]
        public int Id { get; set; }
        public override string ToString()
        {
            return $"#{Id}";
        }
    }
}
