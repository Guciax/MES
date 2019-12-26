using System;
using System.Collections.Generic;
using System.Text;

namespace MES.LED.KITTING
{
    public class MesGetData
    {
        private static string kittingColumns = "Ilosc_wyr_dobrego,Ilosc_wyr_do_poprawy,Ilosc_wyr_na_zlom,STATUS,Diody_LED,Ilosc_do_wysylki,Nr_Planu_Produkcji,Nr_Zlecenia_Produkcyjnego,NC12_wyrobu,Ilosc_wyrobu_zlecona,Data_Poczatku_Zlecenia,Data_Plan_Zakonczenia,Data_Konca_Zlecenia,IloscKIT,MRM,Klient,LiniaProdukcyjna,Tester_Id,Grupa_wyrobu,Zlecenie_powiazane,Ilosc_PcbNaMb_wyrobu_powiazanego FROM tb_Zlecenia_produkcyjne ";

        public static Dictionary<string, KittingDataStructure> GetOrdersInfoByDataReader(int daysBack)
        {
            var nc12ToName = SqlOperations.ConnectDB.Nc12ToModelFullDict();
            var mesModels = MesModels.allModels();

            Dictionary<string, KittingDataStructure> result = new Dictionary<string, KittingDataStructure>();
            DateTime dateUntil = (DateTime.Now.AddDays(-daysBack));

            string query = $@"SELECT {kittingColumns} WHERE Data_Poczatku_Zlecenia>@date AND Grupa_wyrobu='LED' ORDER BY Data_Poczatku_Zlecenia DESC";
            // All columns
            //id  int
            //Nr_Planu_Produkcji  nvarchar(40)
            //Nr_Zlecenia_Produkcyjnego nvarchar(50)
            //NC12_wyrobu nvarchar(40)
            //Ilosc_wyrobu_zlecona    float
            //Data_Poczatku_Zlecenia  datetime
            //Data_Konca_Zlecenia datetime
            //Komentarz   nvarchar(250)
            //RankA nvarchar(12)
            //RankB nvarchar(12)
            //MRM nvarchar(50)
            //STATUS smallint
            //Ilosc_wyr_dobrego   float
            //Ilosc_wyr_do_poprawy    float
            //Ilosc_wyr_na_zlom   float
            //DataCzasWydruku datetime
            //DataCzasOeBS    datetime
            //CzyWydrukowane  bit
            //CzyWOeBS    bit
            //Opis_Wyrobu nvarchar(250)
            //Numer_Klienta nvarchar(50)
            //IloscKIT    float
            //IloscDiodNaWyrob    float
            //SMTSite nchar(1)
            //SMTDate nvarchar(4)
            //CCTznak nchar(1)
            //VfTypeCode nchar(1)
            //LiniaProdukcyjna    int

            using (SqlConnection conn = new SqlConnection(@"Data Source=MSTMS010;Initial Catalog=MES;User Id=mes;Password=mes;"))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    var parameter = cmd.CreateParameter();
                    parameter.ParameterName = "@date";
                    parameter.Value = dateUntil;
                    cmd.Parameters.Add(parameter);

                    conn.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            OrderStructureByOrderNo.Kitting order = new OrderStructureByOrderNo.Kitting();
                            order.orderNo = SqlTools.SafeGetString(rdr, "Nr_Zlecenia_Produkcyjnego");
                            if (result.ContainsKey(order.orderNo)) continue;
                            order.kittingDate = SqlTools.SafeGetDateTime(rdr, "Data_Poczatku_Zlecenia");
                            order.plannedEnd = SqlTools.SafeGetDateTime(rdr, "Data_Plan_Zakonczenia");
                            order.productionPlanId = SqlTools.SafeGetString(rdr, "Nr_Planu_Produkcji");
                            order.endDate = SqlTools.SafeGetDateTime(rdr, "Data_Konca_Zlecenia");
                            order.modelId = SqlTools.SafeGetString(rdr, "NC12_wyrobu").Replace("LLFML", "");
                            order.ModelName = GetNameFrom12NC(order.modelId + "00", nc12ToName);
                            order.shippingClient = SqlTools.SafeGetString(rdr, "Klient");
                            order.odredGroup = KittingTools.GetProductGroup(order.modelId);
                            order.orderedQty = SqlTools.SafeGetFloat(rdr, "Ilosc_wyrobu_zlecona");
                            order.shippingQty = SqlTools.SafeGetFloat(rdr, "Ilosc_do_wysylki");
                            order.ordertatus = (OrderStatus.Status)SqlTools.SafeGetSmallInt(rdr, "STATUS");
                            order.confirmedGoodQty = SqlTools.SafeGetFloat(rdr, "Ilosc_wyr_dobrego");
                            order.confirmedNgQty = SqlTools.SafeGetFloat(rdr, "Ilosc_wyr_do_poprawy");
                            order.confirmedScrQty = SqlTools.SafeGetFloat(rdr, "Ilosc_wyr_na_zlom");
                            if (order.orderedQty < 0) continue;
                            order.modelSpec = mesModels[order.modelId];
                            if (order.odredGroup == "MST")
                            {
                                order.numberOfBins = SqlTools.SafeGetFloat(rdr, "IloscKIT");
                            }
                            else
                            {
                                order.numberOfBins = 2;
                            }
                            result.Add(order.orderNo, order);
                        }
                    }
                }
            }
            return result;
        }
    }
}
}
