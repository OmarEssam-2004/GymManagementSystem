using System.ComponentModel.DataAnnotations;

namespace GymManagementSystem.BLL.ViewModels.Plans
{
    public class PlanToUpdateViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Duration is required")]
        [Range(1, 365, ErrorMessage = "Duration must be between 1 and 365 days")]
        public int DurationDays { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0, 100000, ErrorMessage = "Price must be a positive number")]
        public decimal Price { get; set; }

        public string? Description { get; set; }
    }
}