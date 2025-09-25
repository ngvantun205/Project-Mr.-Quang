using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDEduEnglish.Framwork.Bindable;

namespace TDEduEnglish.DomainModels {
    public class User : Bindable {
        [Key]
        public int UserId { get; set; }
        public string FullName { get; set; } = "";
        public string Email { get; set; } = "";
        public string PasswordHash { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public string Role { get; set; } = "";
        public DateTime JoinDate { get; set; } = DateTime.Now;
        public DateTime DateOfBirth { get; set; }
        public string Level { get; set; } = "";
        public ICollection<UserListeningResult> ListeningResults { get; set; } = new List<UserListeningResult>();
        public ICollection<UserReadingResult> ReadingResults { get; set; } = new List<UserReadingResult>();
    }
}
