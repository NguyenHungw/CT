using CT.MOD;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CT.Services
{
    public class AuthService
    {
        private readonly string _secretKey;
        private readonly string _issuer;
        private readonly string _audience;

        public AuthService(string secretKey, string issuer, string audience)
        {
            _secretKey = secretKey;
            _issuer = issuer;
            _audience = audience;
        }

        public string GenerateJwtToken(TaiKhoanMOD item, string userRole, List<Claim> claims)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secretKey);
            var ThoiGianHetHan = DateTime.Now.AddMinutes(5);
            var additionalClaims = new List<Claim>
            {
                
                new Claim("PhoneNumber", item.PhoneNumber),
                new Claim("NhomNguoiDung", userRole),
                new Claim("ThoiHanDangNhap", ThoiGianHetHan.ToString(), ClaimValueTypes.Integer),

            };

            // chèn claim vào phia trước ds claim hienj tại
            claims.InsertRange(0, additionalClaims);
          //  var result = new BaseResultMOD();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = ThoiGianHetHan,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                ),
                Issuer = _issuer,
                Audience = _audience
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
          

            return tokenHandler.WriteToken(token);
        }
        
    }
}
