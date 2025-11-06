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
        string CreateAddress(AddressDto addressDto);
        List<AddressDto> GetAddressesByPhno(int userid);

        string UpdateAddress(int addressId, string address);

        bool DeleteAddressById(int addressid);
        object UpdateAddress(int addressid, AddressDto address);
    }
}
