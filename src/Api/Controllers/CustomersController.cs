using System;
using System.Collections.Generic;
using System.Linq;
using Logic.Dtos;
using Logic.Entities;
using Logic.Repositories;
using Logic.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    public class CustomersController : BaseController
    {
        private readonly MovieRepository _movieRepository;
        private readonly CustomerRepository _customerRepository;

        public CustomersController(UnitOfWork unitOfWork, MovieRepository movieRepository, CustomerRepository customerRepository) : base(unitOfWork)
        {
            _customerRepository = customerRepository;
            _movieRepository = movieRepository;
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

            return Ok(dto);
            //return Json(customer);
        }

        [HttpGet]
        public IActionResult GetList()
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

            return Ok(dtos);
            //return Json(customers);
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateCustomerDto item)
        {
            var customerNameOnError = CustomerName.Create(item.Name);
            var emailNameOnError = Email.Create(item.Name);

            var result = Result.Combine(customerNameOnError, emailNameOnError);

            if (result.IsFailure)
            {
                return Error(result.Error);
            }

            if (_customerRepository.GetByEmail(emailNameOnError.Value) != null)
            {
                return Error("Email is already in use: " + item.Email);
            }

            var customer = new Customer(customerNameOnError.Value, emailNameOnError.Value);

            _customerRepository.Add(customer);

            return Ok();
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult Update(long id, [FromBody] UpateCustomerDto item)
        {
            var customerNameOnError = CustomerName.Create(item.Name);

            if (customerNameOnError.IsFailure)
            {
                return Error(customerNameOnError.Error);
            }

            Customer customer = _customerRepository.GetById(id);
            if (customer == null)
            {
                return Error("Invalid customer id: " + id);
            }

            customer.Name = customerNameOnError.Value;

            return Ok();
        }

        [HttpPost]
        [Route("{id}/movies")]
        public IActionResult PurchaseMovie(long id, [FromBody] long movieId)
        {
            Movie movie = _movieRepository.GetById(movieId);
            //if (movie == null)
            //{
            //    return Error("Invalid movie id: " + movieId);
            //}

            Customer customer = _customerRepository.GetById(id);
            if (customer == null)
            {
                return Error("Invalid customer id: " + id);
            }

            if (customer.PurchasedMovies.Any(x => x.Movie.Id == movie.Id && !x.ExpirationDate.IsExpired))
            {
                return Error("The movie is already purchased: " + movie.Name);
            }

            customer.PurchasedMovie(movie);

            return Ok();
        }

        [HttpPost]
        [Route("{id}/promotion")]
        public IActionResult PromoteCustomer(long id)
        {
            Customer customer = _customerRepository.GetById(id);
            if (customer == null)
            {
                return Error("Invalid customer id: " + id);
            }

            if (customer.Status.IsAdvanced)
            {
                return Error("The customer already has the Advanced status");
            }

            bool success = customer.Promote();
            if (!success)
            {
                return Error("Cannot promote the customer");
            }

            return Ok();
        }
    }
}
