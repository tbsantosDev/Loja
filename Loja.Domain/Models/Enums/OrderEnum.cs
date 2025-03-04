using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loja.Domain.Models.Enums
{
    public enum OrderStatus
    {
        pending = 0,
        paid = 1,
        sent = 2,
        cancelled = 3,
    }
    public enum PaymentMethodOrder
    {
        multibanco = 0,
        mbWay = 1,
        card = 2,
    }
}
