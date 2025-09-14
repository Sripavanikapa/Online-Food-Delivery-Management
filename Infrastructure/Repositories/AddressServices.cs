using Domain.Data;
using Domain.DTO;
using Domain.Models;
using Infrastructure.Interfaces;
using System.Linq;
using System.Net;
namespace Infrastructure.Repositories
{
    public class AddressServices : IAddress
    {
        private readonly AppDbContext _context;
        public AddressServices(AppDbContext context)
        {
            _context = context;
        }
        
        
        // Add a new address
        public string CreateAddress(AddressDto address)
        {
            var custId = _context.Users
                .Where(u => u.Phoneno == address.Phno)
                .Select(u => u.Id)
                .FirstOrDefault();
            var newAddress = new Address
            {
                CustId = custId,
                Address1 = address.Address1
            };
            _context.Addresses.Add(newAddress);
            _context.SaveChanges();

            return newAddress.Address1;
            
        }

       //Update address
        public string UpdateAddress(int addressid,AddressDto address)
        {
            var custId = _context.Users
               .Where(u => u.Phoneno == address.Phno)
               .Select(u => u.Id)
               .FirstOrDefault();
            var existingAddress = _context.Addresses.Find(addressid);
            if (existingAddress != null)
            {
               
                existingAddress.Address1 = address.Address1;
               
                _context.SaveChanges();

                return existingAddress.Address1;
            }
            return null;
            
        }


        // Get addresses by customer ID

        public List<Address> GetAddressesByPhno(string phno) {

            var custId = _context.Users
               .Where(u => u.Phoneno == phno)
               .Select(u => u.Id)
               .FirstOrDefault();
            return _context.Addresses.Where(a => a.CustId == custId).ToList();
            

        }
        
        public bool DeleteAddressById(int addressid)
        {
            var address = _context.Addresses.Find(addressid);
            if (address != null)
            {
                _context.Addresses.Remove(address);
                _context.SaveChanges();
                return true;
            }
            return false;
        }
            
        }



    
}
