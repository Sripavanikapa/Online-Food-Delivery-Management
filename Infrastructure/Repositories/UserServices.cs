using Domain.Data;
using Domain.DTO;
using Domain.Models;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Repositories
{
    public class UserServices : IUserRepository
    {
        private readonly AppDbContext _context;
        public UserServices(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }
        public UserDto CreateUser(UserDto userdto)
        {
            if(userdto.Role == "admin")
            {
                return null;
            }
            var user = new User
            {
                Name = userdto.Name,
                Phoneno = userdto.Phoneno,
                Password = userdto.Password,
                IsValid = false,
                Role = userdto.Role
            };
            _context.Users.Add(user);
            _context.SaveChanges();

            return new UserDto
            {
                Name = user.Name,
                Phoneno = user.Phoneno,
                Password = user.Password,
                Role = user.Role
            };
        }

        public IEnumerable<Address> GetAddressesByUserId(string phno)
        {
            var userId = _context.Users
                         .Where(u => u.Phoneno == phno)
                         .Select(u => u.Id)
                         .FirstOrDefault();
            var addres = _context.Addresses.Find(userId);
            if (addres != null)
            {
                return _context.Addresses.Where(a => a.CustId == userId).ToList();
            }
            return Enumerable.Empty<Address>();
        }


        //get orders by user id
        public IEnumerable<OrderedItemsByUserDto> GetOrdersByUserId(string phno)
        {
            var userId = _context.Users
    .Where(u => u.Phoneno == phno)
    .Select(u => u.Id)
    .FirstOrDefault();

            return _context.OrderDetailsByUserId.FromSqlRaw("exec proc_get_ordersby_userid @userid={0}", userId)
                .ToList();

        }

       public UpdateUserDto UpdateUser(UpdateUserDto user)
        {
            var existingAddress = _context.Users.FirstOrDefault(u=>u.Phoneno == user.Phoneno);
            if (existingAddress != null)
            {
                existingAddress.Name = user.Name;
                existingAddress.Phoneno = user.Phoneno;
                existingAddress.Password = user.Password;

                _context.SaveChanges();

                return new UpdateUserDto
                {
                    Name = existingAddress.Name,
                    Phoneno = existingAddress.Phoneno,
                    Password = existingAddress.Password,
               
                };
            }

            return null;


        }

        public bool DeleteUser(string phno)
        {
            var user = _context.Users.FirstOrDefault(u=> u.Phoneno == phno);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
                return true;
            }
            return false;
        }



    }
}
