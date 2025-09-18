using Domain.DTO;
using Domain.Models;
using FoodDeliveryProject.DTOs;

namespace Infrastructure.Interfaces
{
    public interface IUserRepository
    {
        public UserDto CreateUser(UserDto user);
        List<GetOrderDto> GetOrdersByUserId(string phno);

        public List<AddressDto> GetAddressesByUserId(string phno);

        UpdateUserDto UpdateUser(UpdateUserDto user);

        bool DeleteUser(string phno);
        //IEnumerable<AddressDto> GetAddressesByUserId(int userId);
        

    }
}
