using Domain.DTO;
using Domain.Models;
using FoodDeliveryProject.DTOs;

namespace Infrastructure.Interfaces
{
    public interface IUserRepository
    {
        public UserDto CreateUser(UserDto user);
        bool ForgotPassword(ForgotPasswordDto dto);
        List<AddressDto> GetAddressesByUserId(string phno);

        //public List<AddressDto> GetOrdersByUserPhno(string phno);
        public List<GetOrderDto> GetOrdersByUserId(int userId);
        UpdateUserDto UpdateUser(UpdateUserDto user);

        bool DeleteUser(string phno);
        //IEnumerable<AddressDto> GetAddressesByUserId(int userId);

        UserInfoDto GetUserInfoByUserid(int userid);
        List<OrderedItemsByUserDto> GetOrderedItems(int userid);
        List<AddressShowing> GetAddressesByUserPhno(int userid);

    }
}
