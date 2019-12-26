using System;
using System.Collections.Generic;
using System.Text;

namespace MES
{
    public class ModelInfo
    {
        public class ModelSpecification
        {
            public string model12Nc;
            public string model12NCFormated
            {
                get
                {
                    if (model12Nc[2] == '-') return model12Nc;
                    return model12Nc.Insert(4, " ").Insert(8, " ");
                }
            }
            public string modelName;
            public int ledCountPerModel;
            public int ledCountA_LgOnly;
            public int ledCountB_LgOnly;
            public int connectorCountPerModel;
            public int connectorCountLgMstCalculated
            {
                get
                {
                    if (model12Nc[2] == '-')
                    {

                        if (model12Nc.StartsWith("K2") || model12Nc.StartsWith("G2"))
                        {
                            int connMark = int.Parse(model12Nc[8].ToString());
                            if (connMark % 2 == 0) return 4;
                        }
                        return 2;

                    }
                    return connectorCountPerModel;
                }
            }
            public int resistorCountPerModel;
            public int pcbCountPerMB;
            private string[] lgSquareTypes = new string[] { "22", "33", "53", "32" };

            /// <summary>
            /// LinLED, RecLED, etc...
            /// </summary>
            public string type
            {
                get
                {
                    if (modelName.Contains("-"))
                    {
                        var lgType = modelName.Split('-')[0].Replace("LLFML", "");
                        if (lgType.StartsWith("33"))
                        {
                            ;
                        }
                        if (lgSquareTypes.Contains(lgType)) return "RecLED";
                        return "LinLED";
                    }
                    else
                    {
                        if (modelName.ToUpper().Contains("RDLED")) return "RdLED";
                        if (modelName.ToUpper().Contains("LINLED")) return "LinLED";
                        if (modelName.ToUpper().Contains("PET")) return "LinLED";
                        if (modelName.ToUpper().Contains("RECLED")) return "RecLED";
                        return "Inne";
                    }
                }
            }
        }
    }
}
