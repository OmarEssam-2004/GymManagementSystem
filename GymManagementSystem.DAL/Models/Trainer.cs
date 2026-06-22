using GymManagementSystem.DAL.Models.Eunms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystem.DAL.Models
{
    public class Trainer : GymUser
    {
        public Specialty Specialty { get; set; }
        public ICollection<Session> Sessions { get; set; }
    }
}
