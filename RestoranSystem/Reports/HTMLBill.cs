using RestoranSystem.Struct;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RestoranSystem.Reports
{
    public class HTMLBill
    {
        private string generateHTMLraport(List<MenuLine> reportOutput)
        {
            string htmlOutput = "";
            htmlOutput += "< !DOCTYPE html ><html lang = 'en'><head><meta charset = 'UTF-8'/><title>Bill</title></head><body>";
            htmlOutput += "<table style='border: solid 1px black;'>";
            htmlOutput += "<tr style='background-color: rgba(0, 0, 0, 0.8); color: rgb(255, 255, 255);'><th>Aircraft Tail Number</th><th>Model Number</th><th>Model Description</th><th>Owner Company Name</th><th>Company Country Code</th><th>Company Country Name</th></th>";
            foreach (MenuLine reportLine in reportOutput)
            {
                //string rowStyle = reportLine.isEuropeCountry ? "style='background-color: rgba(31, 163, 255, 0.5);'" : "style='background-color: rgba(252, 99, 113, 0.5);'";
                //htmlOutput += $"<tr {rowStyle}><td>{reportLine.aircraftTailNumber}</td><td>{reportLine.modelNumber}</td><td>{reportLine.modelDescription}</td><td>{reportLine.ownerComapanyName}</td><td>{reportLine.companyCountryCode}</td><td>{reportLine.companyCountryName}</td></tr>";
            }
            htmlOutput += "</table>";
            htmlOutput += "</ body ></ html >";
            return htmlOutput;
        }
    }
}
