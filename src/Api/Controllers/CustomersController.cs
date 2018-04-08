using System;
using System.Collections.Generic;
using System.Linq;
using Logic.Dtos;
using Logic.Entities;
using Logic.Repositories;
using Logic.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    public class CustomersController : Controller
    {
        private readonly MovieRepository _movieRepository;
        private readonly CustomerRepository _customerRepository;
        private readonly CustomerService _customerService;

        public CustomersController(MovieRepository movieRepository, CustomerRepository customerRepository, CustomerService customerService)
        {
            _customerRepository = customerRepository;
            _movieRepository = movieRepository;
            _customerService = customerService;
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult Get(long id)
        {
            Customer customer = _customerRepository.GetById(id);
            if (customer == null)
            {
                return NotFound();
            }

            var dto = new CustomerDto
            {
                Id = customer.Id,
                Name = customer.Name.Value,
                Email = customer.Email.Value,
                MoneySpent = customer.MoneySpent,
                Status = customer.Status.Type.ToString(),
                StatusExpirationDate = customer.Status.ExpirationDate,
                PurchasedMovies = customer.PurchasedMovies.Select(s => new PurchasedMovieDto
                {
                    Price = s.Price,
                    PurchaseDate = s.PurchaseDate,
                    ExpirationDate = s.ExpirationDate,
                    Movie = new MovieDto
                    {
                        Id = s.Movie.Id,
                        Name = s.Movie.Name
                    }
                }).ToList()
            };

            return Json(dto);
            //return Json(customer);
        }

        [HttpGet]
        public JsonResult GetList()
        {
            IReadOnlyList<Customer> customers = _customerRepository.GetList();

            var dtos = customers.Select(s => new CustomerInListDto
            {
                Id = s.Id,
                Name = s.Name.Value,
                Email = s.Email.Value,
                MoneySpent = s.MoneySpent,
                Status = s.Status.Type.ToString(),
                StatusExpirationDate = s.Status.ExpirationDate
            });

            return Json(dtos);
            //return Json(customers);
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateCustomerDto item)
        {
            try
            {

                var customerNameOnError = CustomerName.Create(item.Name);
                var emailNameOnError = Email.Create(item.Name);

                var result = Result.Combine(customerNameOnError, emailNameOnError);

                if (result.IsFailure)
                {
                    return BadRequest(ModelState);
                }

                if (_customerRepository.GetByEmail(emailNameOnError.Value) != null)
                {
                    return BadRequest("Email is already in use: " + item.Email);
                }

                var customer = new Customer(customerNameOnError.Value,emailNameOnError.Value);

                _customerRepository.Add(customer);
                _customerRepository.SaveChanges();

                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, new { error = e.Message });
            }
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult Update(long id, [FromBody] UpateCustomerDto item)
        {
            try
            {

                var customerNameOnError = CustomerName.Create(item.Name);

                if (customerNameOnError.IsFailure)
                {
                    return BadRequest(ModelState);
                }

                Customer customer = _customerRepository.GetById(id);
                if (customer == null)
                {
                    return BadRequest("Invalid customer id: " + id);
                }

                customer.Name = customerNameOnError.Value;
                _customerRepository.SaveChanges();

                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, new { error = e.Message });
            }
        }

        [HttpPost]
        [Route("{id}/movies")]
        public IActionResult PurchaseMovie(long id, [FromBody] long movieId)
        {
            try
            {
                Movie movie = _movieRepository.GetById(movieId);
                if (movie == null)
                {
                    return BadRequest("Invalid movie id: " + movieId);
                }

                Customer customer = _customerRepository.GetById(id);
                if (customer == null)
                {
                    return BadRequest("Invalid customer id: " + id);
                }

                if (customer.PurchasedMovies.Any(x => x.Movie.Id == movie.Id && !x.ExpirationDate.IsExpired))
                {
                    return BadRequest("The movie is already purchased: " + movie.Name);
                }

                _customerService.PurchaseMovie(customer, movie);

                _customerRepository.SaveChanges();

                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, new { error = e.Message });
            }
        }

        [HttpPost]
        [Route("{id}/promotion")]
        public IActionResult PromoteCustomer(long id)
        {
            try
            {
                Customer customer = _customerRepository.GetById(id);
                if (customer == null)
                {
                    return BadRequest("Invalid customer id: " + id);
                }

                if (customer.Status.IsAdvanced)
                {
                    return BadRequest("The customer already has the Advanced status");
                }

                bool success = _customerService.PromoteCustomer(customer);
                if (!success)
                {
                    return BadRequest("Cannot promote the customer");
                }

                _customerRepository.SaveChanges();

                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, new { error = e.Message });
            }
        }
    }
}
