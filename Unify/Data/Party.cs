using System.Collections.Generic;

namespace Unify.Data
{
    public class Party
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public List<Guests> PartyUsers { get; set; }
    }
}
