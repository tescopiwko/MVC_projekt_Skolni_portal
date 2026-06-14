
using System;
using System.ComponentModel.DataAnnotations;

namespace MVC_projekt_Skolni_portal.Models
{
    public class Grade
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string ClassName { get; set; }   // Mapuje se na name="ClassName"

        [Required]
        public string StudentName { get; set; } // Mapuje se na name="StudentName"

        [Required]
        public int Value { get; set; }          // Mapuje se na name="Value"

        [Required]
        public int Weight { get; set; }         // Mapuje se na name="Weight"

        // NOVÉ: Téma písemky / zkoušení
        public string? Description { get; set; } // Mapuje se na name="Description"

        // BONUS: Čas se uloží sám ve chvíli, kdy se vytvoří tento objekt v C#
        public DateTime DateCreated { get; set; } = DateTime.Now;
    }
}