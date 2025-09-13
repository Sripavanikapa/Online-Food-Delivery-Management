using Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;

namespace Infrastructure.Interfaces
{
    public interface IAddress
    {
        AddressDto CreateAddress(AddressDto addressDto);
        List<Address> GetAddressesByCustomerId(int custId);

        AddressDto UpdateAddress(int id, AddressDto addressDto);

        bool DeleteAddressById(int custid);

    }
}
