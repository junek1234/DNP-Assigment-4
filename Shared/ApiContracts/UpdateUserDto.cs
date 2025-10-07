using System;

namespace ApiContracts;

public class UpdateUserDto
{
    public required string OldUserName { get; set; }
    public required string OldPassword { get; set; }
    public required string NewUserName { get; set; }
    public required string NewPassword { get; set; }
}
