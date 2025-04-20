using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SqlRawAPI.Models;
using System.Data.SqlClient;

namespace SqlRawAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly string _connectionString;
    public ProductController(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("AdonetConnection"); // appsetting den gelir.
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var products = new List<Product>();

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "Select Id, Name, Price, Stock From Products";
            SqlCommand command = new SqlCommand(query, connection);
            connection.Open();

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                products.Add(new Product
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Price = reader.GetDecimal(2),
                    Stock = reader.GetInt32(3)
                });
            }

            connection.Close();
        }

        return Ok(products);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var products = new List<Product>();
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "Select Id, Name, Price, Stock From Products Where Id = @Id";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Id", id);
            connection.Open();

            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                products.Add(new Product
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Price = reader.GetDecimal(2),
                    Stock = reader.GetInt32(3)
                });
            }
            connection.Close();
        }
        return Ok(products);
    }

    [HttpPost]
    public IActionResult AddProduct([FromBody] Product product) // json ile gelen veriyi product nesnesine çevirmek için gerekli
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "Insert Into Products (Name, Price, Stock) Values (@Name, @Price, @Stock)"; //injectionı engelledi
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Name", product.Name);
                command.Parameters.AddWithValue("@Price", product.Price);
                command.Parameters.AddWithValue("@Stock", product.Stock);

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
            return Ok("ürün başarıyla eklendi");
        }
        catch (Exception ex)
        {
            return BadRequest($"Hata: {ex.Message}");
        }
    }

    [HttpPut("{id}")]
    public IActionResult UpdateProduct(int id, [FromBody] Product product)
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "UPDATE Products Set Name = @Name, Price = @Price, Stock = @Stock Where Id = @Id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Name", product.Name);
                command.Parameters.AddWithValue("@Price", product.Price);
                command.Parameters.AddWithValue("@Stock", product.Stock);
                command.Parameters.AddWithValue("@Id", id);

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
            return Ok("ürün başarıyla güncellendi.");
        }
        catch (Exception ex)
        {
            return BadRequest($"Hata: {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteProduct(int id)
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "DELETE From Products Where Id = @Id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
            return Ok("ürün başarıyla silindi.");
        }
        catch (Exception ex)
        {
            return NotFound($"ürün bulunamadı. Hata: {ex.Message}");
        }
    }
}