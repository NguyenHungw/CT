using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CT.Controllers.TestBillimg
{
    public class TextModel
    {
        public string Tien { get; set; }
    }

        [Route("api/[controller]")]
    [ApiController]
    public class TestNum : ControllerBase
    {
        private string ConvertNumberToWords(long number)
        {
            if (number == 0)
                return "không";

            string words = "";

            if (number < 0)
            {
                words += "âm ";
                number = -number;
            }

            string[] strones = { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
            string[] strchucs = { "", " mười", " hai mươi", " ba mươi", " bốn mươi", " năm mươi", " sáu mươi", " bảy mươi", " tám mươi", " chín mươi" };
            string[] strdonvis = { "", " nghìn", " triệu", " tỷ" };

            int i = 0;
            while (number > 0)
            {
                int donvi = (int)(number % 10);
                int chuc = (int)((number % 100) / 10);
                int tram = (int)((number % 1000) / 100);

                string strtram = (tram > 0) ? (strones[tram] + " trăm") : "";
                string strchuc = (chuc > 0) ? (strchucs[chuc] + " ") : "";
                string strdonvi = (donvi > 0) ? (strones[donvi] + " ") : "";

                if (donvi == 1 && chuc > 1)
                    strdonvi = " mốt ";
                if (donvi == 5 && chuc > 0)
                    strdonvi = " lăm ";

                words = strtram + strchuc + strdonvi + strdonvi[i] + words;
                number = number / 1000;
                i++;
            }

            return words;
        }

        private void ConvertTienToWords(TextModel textModel)
        {
            if (textModel.Tien == "1000")
            {
                textModel.Tien = "một nghìn đồng";
            }
            else if (textModel.Tien == "10000")
            {
                textModel.Tien = "mười nghìn đồng";
            }
            else if (textModel.Tien == "100000")
            {
                textModel.Tien = "một trăm nghìn đồng";
            }
            else
            {
                // Gọi hàm ConvertNumberToWords để chuyển đổi số tiền thành chữ
                textModel.Tien = ConvertNumberToWords(Convert.ToInt64(textModel.Tien)) + " đồng";
            }
        }

    }
}
