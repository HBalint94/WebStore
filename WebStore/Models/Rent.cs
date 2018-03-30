using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore.Models
{
    public class Rent
    {
        public Rent()
        {
            Products = new HashSet<Product>();
        }
        [Key]
        public int Id { get; set; }
        public string ClientName { get; set; }
        public string ClientAddress { get; set;}
        public string ClientPhoneNumber { get; set; }
        public string ClientEmail { get; set; }
        public ICollection<Product> Products { get; set; }
        public Boolean Performed { get; set; }
    }
}
