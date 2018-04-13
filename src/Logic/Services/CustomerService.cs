using System;
using Logic.Entities;

namespace Logic.Services
{
    public class CustomerService
    {
        private readonly MovieService _movieService;

        public CustomerService(MovieService movieService)
        {
            _movieService = movieService;
        }

        private Dollars CalculatePrice(CustomerStatus status, LicensingModel licensingModel)
        {
            Dollars price;
            switch (licensingModel)
            {
                case LicensingModel.TwoDays:
                    //price = (Dollars)4;
                    price = Dollars.Of(4);
                    break;

                case LicensingModel.LifeLong:
                    //price = (Dollars)8;
                    price = Dollars.Of(8);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (status.IsAdvanced)
            {
                price = price * 0.75m; //Operador '*' sobrescrito no Dollar Object Value.
            }

            return price;
        }

        public void PurchaseMovie(Customer customer, Movie movie)
        {
            ExpirationDate expirationDate = _movieService.GetExpirationDate(movie.LicensingModel);
            Dollars price = CalculatePrice(customer.Status, movie.LicensingModel);

            customer.AddPurchasedMovie(movie, expirationDate, price);
            //customer.AddPurchasedMovie(purchasedMovie, price);
        }
    }
}
