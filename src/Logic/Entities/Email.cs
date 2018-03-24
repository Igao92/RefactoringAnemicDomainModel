using System;
using System.Text.RegularExpressions;

namespace Logic.Entities
{
    public class Email : ValueObject<Email>
    {
        public string Value { get; }

        /// <summary>
        /// Construtor privado para ninguém instanciar essa classe diretamente.
        /// </summary>
        /// <param name="value">"E-mail" a ser criado.</param>
        private Email(string value)
        {
            Value = value; //TODO: Doubt-> Check if this method are being called...
        }

        /// <summary>
        /// Usamos um método "estático" ao invés do construtor da classe(método de fabrica);
        /// 1 - Esse método realiza todas as validacoes necessarias antes da criacao de um e-mail;
        /// </summary>
        /// <param name="email">"E-mail" que será validado.</param>
        /// <returns> 2 - Se todas as validaçoes estiverem OK, ele retorna uma instancia do "Value Object";</returns>
        public static Result<Email> Create(string email)
        {
            email = (email ?? string.Empty).Trim();

            if (email.Length == 0)
                return Result.Fail<Email>("E-mail should not be empty.");

            if (!Regex.IsMatch(email, @"^(.+)@(.+)$"))
                return Result.Fail<Email>("E-mail is invalid.");

            return Result.Ok(new Email(email));
        }

        protected override bool EqualsCore(Email other)
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
        /// <param name="email"></param>
        public static implicit operator string(Email email)
        {
            return email.Value;
        }

        /// <summary>
        /// Esse metodo nos permite criar um Value Object explicitamente atraves da string passada por parametro.
        /// Nem toda string pode ser um E-mail valido, por isso precisa ser feito explicitamente.
        /// Exemplo: string str = (string)obj; -> Nesse caso, toda string pode ser um objeto, mas
        /// nem todo objeto pode ser uma string.
        /// Referencia:https://docs.microsoft.com/pt-br/dotnet/csharp/language-reference/keywords/explicit
        /// </summary>
        /// <param name="email"></param>
        public static explicit operator Email(string email)
        {
            return Create(email).Value;
        }
    }
}
