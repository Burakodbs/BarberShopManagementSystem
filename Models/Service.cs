﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BarberShopManagementSystem.Models
{
    public class Service
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0, 9999.99)]
        public decimal Price { get; set; }

        [Required]
        [Range(5, 240)] // 5 ile 240 dakika arası
        public int Duration { get; set; } // Dakika cinsinden hizmet süresi

        public ICollection<Salon> Salons  { get; set; } = new List<Salon>();
    }
}