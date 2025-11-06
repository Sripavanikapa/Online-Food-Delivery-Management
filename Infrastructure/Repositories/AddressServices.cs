using Domain.Data;
using Domain.DTO;
using Domain.Models;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
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
            // Defensive check: ensure the user exists
            var userExists = _context.Users.Any(u => u.Id == address.cust_id);
            if (!userExists)
            {
                throw new Exception($"User with ID {address.cust_id} does not exist.");
            }

            var newAddress = new Address
            {
                CustId = address.cust_id,
                Address1 = address.Address1
            };

            _context.Addresses.Add(newAddress);
            _context.SaveChanges();

            return newAddress.Address1;
        }


        public string UpdateAddress(int addressId, string address)
        {
            var existing = _context.Addresses.FirstOrDefault(a => a.AddressId == addressId);
            if (existing == null)
            {
                throw new Exception("Address not found");
            }

            existing.Address1 = address;
            _context.SaveChanges();

            return existing.Address1;
        }


        // Get addresses by customer ID

        public List<AddressDto> GetAddressesByPhno(int userid)
        {

            
            return _context.Addresses.Where(a => a.CustId == userid).Select(a => new AddressDto
            {
                Address1 = a.Address1,
                

            })
                                            .ToList();


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

        public object UpdateAddress(int addressid, AddressDto address)
        {
            throw new NotImplementedException();
        }
    }
    }
