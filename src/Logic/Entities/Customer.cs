using System;
using System.Collections.Generic;
using System.Linq;

namespace Logic.Entities
{
    public class Customer : Entity
    {
        private string _name;
        public virtual CustomerName Name
        {
            //get => CustomerName.Create(_name).Value;
            //set => _name = value.Value;
            get => (CustomerName)_name; //Conversao explicita;
            set => _name = value; //Conversao implicita;
        }

        private string _email;
        public virtual Email Email
        {
            //get => Email.Create(_email).Value;
            //set => _email = value.Value;
            get => (Email)_email; //Conversao explicita;
            protected set => _email = value; //Conversao implicita;
        }

        public CustomerStatus Status { get; set; }

        private decimal _moneySpent;
        public virtual Dollars MoneySpent
        {
            //get => Dollars.Create(_moneySpent).Value;
            //set => _moneySpent = value.Value;
            get => Dollars.Of(_moneySpent); //Conversao explicita;
            protected set => _moneySpent = value; //Conversao implicita;
        }

        private readonly IList<PurchasedMovie> _purchasedMovies;
        public virtual IReadOnlyList<PurchasedMovie> PurchasedMovies => _purchasedMovies.ToList();

        protected Customer()
        {
            _purchasedMovies = new List<PurchasedMovie>();
        }

        public Customer(CustomerName name, Email email) : this() //this() Chama o construtor `protect` garantindo que _purchasedMovies seja instanciado.
        {
            _name = name ?? throw new ArgumentException(nameof(name));
            _email = email ?? throw new ArgumentException(nameof(email));

            MoneySpent = Dollars.Of(0);
            Status = CustomerStatus.Regular;
        }

        //public virtual void AddPurchasedMovie(PurchasedMovie purchasedMovie, Dollars price)
        public virtual void AddPurchasedMovie(Movie movie, ExpirationDate expirationDate, Dollars price)
        {
            var purchasedMovie = new PurchasedMovie
            {
                MovieId = movie.Id,
                CustomerId = Id,
                ExpirationDate = expirationDate,
                Price = price,
                PurchaseDate = DateTime.UtcNow
            };

            _purchasedMovies.Add(purchasedMovie);
            MoneySpent += price;
        }
    }
}
