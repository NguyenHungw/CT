using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Drawing.Text;

public class TextModel
{
    public string Gio { get; set; }
    public PointF Position { get; set; }
    public float TextSize { get; set; }
    public string ColorHex { get; set; }

    public string Tien { get; set; }
    public PointF Position2 { get; set; }
    public float TextSize2 { get; set; }
    public string ColorHex2 { get; set; }

    public string ThongTinChiTiet { get; set; }
    public PointF Position3 { get; set; }
    public float TextSize3 { get; set; }
    public string ColorHex3 { get; set; }

    public string LoiNhan { get; set; }
    public PointF Position4 { get; set; }
    public float TextSize4 { get; set; }
    public string ColorHex4 { get; set; }

    public string NgayThucHien { get; set; }
    public PointF Position5 { get; set; }
    public float TextSize5 { get; set; }
    public string ColorHex5 { get; set; }

    public string MaGiaoDich { get; set; }
    public PointF Position6 { get; set; }
    public float TextSize6 { get; set; }
    public string ColorHex6 { get; set; }

    public string MaThamChieu { get; set; }
    public PointF Position7 { get; set; }
    public float TextSize7 { get; set; }
    public string ColorHex7 { get; set; }

    public int Quality { get; set; }
}

[ApiController]
[Route("[controller]")]
public class TextOnImageController : ControllerBase
{
    private readonly string _backgroundImagePath = "D:\\PHOTO BILL\\Hung\\Send\\sendd1.jpg";

