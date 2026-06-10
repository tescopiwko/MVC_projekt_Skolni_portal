using System.ComponentModel.DataAnnotations;

namespace MVC_projekt_Skolni_portal.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Role { get; set; } // Uloží "zak" nebo "ucitel"

        [Required]
        public string Username { get; set; } // Uživatelské jméno (Login)

        [Required]
        public string Password { get; set; } // Heslo

        [Required]
        public string Jmeno { get; set; }

        [Required]
        public string Prijmeni { get; set; }

        [Required]
        public string Adresa { get; set; }

        [Required]
        public string Telefon { get; set; }

        [Required]
        public string SkolniEmail { get; set; }

        // Tato políčka nemají [Required] (nejsou povinná pro všechny), 
        // protože žák nemá předmět a učitel nemá třídu. 
        // V databázi mohou být prázdná (budou mít hodnotu NULL).
        public string? Trida { get; set; }

        public string? Predmet { get; set; }
    }
    
}

