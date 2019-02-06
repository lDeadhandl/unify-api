using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Unify.Data
{
    public class Party
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public List<PartyUser> PartyUsers { get; set; }
    }
}
