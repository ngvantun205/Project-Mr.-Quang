using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDEduEnglish.Framwork.Bindable;

namespace TDEduEnglish.DomainModels {
    public class User : Bindable {
		private int id;

		public int Id { get => id; set => Set(ref id, value); }
		private  string fullname = "";

		public  string FullName { get => fullname; set => Set(ref fullname, value); }
        private string email = "";

        public string Email { get => email; set => Set(ref email, value); }
        private string passwordhash = "";

        public string PasswordHash { get => passwordhash; set => Set(ref passwordhash, value); }
        private string role = "";

        public string Role { get => role; set => Set(ref role, value); }
        private DateTime joindate;

        public DateTime JoinDate { get => joindate; set => Set(ref joindate, value); }
        private string level = "";

        public string Level { get => level; set => Set(ref level, value); }

    }
}
