using System;

namespace Logic.Entities
{
    public class CustomerName : ValueObject<CustomerName>
    {
        public string Value { get; }

        private CustomerName(string value)
        {
            Value = value;
        }

        public static Result<CustomerName> Create(string customerName)
        {
            customerName = (customerName ?? string.Empty).Trim();

            if (customerName.Length == 0)
                return Result.Fail<CustomerName>("Customer name should not be empty.");

            if (customerName.Length > 100)
                return Result.Fail<CustomerName>("Customer is too long.");

            return Result.Ok(new CustomerName(customerName));

        }

        protected override bool EqualsCore(CustomerName other)
        {
            return Value.Equals(other.Value, StringComparison.InvariantCultureIgnoreCase);
        }

        protected override int GetHashCodeCore()
        {
            return Value.GetHashCode();
        }

        /// <summary>
        /// Esse metodo nos permite converter implicitamente um Value Object para uma string.
        /// Referencia:https://docs.microsoft.com/pt-br/dotnet/csharp/language-reference/keywords/implicit
        /// </summary>
        /// <param name="customerName"></param>
        public static implicit operator string(CustomerName customerName)
        {
            return customerName.Value;
        }

        /// <summary>
        /// Esse metodo nos permite criar um Value Object atraves da string passada por parametro.
        /// Nem toda string pode ser um CustomerName valido, por isso precisa ser feito explicitamente.
        /// Exemplo: string str = (string)obj; -> Nesse caso, toda string pode ser um objeto, mas
        /// nem todo objeto pode ser uma string.
        /// /// Referencia:https://docs.microsoft.com/pt-br/dotnet/csharp/language-reference/keywords/explicit
        /// </summary>
        /// <param name="customerName"></param>
        public static explicit operator CustomerName(string customerName)
        {
            return Create(customerName).Value;
        }
    }
}
