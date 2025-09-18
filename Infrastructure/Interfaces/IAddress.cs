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
        List<AddressDto> GetAddressesByPhno(string phno);

        string UpdateAddress( int addid,AddressDto addressDto);

        bool DeleteAddressById(int addressid);

    }
}
