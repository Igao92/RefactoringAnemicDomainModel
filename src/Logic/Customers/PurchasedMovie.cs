using System;
using Logic.Common;
using Logic.Movies;

namespace Logic.Customers
{
    //Entidade interna. Apenas a raiz agregada pode fazer isso(aggregate root), no caso a classe `Customer`.
    public class PurchasedMovie : Entity
    {
        public virtual Movie Movie { get; protected set; }
        public virtual Customer Customer { get; protected set; }

        private decimal _price;
        public virtual Dollars Price
        {
            //get => ExpirationDate.Create(_price).Value;
            //set => _price = value.Value;
            get => Dollars.Of(_price);//Conversao explicita;
            set => _price = value;//Conversao implicita;
        }

        public virtual DateTime PurchaseDate { get; protected set; }

        private DateTime? _expirationDate;
        public virtual ExpirationDate ExpirationDate
        {
            //get => ExpirationDate.Create(_statusExpirationDate).Value;
            //set => _statusExpirationDate = value.Value;
            get => (ExpirationDate)_expirationDate;//Conversao explicita;
            protected set => _expirationDate = value;//Conversao implicita;
        }

        protected PurchasedMovie()
        {

        }

        internal PurchasedMovie(Movie movie, Customer customer, Dollars price, ExpirationDate expirationDate)
        {
            if (price == null || price.IsZero)
                throw new ArgumentException(nameof(price));

            if (expirationDate == null || expirationDate.IsExpired)
                throw new ArgumentException(nameof(price));

            Movie = movie ?? throw new ArgumentException(nameof(movie));
            Customer = customer ?? throw new ArgumentException(nameof(customer));
            Price = price;
            ExpirationDate = expirationDate;
            PurchaseDate = DateTime.UtcNow;
        }
    }
}
