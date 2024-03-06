using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.DAL.ScheduledTask
{
    public class ScheTask
    {
        private Timer _timer;
        private readonly string _connectionstring;

        public ScheTask(IConfiguration configuration)
        {
            _connectionstring = configuration.GetConnectionString("Hungconnectstring");

        }
        public void Start()
        {
            // Thực hiện công việc định kỳ mỗi ngày lúc 2 giờ sáng
            /*   var now = DateTime.Now;
               // var nextExecutionTime = new DateTime(now.Year, now.Month, now.Day, 2, 0, 0).AddDays(1);
               var nextExecutionTime = now.AddSeconds(10);

               // Thời gian còn lại đến lúc thực hiện đầu tiên
               var initialDelay = nextExecutionTime - now;

               _timer = new Timer(Dowork, null, (int)initialDelay.TotalMilliseconds, Timeout.Infinite);*/
            // Thực hiện công việc định kỳ mỗi 10 giây
            var tuan = (int)604800;
            _timer = new Timer(Dowork, null, TimeSpan.Zero, TimeSpan.FromSeconds(tuan));


        }
        private void Dowork(object state)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionstring))
                {
                    connection.Open();
                    using (var cmd = new SqlCommand("DELETE FROM [User] WHERE isActive = 0", connection))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
                Console.WriteLine("Xóa thành công.");

            }
            catch (Exception ex)
            {
                Console.WriteLine("error");
            }
        }
    }
}
