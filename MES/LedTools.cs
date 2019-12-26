using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MES
{
    class LedTools
    {
        public class LedRankStruct
        {
            public string Collective12Nc { get; set; }
            public string Rank { get; set; }
            public string Collective12NcFormated => Collective12Nc.Insert(4, " ").Insert(8, " ");
            public string CollectiveFormatedRank
            {
                get
                {
                    if (Rank != null) return $"{Collective12NcFormated} {Rank}";
                    return Collective12NcFormated;
                }
            }

            public string StringForDataBaseField
            {
                get
                {
                    if (Rank != null) return $"{Collective12Nc}|{Rank}";
                    return Collective12Nc;
                }
            }
        }

        public static List<LedRankStruct> MesDbFieldToLedStruct(string mesDbField)
        {
            List<LedRankStruct> result = new List<LedRankStruct>();
            var binsSplitted = mesDbField.Split('#');
            foreach (var bin in binsSplitted)
            {
                var splitted = bin.Split('|');
                string rank = null;
                if (splitted.Length == 2)
                {
                    rank = splitted[1];
                }
                result.Add(new LedRankStruct
                {
                    Collective12Nc = splitted[0],
                    Rank = rank
                });
            }
            return result;
        }
        public static LedRankStruct GetLedStruct(string collective12Nc, string Rank)
        {
            return new LedRankStruct
            {
                Collective12Nc = collective12Nc,
                Rank = Rank
            };
        }

        public static string GetMesFieldString(List<LedRankStruct> ledStruct)
        {
            return string.Join("#", ledStruct.Select(x => x.StringForDataBaseField));
        }
    }
}
