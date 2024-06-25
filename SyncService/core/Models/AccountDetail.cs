using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core.Models
{
    public class AccountDetail
    {
        public Guid UserId { get; set; }
        public User User { get; set; }
        public bool PremiumStatus { get; set; }
        
    }
}
