using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Drawing.Text;

public class TextModel
{
    public string Text { get; set; }
    public PointF Position { get; set; }
    public float TextSize { get; set; }
    public string ColorHex { get; set; }

    public string Text2 { get; set; }
    public PointF Position2 { get; set; }
    public float TextSize2 { get; set; }
    public string ColorHex2 { get; set; }
    public int Quality { get; set; }
}

[ApiController]
[Route("[controller]")]
public class TextOnImageController : ControllerBase
{
    private readonly string _backgroundImagePath = "E:\\Work\\Bill\\z1.jpg";

    [HttpPost]
    [Route("TestBill")]
    public IActionResult AddTextToImage([FromBody] List<TextModel> textModels)
    {
        string fontPath = "E:\\Work\\fontcompany\\Roboto\\Roboto-Thin.ttf";
        using (var backgroundImage = Image.FromFile(_backgroundImagePath))
        {
            using (var graphics = Graphics.FromImage(backgroundImage))
            {
                foreach (var textModel in textModels)
                {
                    Color textColor;
                    Color textColor2;
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

                    // Thiết lập font chữ với chế độ Bold và Strong
                    using (var font = new Font("Arial", textModel.TextSize, FontStyle.Bold ))
                    {
                        // Kiểm tra xem vị trí đã được cung cấp không
                        if (textModel.Position != null)
                        {
                            // Vẽ văn bản tại vị trí đã chỉ định với kích thước, font chữ và màu sắc tương ứng
                            using (var brush = new SolidBrush(textColor))
                            {
                                graphics.DrawString(textModel.Text, font, brush, textModel.Position);
                            }
                        }
                    }

                    // Tạo một PrivateFontCollection để load font từ file
                    using (PrivateFontCollection privateFonts = new PrivateFontCollection())
                    {
                        privateFonts.AddFontFile(fontPath);

                        using (var font = new Font(privateFonts.Families[0], textModel.TextSize2))
                        {
                            if (textModel.Position2 != null)
                            {
                                // Tạo sự mịn màng cho văn bản

                                //graphics.TextRenderingHint = TextRenderingHint.AntiAlias;

                                // Vẽ văn bản tại vị trí đã chỉ định với kích thước, font chữ và màu sắc tương ứng
                                using (var brush = new SolidBrush(textColor2))
                                {
                                    graphics.DrawString(textModel.Text2, font, brush, textModel.Position2);
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
