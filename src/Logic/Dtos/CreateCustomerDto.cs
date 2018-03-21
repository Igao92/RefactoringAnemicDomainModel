using System.ComponentModel.DataAnnotations;

namespace Logic.Dtos
{
    public class CreateCustomerDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
