using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Drawing.Text;
using System.Globalization;
using System.Text;
using Encoder = System.Drawing.Imaging.Encoder;
using Microsoft.AspNetCore.Authorization;
using System.Net.Sockets;

public class PVCombankModel
{
    public string Gio { get; set; }
    public PointF Position { get; set; }
    public float? TextSize { get; set; }
    public string? ColorHex { get; set; } = "";
    public float ScaleX { get; set; } = 1;
    public float ScaleY { get; set; } = 1;

    public string Tien { get; set; }
    public PointF Position2 { get; set; }
    public float? TextSize2 { get; set; }
    public string? ColorHex2 { get; set; } = "";
    public float ScaleX2 { get; set; } = 1;
    public float ScaleY2 { get; set; } = 1;

    public string ThongTinChiTiet { get; set; }
    public PointF Position3 { get; set; }
    public float? TextSize3 { get; set; }
    public string? ColorHex3 { get; set; } = "";
    public float ScaleX3 { get; set; } = 1;
    public float ScaleY3 { get; set; } = 1;

    public string LoiNhan { get; set; }
    public PointF Position4 { get; set; }
    public float? TextSize4 { get; set; }
    public string ColorHex4 { get; set; } = "";
    public float ScaleX4 { get; set; } = 1;
    public float ScaleY4 { get; set; } = 1;

    public string NgayThucHien { get; set; } = DateTime.Now.ToString("dddd 'Ngày' dd 'tháng' MM 'năm' yyyy", new CultureInfo("vi-VN"));
    public PointF Position5 { get; set; }
    public float? TextSize5 { get; set; }
    public string? ColorHex5 { get; set; } = "";
    public float ScaleX5 { get; set; } = 1;
    public float ScaleY5 { get; set; } = 1;

    public string MaGiaoDich { get; set; }
    public PointF Position6 { get; set; }
    public float? TextSize6 { get; set; }
    public string? ColorHex6 { get; set; } = "";
    public float ScaleX6 { get; set; } = 1;
    public float ScaleY6 { get; set; } = 1;

