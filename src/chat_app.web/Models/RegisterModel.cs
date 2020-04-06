using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace chat_app.web.Models
{
    public class RegisterModel
    {
        [MaxLength(100)]
        public string Name { get; set; }
        public string Password { get; set; }
        public string RepeatPassword { get; set; }
    }
}
