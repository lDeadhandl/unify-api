using System.Collections.Generic;
using Unify.Data;

namespace Unify.ViewModel
{
    public class PartyVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<string> Guests { get; internal set; }
    }
  
}
