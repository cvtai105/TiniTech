using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSharedModels.Dtos.Identity;

public class LoginForm

{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}
