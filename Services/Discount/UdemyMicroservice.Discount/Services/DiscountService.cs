using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using UdemyMicroservices.Shared.Dtos;

namespace UdemyMicroservice.Discount.Services
{
    public class DiscountService : IDiscountService
    {
        private readonly IConfiguration _configuration;
        private readonly IDbConnection _dbConnection;

        public DiscountService(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

            _dbConnection = new NpgsqlConnection(_configuration.GetConnectionString("PostgreSql"));
        }

        public async Task<Response<List<Models.Discount>>> GetAll()
        {
            var discounts = (await _dbConnection.QueryAsync<Models.Discount>("SELECT * FROM discount")).ToList();

            return Response<List<Models.Discount>>.Success(discounts, 200);
        }

        public async Task<Response<Models.Discount>> GetById(int id)
        {
            var discount = (await _dbConnection.QueryAsync<Models.Discount>("SELECT * FROM discount WHERE id = @id", new { id })).SingleOrDefault();

            if (discount == null)
            {
                return Response<Models.Discount>.Fail("Discount not found", 404);
            }

            return Response<Models.Discount>.Success(discount, 200);
        }

        public async Task<Response<Models.Discount>> GetByCodeAndUserId(string code, string userId)
        {
            var discount = (await _dbConnection.QueryAsync<Models.Discount>("SELECT * FROM discount WHERE code = @code and userid = @userId", new { code, userId })).FirstOrDefault();

            if (discount == null)
            {
                return Response<Models.Discount>.Fail("Discount not found", 404);
            }

            return Response<Models.Discount>.Success(discount, 200);
        }

        public async Task<Response<NoContent>> Save(Models.Discount discount)
        {
            var status = await _dbConnection.ExecuteAsync("INSERT INTO discount (userid, rate, code) VALUES (@UserId, @Rate, @Code)", discount);

            if (status == 0 || status < 0)
            {
                return Response<NoContent>.Fail("An error occurred while adding", 500);
            }

            return Response<NoContent>.Success(204);
        }

        public async Task<Response<NoContent>> Update(Models.Discount discount)
        {
            var status = await _dbConnection.ExecuteAsync("UPDATE discount SET userid= @UserId, rate = @Rate, code = @Code WHERE id = @id", new
            {
                id = discount.Id,
                UserId = discount.UserId,
                Rate = discount.Rate,
                Code = discount.Code
            });

            if (status == 0 || status < 0)
            {
                return Response<NoContent>.Fail("Discount not found", 404);
            }

            return Response<NoContent>.Success(204);
        }

        public async Task<Response<NoContent>> Delete(int id)
        {
            var status = await _dbConnection.ExecuteAsync("DELETE FROM discount WHERE id = @id", new { id });

            if (status == 0 || status < 0)
            {
                return Response<NoContent>.Fail("Discount not found", 500);
            }

            return Response<NoContent>.Success(204);
        }
    }
}
