namespace Entities.Models_Dto.UserDto
{
    public class UserDto
    {
        public int Id { get; set; }


        public string? Username { get; set; }

        public string? PasswordHash { get; set; }

        public int DepartmentId { get; set; }
        public bool IsActive { get; set; }

        public string? Name { get; set; }
    }
}
