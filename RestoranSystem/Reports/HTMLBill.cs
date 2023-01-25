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
    public class HtmlBill
    {
        public string generateHTMLraport(List<MenuLine> order)
        {
            string htmlOutput = "";
            htmlOutput += "<!DOCTYPE html><html lang = 'en'><head><meta charset = 'UTF-8'/><title>Bill</title></head><body>";
            htmlOutput += "<table style='border: solid 1px black;'>";
            htmlOutput += "<tr style='background-color: rgba(0, 0, 0, 0.8); color: rgb(255, 255, 255);'><th>Užsakymas</th><th>Kaina, Eur</th></th>";
            decimal TotalValue = 0.00m;
            foreach (MenuLine reportLine in order)
            {
                htmlOutput += $"<tr><td>{reportLine.MealName}</td><td>{reportLine.MealPrice}</td></tr>";
                TotalValue += reportLine.MealPrice;
            }
            htmlOutput += $"<tr style='background-color: rgba(0, 0, 0, 0.8); color: rgb(255, 255, 255); text-align: right;'><td>Viso:</td><td>{TotalValue}</td></tr>";
            htmlOutput += "</table>";
            htmlOutput += "</body></html>";
            return htmlOutput;
        }
    }
}
