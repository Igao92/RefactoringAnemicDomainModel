using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Entities
{
    public class Dollars : ValueObject<Dollars>
    {
        private const decimal MaxDollarAmount = 1_000_000; //1 Million

        public decimal Value{ get; set; }

        public Dollars(decimal value)
        {
            Value = value;
        }

        public static Result<Dollars> Create(decimal dollarAmount)
        {
            if (dollarAmount < 0)
                return Result.Fail<Dollars>("Dollar amount cannot be negative");

            if (dollarAmount > MaxDollarAmount)
                return Result.Fail<Dollars>($"Dollar amount cannot be greater than {MaxDollarAmount}");

            if (dollarAmount % 0.01m > 0)
                return Result.Fail<Dollars>($"Dollar amount cannot be greater than {MaxDollarAmount}");

            return Result.Ok(new Dollars(dollarAmount));

        }

        protected override bool EqualsCore(Dollars other)
        {
            return Value == other.Value;
        }

        protected override int GetHashCodeCore()
        {
            return Value.GetHashCode();
        }

        public static Dollars Of(decimal dollarAmount)
        {
            return Create(dollarAmount).Value;
        }

        //Conversoes do objeto.
        //Nota: Isso evita o "Boilerplate code"
        //Referencia: https://pt.wikipedia.org/wiki/Boilerplate_code
        /*Em programação de computadores, código boilerplate ou boilerplate se refere a seções de código que
          devem ser incluídas em muitos lugares com pouca ou nenhuma alteração. 
          Ele é muitas vezes usado quando se refere a linguagens que são consideradas detalhadas, 
          onde o programador deve escrever ~muito código~ para fazer tarefas mínimas.*/

        /// <summary>
        /// Esse metodo nos permite converter implicitamente um Value Object para uma string.
        /// Referencia:https://docs.microsoft.com/pt-br/dotnet/csharp/language-reference/keywords/implicit
        /// </summary>
        /// <param name="customerName"></param>
        public static implicit operator decimal(Dollars dollars)
        {
            return dollars.Value;
        }

        /// <summary>
        /// Esse metodo nos permite criar um Value Object explicitamente atraves da string passada por parametro.
        /// Nem toda string pode ser um CustomerName valido, por isso precisa ser feito explicitamente.
        /// Exemplo: string str = (string)obj; -> Nesse caso, toda string pode ser um objeto, mas
        /// nem todo objeto pode ser uma string.
        /// /// Referencia:https://docs.microsoft.com/pt-br/dotnet/csharp/language-reference/keywords/explicit
        /// </summary>
        /// <param name="customerName"></param>
        //public static explicit operator Dollars(decimal dollars)
        //{
        //    return Create(dollars).Value;
        //}

        public static Dollars operator *(Dollars dollars, decimal multiplier)
        {
            return new Dollars(dollars * multiplier);
        }

        public static Dollars operator +(Dollars dollars1, Dollars dollars2)
        {
            return new Dollars(dollars1.Value + dollars2.Value);
        }
    }
}
