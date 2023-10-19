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

        public (string jwtToken, string refreshToken) GenerateJwtAndRefreshToken(TaiKhoanMOD item, string userRole, List<Claim> claims)
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

            // Chèn claim vào phía trước danh sách claim hiện tại
            claims.InsertRange(0, additionalClaims);

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

            // Tạo JWT Token
            var jwtToken = tokenHandler.CreateToken(tokenDescriptor);
            var jwtTokenString = tokenHandler.WriteToken(jwtToken);

            // tạo ref token sử dụng một chuỗi ngẫu nhiên để làm rf token
            var refreshToken = Guid.NewGuid().ToString();

            // Lưu trữ refresh token 

            return (jwtTokenString, refreshToken);
        }
    }
}
