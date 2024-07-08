using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Drawing.Text;
using System.Globalization;

public class TextModel
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
[Route("[controller]")]
public class TextOnImageController : ControllerBase
{
    private readonly string _backgroundImagePath = "D:\\BillTemplate\\TechComBank.jpg";

    [HttpPost]
    [Route("TestBill")]
    public IActionResult AddTextToImage([FromBody] List<TextModel> textModels)
    {
        string fontPath = "C:\\Windows\\Fonts\\Roboto-Regular.ttf";
        string HinhAnhPath = "C:\\Users\\Admin\\OneDrive\\Máy tính\\c-Sharp.png";

        using (var backgroundImage = Image.FromFile(_backgroundImagePath))
        {
            using (var graphics = Graphics.FromImage(backgroundImage))
            {
                foreach (var textModel in textModels)
                {
                    CanLeTrai(textModel);
                    LockTextSize(textModel);
                    DefaultColor(textModel);
                    DefaultScaleX(textModel);
                    DefaultScaleY(textModel);
                    DefaultDay(textModel);
                    ConvertTienToWords(textModel);

                    Color textColor = !string.IsNullOrWhiteSpace(textModel.ColorHex) ? ColorTranslator.FromHtml(textModel.ColorHex) : Color.Red;
                    Color textColor2 = !string.IsNullOrWhiteSpace(textModel.ColorHex2) ? ColorTranslator.FromHtml(textModel.ColorHex2) : Color.Red;
                    Color textColor3 = !string.IsNullOrWhiteSpace(textModel.ColorHex3) ? ColorTranslator.FromHtml(textModel.ColorHex3) : Color.Red;
                    Color textColor4 = !string.IsNullOrWhiteSpace(textModel.ColorHex4) ? ColorTranslator.FromHtml(textModel.ColorHex4) : Color.Red;
                    Color textColor5 = !string.IsNullOrWhiteSpace(textModel.ColorHex5) ? ColorTranslator.FromHtml(textModel.ColorHex5) : Color.Red;
                    Color textColor6 = !string.IsNullOrWhiteSpace(textModel.ColorHex6) ? ColorTranslator.FromHtml(textModel.ColorHex6) : Color.Red;
                    Color textColor7 = !string.IsNullOrWhiteSpace(textModel.ColorHex7) ? ColorTranslator.FromHtml(textModel.ColorHex7) : Color.Red;

                    using (PrivateFontCollection privateFonts = new PrivateFontCollection())
                    {
                        privateFonts.AddFontFile(fontPath);

                        using (var font1 = new Font(privateFonts.Families[0], textModel.TextSize ?? 0.00f, FontStyle.Bold))
                        using (var font2 = new Font(privateFonts.Families[0], textModel.TextSize2 ?? 0.00f, FontStyle.Bold))
                        using (var font3 = new Font(privateFonts.Families[0], textModel.TextSize3 ?? 0.00f))
                        using (var font4 = new Font(privateFonts.Families[0], textModel.TextSize4 ?? 0.00f, FontStyle.Bold))
                        using (var font5 = new Font(privateFonts.Families[0], textModel.TextSize5 ?? 0.00f, FontStyle.Bold))
                        using (var font6 = new Font(privateFonts.Families[0], textModel.TextSize6 ?? 0.00f))
                        using (var font7 = new Font(privateFonts.Families[0], textModel.TextSize7 ?? 0.00f, FontStyle.Bold))
                        {
                            ApplyScaleTransform(graphics, textModel.ScaleX, textModel.ScaleY);
                            DrawText(graphics, textModel.Gio, font1, textColor, textModel.Position);
                            ApplyScaleTransform(graphics, textModel.ScaleX2, textModel.ScaleY2);
                            DrawText(graphics, textModel.Tien, font2, textColor2, textModel.Position2);
                            ApplyScaleTransform(graphics, textModel.ScaleX3, textModel.ScaleY3);
                            DrawText(graphics, textModel.ThongTinChiTiet, font3, textColor3, textModel.Position3);
                            ApplyScaleTransform(graphics, textModel.ScaleX4, textModel.ScaleY4);
                            DrawText(graphics, textModel.LoiNhan, font4, textColor4, textModel.Position4);
                            ApplyScaleTransform(graphics, textModel.ScaleX5, textModel.ScaleY5);
                            DrawText(graphics, textModel.NgayThucHien, font5, textColor5, textModel.Position5);
                            ApplyScaleTransform(graphics, textModel.ScaleX6, textModel.ScaleY6);
                            DrawText(graphics, textModel.MaGiaoDich, font6, textColor6, textModel.Position6);
                            ApplyScaleTransform(graphics, textModel.ScaleX7, textModel.ScaleY7);
                            DrawText(graphics, textModel.MaThamChieu, font7, textColor7, textModel.Position7);

                            try
                            {
                                using (var hinhAnh = Image.FromFile(HinhAnhPath))
                                {
                                    graphics.DrawImage(hinhAnh, textModel.HinhAnhPosition);
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
                encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, textModels[0].Quality);
                var jpegCodecInfo = ImageCodecInfo.GetImageEncoders().FirstOrDefault(codec => codec.FormatID == ImageFormat.Jpeg.Guid);
                backgroundImage.Save(stream, jpegCodecInfo, encoderParams);
                return File(stream.ToArray(), "image/jpeg");
            }
        }
    }

    private void CanLeTrai(TextModel textModel)
    {
        textModel.Position = new PointF(31, textModel.Position.Y);
        textModel.Position2 = new PointF(31, textModel.Position2.Y);
        textModel.Position3 = new PointF(31, textModel.Position3.Y);
        textModel.Position4 = new PointF(31, textModel.Position4.Y);
        textModel.Position5 = new PointF(31, textModel.Position5.Y);
        textModel.Position6 = new PointF(31, textModel.Position6.Y);
        textModel.Position7 = new PointF(31, textModel.Position7.Y);
    }

    private void LockTextSize(TextModel textModel)
    {
        textModel.TextSize = 30;
        textModel.TextSize2 = 10;
        textModel.TextSize3 = 30;
        textModel.TextSize4 = 30;
        textModel.TextSize5 = 30;
        textModel.TextSize6 = 30;
        textModel.TextSize7 = 30;
    }

    private void DefaultColor(TextModel textModel)
    {
        textModel.ColorHex = "";
        textModel.ColorHex2 = "";
        textModel.ColorHex3 = "";
        textModel.ColorHex4 = "";
        textModel.ColorHex5 = "";
        textModel.ColorHex6 = "";
        textModel.ColorHex7 = "";
    }

    private void DefaultScaleX(TextModel textModel, int p = 1)
    {
        textModel.ScaleX = p;
        textModel.ScaleX2 = p;
        textModel.ScaleX3 = p;
        textModel.ScaleX4 = p;
        textModel.ScaleX5 = p;
        textModel.ScaleX6 = p;
        textModel.ScaleX7 = p;
    }

    private void DefaultScaleY(TextModel textModel, int p = 1)
    {
        textModel.ScaleY = p;
        textModel.ScaleY2 = p;
        textModel.ScaleY3 = p;
        textModel.ScaleY4 = p;
        textModel.ScaleY5 = p;
        textModel.ScaleY6 = p;
        textModel.ScaleY7 = p;
    }

    private void DefaultDay(TextModel textModel)
    {
        textModel.NgayThucHien = DateTime.Now.ToString("dddd 'Ngày' dd 'tháng' MM 'năm' yyyy", new CultureInfo("vi-VN"));
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

    private void ConvertTienToWords(TextModel textModel)
    {
        if (long.TryParse(textModel.Tien, out long number))
        {
            textModel.Tien = ConvertNumberToWords(number);
        }
    }
}
