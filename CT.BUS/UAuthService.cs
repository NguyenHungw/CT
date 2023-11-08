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
            var ThoiGianHetHan = DateTime.Now.AddMinutes(10);

            var additionalClaims = new List<Claim>
            {
                
              /*  new Claim("PhoneNumber", item.PhoneNumber),
                new Claim("NhomNguoiDung", userRole),
                new Claim("ThoiHanDangNhap", ThoiGianHetHan.ToString(), ClaimValueTypes.Integer),*/
            
            };
            bool phoneNumberClaimExists = additionalClaims.Any(claim => claim.Type == "PhoneNumber");
            bool NhomNguoiDungClaimExists = additionalClaims.Any(claim => claim.Type == "NhomNguoiDung");
            bool ThoiHanDangNhapClaimExists = additionalClaims.Any(claim => claim.Type == "ThoiHanDangNhap");
            if (!phoneNumberClaimExists) 
            {
                // Nếu claim "PhoneNumber" chưa tồn tại, thêm nó vào danh sách
                additionalClaims.Add(new Claim("PhoneNumber", item.PhoneNumber));
            }
            if (!NhomNguoiDungClaimExists)
            {
                additionalClaims.Add(new Claim("NhomNguoiDung", userRole));
            }
            if (!NhomNguoiDungClaimExists)
            {
                additionalClaims.Add(new Claim("ThoiHanDangNhap", ThoiGianHetHan.ToString(), ClaimValueTypes.Integer));
            }

            var refreshToken = Guid.NewGuid().ToString();
            additionalClaims.Add(new Claim("RefreshToken", refreshToken));
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

            return (jwtTokenString, refreshToken);
        }

        public ( string jwtToken, string refreshToken) GenerateJwtAndRefreshToken2( string userRole, List<Claim> claims)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secretKey);
            var ThoiGianHetHan = DateTime.Now.AddMinutes(10);

            var additionalClaims = new List<Claim>(); // Khởi tạo danh sách Claims

            additionalClaims.Add(new Claim("ThoiHanDangNhap", ThoiGianHetHan.ToString(), ClaimValueTypes.Integer));

            // Xóa tất cả các claim có loại "ThoiHanDangNhap" từ danh sách claims
            var claimsToRemove = claims.Where(claim => claim.Type == "ThoiHanDangNhap").ToList();
            foreach (var claim in claimsToRemove)
            {
                claims.Remove(claim);
            }

            // Thêm các Claim mới vào danh sách claims
            claims.AddRange(additionalClaims);

            var refreshToken = Guid.NewGuid().ToString();
            

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

            return (jwtTokenString, refreshToken);
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false, // Thay đổi tùy theo cấu hình
                ValidateIssuer = false, // Thay đổi tùy theo cấu hình
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_secretKey)),
                ValidateLifetime = false // Chú ý rằng AccessToken đã hết hạn
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                SecurityToken securityToken;
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
                if (!(securityToken is JwtSecurityToken jwtSecurityToken) || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                    return null;
                var claims = principal.Claims.ToList();
                return principal;
            }
            catch
            {
                return null;
            }
        }

    }
}
