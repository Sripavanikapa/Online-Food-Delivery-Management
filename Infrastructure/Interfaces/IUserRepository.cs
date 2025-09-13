using Domain.DTO;
using Domain.Models;

namespace Infrastructure.Interfaces
{
    public interface IUserRepository
    {
        public UserDto CreateUser(UserDto user);
        IEnumerable<OrderedItemsByUserDto> GetOrdersByUserId(int userId);

        public IEnumerable<Address> GetAddressesByUserId(int userId);

        UserDto UpdateUser(int id, UserDto user);

        bool DeleteUser(int id);
        //IEnumerable<AddressDto> GetAddressesByUserId(int userId);


    }
}