    public string MaThamChieu { get; set; }
    public PointF Position7 { get; set; }
    public float? TextSize7 { get; set; }
    public string? ColorHex7 { get; set; } = "";
    public float ScaleX7 { get; set; } = 1;
    public float ScaleY7 { get; set; } = 1;
    public RectangleF HinhAnhPosition { get; set; }
    public int Quality { get; set; } = 100;
}

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class PvcombankController : ControllerBase
{
    private readonly string _backgroundImagePath = "C:\\Users\\DUY NGUYEN\\Desktop\\TemplatePVcombank_Permie.jpg";

    [HttpPost]
    [Route("PVbank")]
    public IActionResult AddTextToImage([FromBody] List<PVCombankModel> pVCombankModels)
    {
        string fontPath = "D:\\PHOTO BILL\\Font\\SFUIText-Regular.ttf";
        string HinhAnhPath = "C:\\Users\\Admin\\OneDrive\\Máy tính\\c-Sharp.png";
        string randomString = GenerateRandomString('A', 'Z');
        char randomChar = GenerateRandomChar();


        using (var backgroundImage = Image.FromFile(_backgroundImagePath))
        {
            using (var graphics = Graphics.FromImage(backgroundImage))
            {
                foreach (var pvcombank in pVCombankModels)
                {
                    CanLeTrai(pvcombank);
                    LockTextSize(pvcombank);
                    DefaultColor(pvcombank);
                    DefaultScaleX(pvcombank);
                    DefaultScaleY(pvcombank);
                    DefaultDay(pvcombank);
                    DefaultQuality(pvcombank);
                    DefaultText(pvcombank);

                    pvcombank.LoiNhan = GenerateRandomString('A','Z');
                    ConvertTienToWords(pvcombank);

                    Color textColor = !string.IsNullOrWhiteSpace(pvcombank.ColorHex) ? ColorTranslator.FromHtml(pvcombank.ColorHex) : Color.Red;
                    Color textColor2 = !string.IsNullOrWhiteSpace(pvcombank.ColorHex2) ? ColorTranslator.FromHtml(pvcombank.ColorHex2) : Color.Red;
                    Color textColor3 = !string.IsNullOrWhiteSpace(pvcombank.ColorHex3) ? ColorTranslator.FromHtml(pvcombank.ColorHex3) : Color.Red;
                    Color textColor4 = !string.IsNullOrWhiteSpace(pvcombank.ColorHex4) ? ColorTranslator.FromHtml(pvcombank.ColorHex4) : Color.Red;
                    Color textColor5 = !string.IsNullOrWhiteSpace(pvcombank.ColorHex5) ? ColorTranslator.FromHtml(pvcombank.ColorHex5) : Color.Red;
                    Color textColor6 = !string.IsNullOrWhiteSpace(pvcombank.ColorHex6) ? ColorTranslator.FromHtml(pvcombank.ColorHex6) : Color.Red;
                    Color textColor7 = !string.IsNullOrWhiteSpace(pvcombank.ColorHex7) ? ColorTranslator.FromHtml(pvcombank.ColorHex7) : Color.Red;

                    using (PrivateFontCollection privateFonts = new PrivateFontCollection())
                    {
                        privateFonts.AddFontFile(fontPath);

                        using (var font1 = new Font(privateFonts.Families[0], pvcombank.TextSize ?? 0.00f, FontStyle.Bold))
                        using (var font2 = new Font(privateFonts.Families[0], pvcombank.TextSize2 ?? 0.00f, FontStyle.Bold))
                        using (var font3 = new Font(privateFonts.Families[0], pvcombank.TextSize3 ?? 0.00f,FontStyle.Regular))
                        using (var font4 = new Font(privateFonts.Families[0], pvcombank.TextSize4 ?? 0.00f, FontStyle.Bold))
                        using (var font5 = new Font(privateFonts.Families[0], pvcombank.TextSize5 ?? 0.00f, FontStyle.Bold))
                        using (var font6 = new Font(privateFonts.Families[0], pvcombank.TextSize6 ?? 0.00f))
                        using (var font7 = new Font(privateFonts.Families[0], pvcombank.TextSize7 ?? 0.00f, FontStyle.Bold))
                        {
                             ApplyScaleTransform(graphics, pvcombank.ScaleX, pvcombank.ScaleY);
                            DrawText(graphics, pvcombank.Gio, font1, textColor, pvcombank.Position);
                            ApplyScaleTransform(graphics, pvcombank.ScaleX2, pvcombank.ScaleY2);
                            DrawText(graphics, pvcombank.Tien, font2, textColor2, pvcombank.Position2) ;
                            ApplyScaleTransform(graphics, pvcombank.ScaleX3, pvcombank.ScaleY3);
                            DrawText(graphics, pvcombank.ThongTinChiTiet, font3, textColor3, pvcombank.Position3);
                            ApplyScaleTransform(graphics, pvcombank.ScaleX4, pvcombank.ScaleY4);
                            DrawText(graphics, pvcombank.LoiNhan, font4, textColor4, pvcombank.Position4);
                            ApplyScaleTransform(graphics, pvcombank.ScaleX5, pvcombank.ScaleY5);
                            DrawText(graphics, pvcombank.NgayThucHien, font5, textColor5, pvcombank.Position5);
                            ApplyScaleTransform(graphics, pvcombank.ScaleX6, pvcombank.ScaleY6);
                            DrawText(graphics, pvcombank.MaGiaoDich, font6, textColor6, pvcombank.Position6);
                            ApplyScaleTransform(graphics, pvcombank.ScaleX7, pvcombank.ScaleY7);
                            DrawText(graphics, pvcombank.MaThamChieu, font7, textColor7, pvcombank.Position7);

                            try
                            {
                                using (var hinhAnh = Image.FromFile(HinhAnhPath))
                                {
                                    graphics.DrawImage(hinhAnh, pvcombank.HinhAnhPosition);
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Không thể tải hình ảnh: {ex.Message}");
                            }
                        }
                    }
                }
            }

            using (var stream = new MemoryStream())
            {
                var encoderParams = new EncoderParameters(1);
                encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, pVCombankModels[0].Quality);
                var jpegCodecInfo = ImageCodecInfo.GetImageEncoders().FirstOrDefault(codec => codec.FormatID == ImageFormat.Jpeg.Guid);
                backgroundImage.Save(stream, jpegCodecInfo, encoderParams);
                return File(stream.ToArray(), "image/jpeg");
            }
        }
    }

    private void CanLeTrai(PVCombankModel pvcombank)
    {
        pvcombank.Position = new PointF(31, pvcombank.Position.Y);
        //pvcombank.Position2 = new PointF(200, pvcombank.Position2.Y);
        //pvcombank.Position2 = new PointF(200, 500);
        pvcombank.Position2 = new PointF(pvcombank.Position2.X,pvcombank.Position2.Y);
        pvcombank.Position3 = new PointF(31, pvcombank.Position3.Y);
        pvcombank.Position4 = new PointF(31, pvcombank.Position4.Y);
        pvcombank.Position5 = new PointF(31, pvcombank.Position5.Y);
        pvcombank.Position6 = new PointF(31, pvcombank.Position6.Y);
        pvcombank.Position7 = new PointF(31, pvcombank.Position7.Y);
    }
    private void DefaultText(PVCombankModel pvcombank)
    {
        pvcombank.Tien = "25,713,096";
    }

    private void LockTextSize(PVCombankModel pvcombank)
    {
        pvcombank.TextSize = 30;
        //pvcombank.TextSize2 = 30;
        pvcombank.TextSize3 = 30;
        pvcombank.TextSize4 = 30;
        pvcombank.TextSize5 = 30;
        pvcombank.TextSize6 = 30;
        pvcombank.TextSize7 = 30;
    }

    private void DefaultColor(PVCombankModel pvcombank)
    {
        pvcombank.ColorHex = "";
        pvcombank.ColorHex2 = "";
        pvcombank.ColorHex3 = "";
        pvcombank.ColorHex4 = "";
        pvcombank.ColorHex5 = "";
        pvcombank.ColorHex6 = "";
        pvcombank.ColorHex7 = "";
    }

    private void DefaultScaleX(PVCombankModel pvcombank, int p = 1)
    {
        pvcombank.ScaleX = p;
     //   pvcombank.ScaleX2 = pvcombank.ScaleX2;
        pvcombank.ScaleX3 = p;
        pvcombank.ScaleX4 = p;
        pvcombank.ScaleX5 = p;
        pvcombank.ScaleX6 = p;
        pvcombank.ScaleX7 = p;
    }

    private void DefaultScaleY(PVCombankModel pvcombank, int p = 1)
    {
        pvcombank.ScaleY = p;
      //  pvcombank.ScaleY2 = pvcombank.ScaleY2;
        pvcombank.ScaleY3 = p;
        pvcombank.ScaleY4 = p;
        pvcombank.ScaleY5 = p;
        pvcombank.ScaleY6 = p;
        pvcombank.ScaleY7 = p;
    }

    private void DefaultDay(PVCombankModel pvcombank)
    {
        pvcombank.NgayThucHien = DateTime.Now.ToString("dddd 'Ngày' dd 'tháng' MM 'năm' yyyy", new CultureInfo("vi-VN"));
    }
    private void DefaultQuality(PVCombankModel pvcombank) 
    {
        pvcombank.Quality = 100;
    }

    private void ApplyScaleTransform(Graphics graphics, float scaleX, float scaleY)
    {
        graphics.ScaleTransform(scaleX, scaleY);
    }

    private void DrawText(Graphics graphics, string text, Font font, Color color, PointF position)
    {
        if (!string.IsNullOrEmpty(text))
        {
            using (var brush = new SolidBrush(color))
            {
                graphics.DrawString(text, font, brush, position);
            }
        }
    }

    private string ConvertGroup(int index)
    {
        switch (index)
        {
            case 11:
                return " decillion";
            case 10:
                return " nonillion";
            case 9:
                return " octillion";
            case 8:
                return " septillion";
            case 7:
                return " sextillion";
            case 6:
                return " quintrillion";
            case 5:
                return " nghìn triệu triệu";
            case 4:
                return " nghìn tỷ";
            case 3:
                return " tỷ";
            case 2:
                return " triệu";
            case 1:
                return " nghìn";
            case 0:
                return "";
            default:
                return "";
        }
    }

    private string ConvertDigit(char digit)
    {
        switch (digit)
        {
            case '0':
                return "không";
            case '1':
                return "một";
            case '2':
                return "hai";
            case '3':
                return "ba";
            case '4':
                return "bốn";
            case '5':
                return "năm";
            case '6':
                return "sáu";
            case '7':
                return "bảy";
            case '8':
                return "tám";
            case '9':
                return "chín";
            default:
                return "";
        }
    }

    private string ConvertTwoDigit(char digit1, char digit2)
    {
        if (digit2 == '0')
        {
            switch (digit1)
            {
                case '1':
                    return "mười";
                case '2':
                    return "hai mươi";
                case '3':
                    return "ba mươi";
                case '4':
                    return "bốn mươi";
                case '5':
                    return "năm mươi";
                case '6':
                    return "sáu mươi";
                case '7':
                    return "bảy mươi";
                case '8':
                    return "tám mươi";
                case '9':
                    return "chín mươi";
                default:
                    return "";
            }
        }
        else if (digit1 == '1')
        {
            switch (digit2)
            {
                case '1':
                    return "mười một";
                case '2':
                    return "mười hai";
                case '3':
                    return "mười ba";
                case '4':
                    return "mười bốn";
                case '5':
                    return "mười lăm";
                case '6':
                    return "mười sáu";
                case '7':
                    return "mười bảy";
                case '8':
                    return "mười tám";
                case '9':
                    return "mười chín";
                default:
                    return "";
            }
        }
        else
        {
            string temp = ConvertDigit(digit2);
            if (temp == "năm") temp = "lăm";
            if (temp == "một") temp = "mốt";
            switch (digit1)
            {
                case '2':
                    return "hai mươi " + temp;
                case '3':
                    return "ba mươi " + temp;
                case '4':
                    return "bốn mươi " + temp;
                case '5':
                    return "năm mươi " + temp;
                case '6':
                    return "sáu mươi " + temp;
                case '7':
                    return "bảy mươi " + temp;
                case '8':
                    return "tám mươi " + temp;
                case '9':
                    return "chín mươi " + temp;
                default:
                    return "";
            }
        }
    }

    private string ConvertThreeDigit(char digit1, char digit2, char digit3)
    {
        string buffer = "";

        if (digit1 == '0' && digit2 == '0' && digit3 == '0')
        {
            return "";
        }

        if (digit1 != '0')
        {
            buffer += ConvertDigit(digit1) + " trăm";
            if (digit2 != '0' || digit3 != '0')
            {
                buffer += " ";
            }
        }

        if (digit2 != '0')
        {
            buffer += ConvertTwoDigit(digit2, digit3);
        }
        else if (digit3 != '0')
        {
            buffer += ConvertDigit(digit3);
        }

        return buffer;
    }

    private string ConvertNumberToWords(long number)
    {
        string numberString = number.ToString();
        string output = "";

        if (numberString.StartsWith("-"))
        {
            output = "âm ";
            numberString = numberString.Substring(1);
        }
        else if (numberString.StartsWith("+"))
        {
            output = "dương ";
            numberString = numberString.Substring(1);
        }

        if (numberString == "0")
        {
            output += "không";
        }
        else
        {
            numberString = numberString.PadLeft(36, '0');
            var groups = new List<string>();

            for (int i = 0; i < numberString.Length; i += 3)
            {
                groups.Add(numberString.Substring(i, 3));
            }

            var groups2 = new List<string>();
            foreach (var g in groups)
            {
                groups2.Add(ConvertThreeDigit(g[0], g[1], g[2]));
            }

            for (int z = 0; z < groups2.Count; z++)
            {
                if (groups2[z] != "")
                {
                    output += groups2[z] + ConvertGroup(11 - z) + (
                        z < 11
                        && !groups2.GetRange(z + 1, groups2.Count - (z + 1)).Contains("")
                        && groups2[11] != ""
                        && groups[11][0] == '0'
                            ? " "
                            /*: ", "*/
                            : " "
                    );
                }
            }

            //  output = output.TrimEnd(',', ' ');
            output = output.TrimEnd(',', ' ');
        }

        return output + " đồng";
    }
    public static char GenerateRandomChar()
    {
        Random random = new Random();
        int num = random.Next(0, 26); // Sinh số ngẫu nhiên từ 0 đến 25

        // Chuyển đổi số nguyên thành ký tự 'A' đến 'Z'
        char randomChar = (char)('A' + num);

        return randomChar;
    }
    public static string GenerateRandomString(char prefix,char suffix)
    {
        Random random = new Random();
        StringBuilder result = new StringBuilder();
        result.Append(prefix);

        // Sinh số ngẫu nhiên từ 1 đến 9 và nối vào chuỗi kết quả
        for (int i = 0; i < 6; i++)
        {
            int digit = random.Next(1, 10);
            result.Append(digit);
        }
        result.Append(prefix);
 

      /*  // Lấy ký tự đầu từ chuỗi số ngẫu nhiên
        char firstChar = result[0];

        // Thêm ký tự đầu vào đầu chuỗi kết quả
        result.Insert(0, firstChar);*/

        return result.ToString();

    }

    private void ConvertTienToWords(PVCombankModel pvcombank)
    {
        if (long.TryParse(pvcombank.Tien, out long number))
        {
            pvcombank.Tien = ConvertNumberToWords(number);
        }
    }
}
