using Dapper;
using Discount.API.Entities;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discount.API.Repositories
{
    public class CouponRepository : ICouponRepository
    {
        private readonly IConfiguration _configuration;
        private string _connectionString;
        public CouponRepository(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _connectionString = _configuration.GetConnectionString("Postgre");
        }
        public async Task<bool> CreateCoupon(Coupon coupon)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            var executed = await connection.ExecuteAsync("INSERT INTO Coupon(ProductName, Description, Amount, Remain) VALUES(@productName, @description, @amount, @remain)"
                                    , new { productName = coupon.ProductName, description = coupon.Description, amount = coupon.Amount, remain = coupon.Remain });
            return executed > 0;
        }

        public async Task<bool> DeleteCoupon(int id)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            var executed = await connection.ExecuteAsync("DELETE FROM Coupon WHERE Id = @id"
                                            , new { id = id });
            return executed > 0;
        }

        public async Task<bool> DeleteCouponByProductName(string productName)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            var executed = await connection.ExecuteAsync("DELETE FROM Coupon WHERE ProductName = @productName"
                                            , new { productName = productName } );
            return executed > 0;
        }

        public async Task<Coupon> GetCoupon(int id)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>("SELECT * FROM Coupon WHERE Id=@id", new { id = id });

            return coupon;
        }

        public async Task<IEnumerable<Coupon>> GetCoupons(string productName)
        {
            using var connection = new NpgsqlConnection(_connectionString);


            var coupons = await connection.QueryAsync<Coupon>("SELECT * FROM coupon WHERE ProductName = @productName", new { productName = productName });

            return coupons;
        }

        public async Task<bool> UpdateCoupon(Coupon coupon)
        {
            using var connection = new NpgsqlConnection(_connectionString);


            var executed = await connection.ExecuteAsync("UPDATE Coupon set Description=@description, Amount=@amount, Remain=@remain WHERE Id=@id"
                                                             , new { description = coupon.Description, amount = coupon.Amount, remain = coupon.Remain, id = coupon.Id });

            return executed > 0;
        }
    }
}
