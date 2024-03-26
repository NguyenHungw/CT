using CT.MOD;
using CT.ULT;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.DAL
{
	public class DonViDAL
	{
		//private string SQLHelper.appConnectionStrings = "Data Source=DESKTOP-PMRM3DP\\SQLEXPRESS;Initial Catalog=CT;Persist Security Info=True;User ID=Hungw;Password=123456;Trusted_Connection=True;Max Pool Size=100";
		SqlConnection SQLCon = null;
		public BaseResultMOD getdsDonVi(int page)
		{
			const int productperpage = 20;
			int startpage = productperpage * (page - 1);
			var result = new BaseResultMOD();
			try
			{
				List<DonViMOD> dsdv = new List<DonViMOD>();
				using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
				{
					SQLCon.Open();
					SqlCommand cmd = new SqlCommand();
					cmd.CommandType = CommandType.Text;
					cmd.CommandText = @"SELECT *
										FROM DonViTinh
										ORDER BY ID_DonVi
										OFFSET @StartPage ROWS
                                        FETCH NEXT @ProductPerPage ROWS ONLY;";

					cmd.Parameters.AddWithValue("@StartPage", startpage);
					cmd.Parameters.AddWithValue("@ProductPerPage", productperpage);
					cmd.Connection = SQLCon;
					cmd.ExecuteNonQuery();
					SqlDataReader read = cmd.ExecuteReader();
					while (read.Read())
					{
						DonViMOD item = new DonViMOD();
						item.ID_DonVi = read.GetInt32(0);
						item.TenDonVi = read.GetString(1);
						item.GhiChu = read.GetString(2);

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
		public BaseResultMOD SuaDonVi(DonViMOD item)
		{
			var result = new BaseResultMOD();
			try
			{
				using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
				{
					SQLCon.Open();

					SqlCommand cmd = new SqlCommand();
					cmd.CommandType = CommandType.Text;
					cmd.Connection = SQLCon;
					cmd.CommandText = "UPDATE [DonViTinh] SET TenDonVi=@TenDonVi,GhiChu=@GhiChu where ID_DonVi=@ID_DonVi";
					cmd.Parameters.AddWithValue("@ID_DonVi", item.ID_DonVi);
					cmd.Parameters.AddWithValue("@TenDonVi", item.TenDonVi);
					cmd.Parameters.AddWithValue("@GhiChu", item.GhiChu);
					

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

		public LoaiSanPhamMOD ThongTinDonVi(int id)
		{
			LoaiSanPhamMOD item = null;

			try
			{
				if (SQLCon == null)
				{
					SQLCon = new SqlConnection(SQLHelper.appConnectionStrings);

				}
				if (SQLCon.State == ConnectionState.Closed)
				{
					SQLCon.Open();
				}
				SqlCommand cmd = new SqlCommand();
				cmd.CommandType = CommandType.Text;
				cmd.CommandText = "SELECT ID_DonVi from DonViTinh where ID_DonVi ='" + id + "'";
				cmd.Connection = SQLCon;
				SqlDataReader reader = cmd.ExecuteReader();
				while (reader.Read())
				{
					item = new LoaiSanPhamMOD();
					item.ID_LoaiSanPham = reader.GetInt32(0);
				}
				reader.Close();
			}
			catch (Exception)
			{
				throw;
			}
			return item;
		}

		public BaseResultMOD ThemDonVi(ThemMoiDonVi item)
		{
			var result = new BaseResultMOD();
			try
			{
				using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
				{
					SQLCon.Open();
					SqlCommand cmd = new SqlCommand();
					cmd.Connection = SQLCon;
					cmd.CommandType = CommandType.Text;
					cmd.CommandText = "INSERT INTO DonViTinh (TenDonVi,GhiChu) VALUES (@TenDonVi,@GhiChu)";
					cmd.Parameters.AddWithValue("@TenDonVi", item.TenDonVi);
					cmd.Parameters.AddWithValue("@GhiChu", item.GhiChu);
					cmd.ExecuteNonQuery();

					result.Status = 1;
					result.Message = "Thêm mới đơn vị thành công";
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
				using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
				{
					SQLCon.Open();
					SqlCommand cmd = new SqlCommand();
					cmd.Connection = SQLCon;
					cmd.CommandType = CommandType.Text;
					cmd.CommandText = "DELETE from DonViTinh where ID_DonVi=@ID_DonVi";
					cmd.Parameters.AddWithValue("@ID_DonVi", id);
					cmd.ExecuteReader();

					result.Status = 1;
					result.Message = "Xóa đơn vị thành công";

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
