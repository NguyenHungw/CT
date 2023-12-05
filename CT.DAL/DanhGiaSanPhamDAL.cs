using CT.MOD;
using Microsoft.Data.SqlClient;
using OfficeOpenXml.Drawing.Chart;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.DAL
{
	public class DanhGiaSanPhamDAL
	{
		private string strcon = "Data Source=DESKTOP-PMRM3DP\\SQLEXPRESS;Initial Catalog=CT;Persist Security Info=True;User ID=Hungw;Password=123456;Trusted_Connection=True;Max Pool Size=100";
		SqlConnection SQLCon = null;
		public BaseResultMOD getdsDanhGia(int page)
		{
			const int productperpage = 20;
			int startpage = productperpage * (page - 1);
			var result = new BaseResultMOD();
			try
			{
				List<DanhGiaSanPhamMOD> dsdv = new List<DanhGiaSanPhamMOD>();
				using (SqlConnection SQLCon = new SqlConnection(strcon))
				{
					SQLCon.Open();
					SqlCommand cmd = new SqlCommand();
					cmd.CommandType = CommandType.Text;
					cmd.CommandText = @"SELECT *
										FROM DanhGiaSanPham
										ORDER BY id
										OFFSET @StartPage ROWS
                                        FETCH NEXT @ProductPerPage ROWS ONLY;";

					cmd.Parameters.AddWithValue("@StartPage", startpage);
					cmd.Parameters.AddWithValue("@ProductPerPage", productperpage);
					cmd.Connection = SQLCon;
					cmd.ExecuteNonQuery();
					SqlDataReader read = cmd.ExecuteReader();
					while (read.Read())
					{
						DanhGiaSanPhamMOD item = new DanhGiaSanPhamMOD();
						item.id = read.GetInt32(0);
						item.MSanPham = read.GetString(1);
						item.idUser = read.GetInt32(2);
						item.DiemDanhGia = read.GetInt32(3);
						item.NhanXet = read.GetString(4);
						item.NgayDanhGia = read.GetDateTime(5);

						dsdv.Add(item);
					}
					read.Close();
					result.Status = 1;
					result.Data = dsdv;


				}
			}
			catch (Exception ex)
			{
				result.Status = -1;
				result.Message = ULT.Constant.API_Error_System;
			}
			return result;
		}
		public BaseResultMOD SuaDanhGia(SuaDanhGiaSanPhamMOD item )
		{
			var result = new BaseResultMOD();
			try
			{
				using (SqlConnection SQLCon = new SqlConnection(strcon))
				{
					SQLCon.Open();

					SqlCommand cmd = new SqlCommand();
					cmd.CommandType = CommandType.Text;
					cmd.Connection = SQLCon;
					cmd.CommandText = "UPDATE [DanhGiaSanPham] SET MSanPham=@MSanPham,idUser=@idUser,DiemDanhGia=@DiemDanhGia,NhanXet=@NhanXet where id=@id";
					cmd.Parameters.AddWithValue("@MSanPham", item.MSanPham);
					cmd.Parameters.AddWithValue("@idUser", item.idUser);
					cmd.Parameters.AddWithValue("@DiemDanhGia", item.DiemDanhGia);
					cmd.Parameters.AddWithValue("@NhanXet", item.NhanXet);
					cmd.Parameters.AddWithValue("@id", item.id);


                    cmd.ExecuteNonQuery();
					result.Status = 1;
					result.Message = "Chỉnh sửa thông tin thành công";
					result.Data = 1;


				}
			}
			catch (Exception ex)
			{
				result.Status = -1;
				result.Message = ULT.Constant.API_Error_System;
			}
			return result;
		}

		public DanhGiaSanPhamMOD ThongTinDanhGia(int id)
		{
            DanhGiaSanPhamMOD item = null;

			try
			{
				if (SQLCon == null)
				{
					SQLCon = new SqlConnection(strcon);

				}
				if (SQLCon.State == ConnectionState.Closed)
				{
					SQLCon.Open();
				}
				SqlCommand cmd = new SqlCommand();
				cmd.CommandType = CommandType.Text;
				cmd.CommandText = "SELECT id from DanhGiaSanPham where id ='" + id + "'";
				cmd.Connection = SQLCon;
				SqlDataReader reader = cmd.ExecuteReader();
				while (reader.Read())
				{
					item = new DanhGiaSanPhamMOD();
					item.id = reader.GetInt32(0);
				}
				reader.Close();
			}
			catch (Exception)
			{
				throw;
			}
			return item;
		}

		public BaseResultMOD ThemDanhGiaSP(ThemMoiDanhGiaSanPhamMOD item)
		{
			var result = new BaseResultMOD();
			try
			{
				using (SqlConnection SQLCon = new SqlConnection(strcon))
				{
					SQLCon.Open();
					SqlCommand cmd = new SqlCommand();
					cmd.Connection = SQLCon;
					cmd.CommandType = CommandType.Text;
					cmd.CommandText = "INSERT INTO DanhGiaSanPham (MSanPham,idUser,DiemDanhGia,NhanXet,NgayDanhGia) VALUES (@MSanPham,@idUser,@DiemDanhGia,@NhanXet,@NgayDanhGia)";
					cmd.Parameters.AddWithValue("@MSanPham", item.MSanPham);
					cmd.Parameters.AddWithValue("@idUser", item.idUser);
                    cmd.Parameters.AddWithValue("@DiemDanhGia", item.DiemDanhGia);
                    cmd.Parameters.AddWithValue("@NhanXet", item.NhanXet);
                    cmd.Parameters.AddWithValue("@NgayDanhGia", item.NgayDanhGia);
                    cmd.ExecuteNonQuery();

					result.Status = 1;
					result.Message = "Đánh giá sản phẩm thành công";
					result.Data = 1;


				}
			}
			catch (Exception ex)
			{
				result.Status = -1;
				result.Message = ULT.Constant.API_Error_System;

			}
			return result;
		}
		public BaseResultMOD XoaDonVi(int id)
		{
			var result = new BaseResultMOD();
			try
			{
				using (SqlConnection SQLCon = new SqlConnection(strcon))
				{
					SQLCon.Open();
					SqlCommand cmd = new SqlCommand();
					cmd.Connection = SQLCon;
					cmd.CommandType = CommandType.Text;
					cmd.CommandText = "DELETE from DanhGiaSanPham where id=@id";
					cmd.Parameters.AddWithValue("@ID_DonVi", id);
					cmd.ExecuteReader();

					result.Status = 1;
					result.Message = "Xóa đánh giá thành công";

				}

			}
			catch (Exception ex)
			{
				result.Status = 1;
				result.Message = ULT.Constant.API_Error_System;
			}
			return result;
		}
	}
}
