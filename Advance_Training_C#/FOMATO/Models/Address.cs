using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOMATO.Models
{
    public class Address
    {
        public int AddressId { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public string AddressType { get; set; } // Home, Work, etc.

        public Address(int addressId, string street, string city, string state, string zipCode, string country, string addressType)
        {
            AddressId = addressId;
            Street = street;
            City = city;
            State = state;
            ZipCode = zipCode;
            Country = country;
            AddressType = addressType;
        }

        public string GetFullAddress()
        {
            return $"{Street}, {City}, {State} {ZipCode}, {Country}";
        }

        public void DisplayAddress()
        {
            Console.WriteLine($"Address ID: {AddressId}");
            Console.WriteLine($"Type: {AddressType}");
            Console.WriteLine($"Address: {GetFullAddress()}");
        }
    }
}
