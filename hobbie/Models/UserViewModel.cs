using System;
using System.Collections.Generic;

namespace hobbie.Models
{
    public class UserViewModel
    {
        public long id { get; set; }
        public string name { get; set; }
        public bool isDeleted { get; set; }
        public List<HobbieViewModel> hobbies { get; set; }
        public List<HobbieViewModel> newHobbies { get; set; }
        public List<HobbieViewModel> removeHobbies { get; set; }

        public UserViewModel()
        {
            hobbies = new List<HobbieViewModel>();
            newHobbies = new List<HobbieViewModel>();
            removeHobbies = new List<HobbieViewModel>();
        }
    }
}
