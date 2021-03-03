using System;
using System.Collections.Generic;
using System.Linq;

namespace hobbie.Models
{
    public class UserSummaryView
    {
        public UserSummaryView(long id, string name, List<HobbieViewModel> hobbies)
        {
            this.id = id;
            this.name = name;
            processHobbies(hobbies);
        }
        public readonly long id;
        public readonly string name;
        private string _hobbies;
        public string hobbies { get => _hobbies; }

        private void processHobbies(List<HobbieViewModel> hobbies)
        {
            if (hobbies != null)
            {
                _hobbies = string.Join(",", hobbies.Select(c => c.hobbie));
            }
        }
    }
}
