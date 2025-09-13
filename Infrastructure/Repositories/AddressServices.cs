using Domain.Data;
using Domain.DTO;
using Domain.Models;
using Infrastructure.Interfaces;
using System.Linq;
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
        public AddressDto CreateAddress(AddressDto address)
        {
            var newAddress = new Address
            {
                CustId = address.CustId,
                Address1 = address.Address1
            };
            _context.Addresses.Add(newAddress);
            _context.SaveChanges();
            return new AddressDto
            {
                CustId = newAddress.CustId,
                Address1 = newAddress.Address1
            };
        }

       //Update address
        public AddressDto UpdateAddress(int addressid,AddressDto address)
        {
            var existingAddress = _context.Addresses.Find(addressid);
            if (existingAddress != null)
            {
                existingAddress.CustId = address.CustId;
                existingAddress.Address1 = address.Address1;
               
                _context.SaveChanges();
                return new AddressDto
                {
                    CustId = existingAddress.CustId,
                    Address1 = existingAddress.Address1
                };
            }
            return null;
            
        }


        // Get addresses by customer ID

        public List<Address> GetAddressesByCustomerId(int custId) {

            return _context.Addresses.Where(a => a.CustId == custId).ToList();
            

        }
        
        public bool DeleteAddressById(int custid)
        {
            var address = _context.Addresses.FirstOrDefault(a => a.CustId == custid);
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
