using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MES.LED.KITTING
{
    class KittingDataStructure
    {
        public class Kitting
        {
            public string orderNo { get; set; }
            public string modelId { get; set; }
            public string ModelName { get; set; }
            public List<LedTools.LedRankStruct> ledsChoosenByPlanner { get; set; }
            public OrderStatus.Status ordertatus { get; set; }
            public double confirmedGoodQty { get; set; }
            public double confirmedNgQty { get; set; }
            public double confirmedScrQty { get; set; }
            public string[] ledCol12NcGraffitiOnly
            {
                get
                {
                    return ledsChoosenByPlanner.Select(l => l.Collective12Nc).ToArray();
                }
            }


            public int smtLine { get; set; }
            public int testerId { get; set; }

            public string connectedOrder { get; set; }
            public int connectedOrderPcbOnMb { get; set; }


            public ModelInfo.ModelSpecification modelSpec;
            public string productionPlanId { get; set; }
            public string modelId_12NCFormat
            {
                get
                {
                    if (modelId.Contains('-')) return modelId;
                    return modelId.Insert(4, " ").Insert(8, " ");
                }
            }

            public double orderedQty { get; set; }
            public double shippingQty { get; set; }
            public DateTime kittingDate { get; set; }
            public DateTime endDate { get; set; }
            public DateTime plannedEnd { get; set; }
            public DateTime? plannedEndNullable
            {
                get
                {
                    if (plannedEnd == DateTime.MinValue) return null;
                    return plannedEnd;
                }
            }
            public double numberOfBins { get { return ledCol12NcGraffitiOnly.Length; } }
            public string shippingClient { get; set; }
        }
    }
}
