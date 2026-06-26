using System;
using GymManagementSystem.DAL.Models.Eunms;

namespace GymManagementSystem.BLL.ViewModels.Trainers
{
    public class TrainerViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Phone { get; set; } = default!;
        public string Gender { get; set; } = default!;
        public string DateOfBirth { get; set; } = default!;
        public string Address { get; set; } = default!;
        public Specialty Specialty { get; set; }
    }
}