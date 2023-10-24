using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.MOD.Jwt
{
    public class RefreshToken
    {
        public string UserId { get; set; } // ID của người dùng liên quan đến Refresh Token
        public string TokenValue { get; set; } // Giá trị của Refresh Token
        public DateTime ExpirationDate { get; set; } // Ngày hết hạn của Refresh Token
    }

}
