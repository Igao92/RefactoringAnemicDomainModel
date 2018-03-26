using System;
using Newtonsoft.Json;

namespace Logic.Entities
{
    public class PurchasedMovie : Entity
    {
        public virtual long MovieId { get; set; }

        public virtual Movie Movie { get; set; }

        public virtual long CustomerId { get; set; }

        private decimal _price;
        public virtual Dollars Price
        {
            //get => ExpirationDate.Create(_price).Value;
            //set => _price = value.Value;
            get => Dollars.Of(_price);//Conversao explicita;
            set => _price = value;//Conversao implicita;
        }

        public virtual DateTime PurchaseDate { get; set; }

        private DateTime? _expirationDate;
        public virtual ExpirationDate ExpirationDate
        {
            //get => ExpirationDate.Create(_statusExpirationDate).Value;
            //set => _statusExpirationDate = value.Value;
            get => (ExpirationDate)_expirationDate;//Conversao explicita;
            set => _expirationDate = value;//Conversao implicita;
        }
    }
}
