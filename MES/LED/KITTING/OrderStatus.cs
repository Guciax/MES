using System;
using System.Collections.Generic;
using System.Text;

namespace MES.LED.KITTING
{
    class OrderStatus
    {
        public enum Status
        {
            NotKitted = 0,
            Kitted = 1,
            SmtFinished = 2,
            ReadyToShip = 3,
            ShippedNgNotDone = 4,
            Finished = 5,
        }

        public static void ChangeOrderStatus(string orderNo, Status chooseStatus)
        {
            int status = (int)chooseStatus;

            using (SqlConnection openCon = new SqlConnection(@"Data Source=MSTMS010;Initial Catalog=MES;User Id=mes;Password=mes;"))
            {
                openCon.Open();

                string updString = "UPDATE tb_Zlecenia_produkcyjne SET STATUS=@status WHERE Nr_Zlecenia_Produkcyjnego = @orderNo";
                using (SqlCommand querySave = new SqlCommand(updString))
                {
                    querySave.Connection = openCon;
                    querySave.Parameters.AddWithValue("@status", status);
                    querySave.Parameters.AddWithValue("@orderNo", orderNo);
                    querySave.ExecuteNonQuery();
                }
            }
        }
    }
}
