using Domain.DTO;
using Domain.Models;



namespace FoodDeliveryProject.Repositories
{
    public interface IRestaurant
    {
        List<string> GetRestaurants();
        RestaurantDto AddRestaurant(RestaurantCreateDto restaurantCreateDto);

        RestaurantDto GetRestaurantById(int id);

        bool UpdateRestaurantStatus(int id, bool status);

        bool DeleteRestaurant(int id);

        //List<RestaurantDto> GetRestaurantByRole(string role);

        public List<RestaurantWithFoodDto> RestaurantWithThereFoodItems();

        List<RestaurantDto> GetOpenRestaurants();
    }
}
