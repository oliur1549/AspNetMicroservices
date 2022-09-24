using Dapper;
using Discount.API.Entities;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Discount.API.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly IConfiguration _configuration;
        public DiscountRepository(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<Coupon> GetDiscount(string productName)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>("Select * from Coupon where ProductName=@ProductName", new { ProductName = productName });

            if(coupon == null)
                return new Coupon { ProductName = "No Discount", Amount = 0, Description = "No Discription Found" };

            return coupon;
        }
        public async Task<bool> CreateDiscount(Coupon coupon)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var insert = await connection.ExecuteAsync
                ("insert into Coupon (ProductName,Amount,Description) values (@ProductName,@Amount,@Description)", 
                new { ProductName = coupon.ProductName, Amount = coupon.Amount, Description = coupon.Description });

            if (insert == 0)
                return false;

            return true;
        }
        public async Task<bool> UpdateDiscount(Coupon coupon)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var update = await connection.ExecuteAsync
                ("update Coupon set ProductName=@ProductName,Amount=@Amount,Description=@Description where Id=@Id",
                new { ProductName = coupon.ProductName, Amount = coupon.Amount, Description = coupon.Description, Id = coupon.Id });

            if (update == 0)
                return false;

            return true;
        }

        public async Task<bool> DeleteDiscount(string productName)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var update = await connection.ExecuteAsync
                ("Delete from Coupon where ProductName=@ProductName",
                new { ProductName = productName });

            if (update == 0)
                return false;

            return true;
        }


    }
}
