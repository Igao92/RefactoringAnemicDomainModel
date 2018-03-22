using System;
using System.Collections.Generic;

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
            set => _email = value; //Conversao implicita;
        }
        public virtual CustomerStatus Status { get; set; }
        public virtual DateTime? StatusExpirationDate { get; set; }
        public virtual decimal MoneySpent { get; set; }
        public virtual IList<PurchasedMovie> PurchasedMovies { get; set; }
    }
}
