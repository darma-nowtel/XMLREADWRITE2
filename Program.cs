using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace XMLReadWrite
{
    class Program
    {
        static void Main(string[] args)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("C:\\xml\\in\\PrefFaultSectionData.xml");
            XmlNodeList userNodes = xmlDoc.SelectNodes("//PrefFaultSectionData/FaultSectionPrefData");
            foreach (XmlNode DeformationModel in userNodes)
            {

                int faultSectionId = int.Parse(DeformationModel.Attributes["sectionId"].Value);
                var MainElement = findMainElement(faultSectionId.ToString());
                var x = MainElement.ToList();
                if (x != null && x.Count > 0)
                {
                /***
                    *  sectionName = data[1],
                        aveLowerDepth = data[2],
                        aveLongTermSlipRate = data[3],
                        slipRateStdDev = data[4],
                        aveDip = data[5],
                        aveRake= data[6],
                        aseismicSlipFactor = data[7],
                        dipDirection = data[8],
                        Latitude = data[9] ,
                        Longitude = data[10],
                        Depth = data[11]**/



                DeformationModel.Attributes["sectionName"].Value = x[0].sectionName;
                DeformationModel.Attributes["aveLowerDepth"].Value = x[0].aveLowerDepth;
                DeformationModel.Attributes["aveLongTermSlipRate"].Value = x[0].aveLongTermSlipRate;

                DeformationModel.Attributes["slipRateStdDev"].Value = x[0].slipRateStdDev;
                DeformationModel.Attributes["aveDip"].Value = x[0].aveDip;
                DeformationModel.Attributes["aveRake"].Value = x[0].aveRake;

                DeformationModel.Attributes["aseismicSlipFactor"].Value = x[0].aseismicSlipFactor;
                DeformationModel.Attributes["dipDirection"].Value = x[0].dipDirection;



                foreach (XmlNode FaultTrace in DeformationModel.ChildNodes)
                {
                    FaultTrace.Attributes["name"].Value = x[0].sectionName;


                    foreach (XmlNode Location in FaultTrace.ChildNodes)
                    {
                        Location.RemoveAll();
                    }


                    XmlElement elem = xmlDoc.CreateElement("Location");
                    elem.SetAttribute("Latitude", x[0].Latitude);
                    elem.SetAttribute("Longitude", x[0].Longitude);
                    elem.SetAttribute("Depth", x[0].Depth);

                    FaultTrace.AppendChild(elem);

                    var subElements = findChildElements(faultSectionId.ToString());
                    foreach (Location ltn in subElements.ToList())
                    {
                        XmlElement elem1 = xmlDoc.CreateElement("Location");

                        elem1.SetAttribute("Latitude", ltn.Latitude);
                        elem1.SetAttribute("Longitude", ltn.Longitude);
                        elem1.SetAttribute("Depth", ltn.Depth);

                        FaultTrace.AppendChild(elem1);
                    }


                }




                    






                }



                //userNode.Attributes["defModId"].Value = (age + 1).ToString();
                

            }
            xmlDoc.Save("C:\\xml\\out\\PrefFaultSectionDataOutput.xml");
        }


        public static IEnumerable<FaultSectionPrefData> findMainElement(string faultSectionId)
        {
            var csvData = File.ReadAllLines("C:\\xml\\csv\\PreFaultSectionData.csv");

            var MainElelement = from defModelData in csvData
                            where defModelData.StartsWith(faultSectionId+",") && defModelData.Split(",")[1].Trim()!=""
                            let data = defModelData.Split(',')
                            select new FaultSectionPrefData()
                            {
                                sectionName = data[1],
                                aveLowerDepth = data[2],
                                aveLongTermSlipRate = data[3],
                                slipRateStdDev = data[4],
                                aveDip = data[5],
                                aveRake= data[6],
                                aseismicSlipFactor = data[7],
                                dipDirection = data[8],
                                Latitude = data[9] ,
                                Longitude = data[10],
                                Depth = data[11]


                            } ;

            return MainElelement;



        }

        public static IEnumerable<Location> findChildElements(string faultSectionId)
        {
            var csvData = File.ReadAllLines("C:\\xml\\csv\\PreFaultSectionData.csv");

            var childList = from defModelData in csvData
                               where defModelData.StartsWith(faultSectionId + ",") && defModelData.Split(",")[1].Trim() == ""
                               let data = defModelData.Split(',')
                               select new Location()
                               {
                                   Latitude = data[9],
                                   Longitude = data[10],
                                   Depth = data[11]
                               };

            return childList;



        }


        public class FaultSectionPrefData
        {
            public string sectionName { get; set; }
            public string aveLowerDepth { get; set; }
            public string aveLongTermSlipRate { get; set; }
            public string slipRateStdDev { get; set; }
            public string aveDip { get; set; }
            public string aveRake { get; set; }
            public string aseismicSlipFactor { get; set; }
            public string dipDirection { get; set; }
            public string Latitude { get; set; }
            public string Longitude { get; set; }
            public string Depth { get; set; }


        }

        public class Location
        {
            public string Latitude { get; set; }
            public string Longitude { get; set; }
            public string Depth { get; set; }
        }
    }
}
