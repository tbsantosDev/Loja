using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loja.Application.DTOs.UserDTOs
{
    public class ApproveUserDto
    {
        public int Id { get; set; }
        public bool Approved { get; set; } = true;
    }
}
