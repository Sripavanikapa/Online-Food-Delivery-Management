using Domain.DTO;
using Domain.Models;

namespace Infrastructure.Interfaces
{
    public interface IUserRepository
    {
        public UserDto CreateUser(UserDto user);
        IEnumerable<OrderedItemsByUserDto> GetOrdersByUserId(string phno);

        public IEnumerable<Address> GetAddressesByUserId(string phno);

        UpdateUserDto UpdateUser(UpdateUserDto user);

        bool DeleteUser(string phno);
        //IEnumerable<AddressDto> GetAddressesByUserId(int userId);


    }
}
