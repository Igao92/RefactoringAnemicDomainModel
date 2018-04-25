using System;
using Logic.Common;

namespace Logic.Customers
{
    public class ExpirationDate : ValueObject<ExpirationDate>
    {
        public static readonly ExpirationDate Infinite = new ExpirationDate(null);

        public DateTime? Date { get; }

        //public bool IsExpired => Date != null && Date.Value < DateTime.UtcNow;
        public bool IsExpired => this != Infinite && Date < DateTime.UtcNow;

        private ExpirationDate(DateTime? date)
        {
            Date = date;
        }

        public static Result<ExpirationDate> Create(DateTime date)
        {
            //if (date != null && date.Value < DateTime.UtcNow)
            //if (date < DateTime.UtcNow)
                //return Result.Fail<CustomerName>("Expiration date cannot be in the past.");

            return Result.Ok(new ExpirationDate(date));
        }
        protected override bool EqualsCore(ExpirationDate other)
        {
            return Date == other.Date;
        }

        protected override int GetHashCodeCore()
        {
            return Date.GetHashCode();
        }

        //Conversoes do objeto.
        //Nota: Isso evita o "Boilerplate code"
        //Referencia: https://pt.wikipedia.org/wiki/Boilerplate_code
        /*Em programação de computadores, código boilerplate ou boilerplate se refere a seções de código que
          devem ser incluídas em muitos lugares com pouca ou nenhuma alteração. 
          Ele é muitas vezes usado quando se refere a linguagens que são consideradas detalhadas, 
          onde o programador deve escrever ~muito código~ para fazer tarefas mínimas.*/


        /// <summary>
        /// Esse metodo nos permite criar um Value Object explicitamente atraves da string passada por parametro.
        /// Nem toda string pode ser um CustomerName valido, por isso precisa ser feito explicitamente.
        /// Exemplo: string str = (string)obj; -> Nesse caso, toda string pode ser um objeto, mas
        /// nem todo objeto pode ser uma string.
        /// /// Referencia:https://docs.microsoft.com/pt-br/dotnet/csharp/language-reference/keywords/explicit
        /// </summary>
        /// <param name="customerName"></param>
        public static explicit operator ExpirationDate(DateTime? date)
        {
            if (date.HasValue)
                return Create(date.Value).Value;

            return Infinite;
            //return Create(date).Value;
        }

        /// <summary>
        /// Esse metodo nos permite converter implicitamente um Value Object para uma string.
        /// Referencia:https://docs.microsoft.com/pt-br/dotnet/csharp/language-reference/keywords/implicit
        /// </summary>
        /// <param name="customerName"></param>
        public static implicit operator DateTime? (ExpirationDate date)
        {
            return date.Date;
        }


    }
}