    [HttpPost]
    [Route("TestBill")]
    public IActionResult AddTextToImage([FromBody] List<TextModel> textModels)
    {
        //string fontPath = "D:\\Font\\SF Pro Display\\SF Pro Display\\SF-Pro-Display\\SF-Pro-Display-Black.otf";
        string fontPath = "D:\\Font\\SF Pro Display\\SF Pro Display\\SF-Pro-Display\\SF-Pro-Display-Regular.otf";
        string fontSF = "D:\\Font\\SF Pro Display\\SF Pro Display\\SF-Pro-Display";
        string fontRoboto = "D:\\Font\\font\\A - Z\\Roboto-Regular.ttf";
        using (var backgroundImage = Image.FromFile(_backgroundImagePath))
        {
            using (var graphics = Graphics.FromImage(backgroundImage))
            {
                foreach (var textModel in textModels)
                {
                    Color textColor;
                    Color textColor2;
                    Color textColor3;
                    Color textColor4;
                    Color textColor5;
                    Color textColor6;
                    Color textColor7;
                    if (!string.IsNullOrWhiteSpace(textModel.ColorHex))
                    {
                        textColor = ColorTranslator.FromHtml(textModel.ColorHex);
                    }
                    else
                    {
                        textColor = Color.Red;
                    }
                    if (!string.IsNullOrWhiteSpace(textModel.ColorHex2))
                    {
                        textColor2 = ColorTranslator.FromHtml(textModel.ColorHex2);
                    }
                    else
                    {
                        textColor2 = Color.Red;
                    }
                    if (!string.IsNullOrWhiteSpace(textModel.ColorHex3))
                    {
                        textColor3 = ColorTranslator.FromHtml(textModel.ColorHex3);
                    }
                    else
                    {
                        textColor3 = Color.Red;
                    }
                    if (!string.IsNullOrWhiteSpace(textModel.ColorHex4))
                    {
                        textColor4 = ColorTranslator.FromHtml(textModel.ColorHex4);
                    }
                    else
                    {
                        textColor4 = Color.Red;
                    }
                    if (!string.IsNullOrWhiteSpace(textModel.ColorHex5))
                    {
                        textColor5 = ColorTranslator.FromHtml(textModel.ColorHex5);
                    }
                    else
                    {
                        textColor5 = Color.Red;
                    }
                    if (!string.IsNullOrWhiteSpace(textModel.ColorHex6))
                    {
                        textColor6 = ColorTranslator.FromHtml(textModel.ColorHex6);
                    }
                    else
                    {
                        textColor6 = Color.Red;
                    }
                    if (!string.IsNullOrWhiteSpace(textModel.ColorHex7))
                    {
                        textColor7 = ColorTranslator.FromHtml(textModel.ColorHex7);
                    }
                    else
                    {
                        textColor7 = Color.Red;
                    }
                    // Thiết lập font chữ với chế độ Bold và Strong
                    //using (var font = new Font("Arial", textModel.TextSize, FontStyle.Bold ))
                    using (PrivateFontCollection privateFonts = new PrivateFontCollection())
                    {
                        privateFonts.AddFontFile(fontPath);
                        // Kiểm tra xem vị trí đã được cung cấp không
                        using (var font = new Font(privateFonts.Families[0], textModel.TextSize, FontStyle.Bold))
                        {
                            if (textModel.Position != null)
                            {
                                // Vẽ văn bản tại vị trí đã chỉ định với kích thước, font chữ và màu sắc tương ứng
                                using (var brush = new SolidBrush(textColor))
                                {
                                    graphics.DrawString(textModel.Gio, font, brush, textModel.Position);
                                }
                            }
                        }
                    }

                    // Tạo một PrivateFontCollection để load font từ file
                    using (PrivateFontCollection privateFonts = new PrivateFontCollection())
                    {
                        privateFonts.AddFontFile(fontPath);

                        using (var font = new Font(privateFonts.Families[0], textModel.TextSize2, FontStyle.Bold))
                        {
                            if (textModel.Position2 != null)
                            {
                                // Tạo sự mịn màng cho văn bản

                                //graphics.TextRenderingHint = TextRenderingHint.AntiAlias;

                                // Vẽ văn bản tại vị trí đã chỉ định với kích thước, font chữ và màu sắc tương ứng
                                using (var brush = new SolidBrush(textColor2))
                                {
                                    graphics.DrawString(textModel.Tien, font, brush, textModel.Position2);
                                }
                            }
                        }
                    }
                    //thong tin chi tiet

                    using (PrivateFontCollection privateFonts = new PrivateFontCollection())
                    {
                        privateFonts.AddFontFile(fontPath);

                        using (var font = new Font(privateFonts.Families[0], textModel.TextSize3))
                        {
                            if (textModel.Position3 != null)
                            {
                                // Tạo sự mịn màng cho văn bản

                                //graphics.TextRenderingHint = TextRenderingHint.AntiAlias;

                                // Vẽ văn bản tại vị trí đã chỉ định với kích thước, font chữ và màu sắc tương ứng
                                using (var brush = new SolidBrush(textColor2))
                                {
                                    graphics.DrawString(textModel.ThongTinChiTiet, font, brush, textModel.Position3);
                                }
                            }
                        }
                    }
                    //loi nhan
                    using (PrivateFontCollection privateFonts = new PrivateFontCollection())
                    {
                        privateFonts.AddFontFile(fontPath);

                        using (var font = new Font(privateFonts.Families[0], textModel.TextSize4, FontStyle.Bold))
                        {
                            if (textModel.Position4 != null)
                            {
                                // Tạo sự mịn màng cho văn bản

                                //graphics.TextRenderingHint = TextRenderingHint.AntiAlias;

                                // Vẽ văn bản tại vị trí đã chỉ định với kích thước, font chữ và màu sắc tương ứng
                                using (var brush = new SolidBrush(textColor2))
                                {
                                    graphics.DrawString(textModel.LoiNhan, font, brush, textModel.Position4);
                                }
                            }
                        }
                    }
                    //ngay thuc hien
                    using (PrivateFontCollection privateFonts = new PrivateFontCollection())
                    {
                        privateFonts.AddFontFile(fontPath);

                        using (var font = new Font(privateFonts.Families[0], textModel.TextSize5, FontStyle.Bold))
                        {
                            if (textModel.Position5 != null)
                            {
                                // Tạo sự mịn màng cho văn bản

                                //graphics.TextRenderingHint = TextRenderingHint.AntiAlias;

                                // Vẽ văn bản tại vị trí đã chỉ định với kích thước, font chữ và màu sắc tương ứng
                                using (var brush = new SolidBrush(textColor2))
                                {
                                    graphics.DrawString(textModel.NgayThucHien, font, brush, textModel.Position5);
                                }
                            }
                        }
                    }
                    //ma giao dich
                    using (PrivateFontCollection privateFonts = new PrivateFontCollection())
                    {
                        privateFonts.AddFontFile(fontPath);

                        using (var font = new Font(privateFonts.Families[0], textModel.TextSize6))
                        {
                            if (textModel.Position6 != null)
                            {
                                // Tạo sự mịn màng cho văn bản

                                //graphics.TextRenderingHint = TextRenderingHint.AntiAlias;

                                // Vẽ văn bản tại vị trí đã chỉ định với kích thước, font chữ và màu sắc tương ứng
                                using (var brush = new SolidBrush(textColor2))
                                {
                                    graphics.DrawString(textModel.MaGiaoDich, font, brush, textModel.Position6);
                                }
                            }
                        }
                    }
                    //ma tham chieu
                    using (PrivateFontCollection privateFonts = new PrivateFontCollection())
                    {
                        privateFonts.AddFontFile(fontPath);

                        using (var font = new Font(privateFonts.Families[0], textModel.TextSize7, FontStyle.Bold))
                        {
                            if (textModel.Position7 != null)
                            {
                                // Tạo sự mịn màng cho văn bản

                                //graphics.TextRenderingHint = TextRenderingHint.AntiAlias;

                                // Vẽ văn bản tại vị trí đã chỉ định với kích thước, font chữ và màu sắc tương ứng
                                using (var brush = new SolidBrush(textColor2))
                                {
                                    graphics.DrawString(textModel.MaThamChieu, font, brush, textModel.Position7);
                                }
                            }
                        }
                    }
                }
            }

            // Chuyển đổi hình ảnh sang byte array để trả về
            using (var stream = new MemoryStream())
            {
                // Tạo EncoderParameter để cấu hình chất lượng hình ảnh JPEG
                var encoderParams = new EncoderParameters(1);
                encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, textModels[0].Quality); // Chất lượng ảnh

                // Tạo thông tin về định dạng hình ảnh JPEG
                var jpegCodecInfo = ImageCodecInfo.GetImageEncoders().FirstOrDefault(codec => codec.FormatID == ImageFormat.Jpeg.Guid);

                // Lưu ảnh với chất lượng đã cấu hình
                backgroundImage.Save(stream, jpegCodecInfo, encoderParams);

                return File(stream.ToArray(), "image/jpeg");
            }
        }
    }


}
