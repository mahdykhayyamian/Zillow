using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Zillow.DataModels
{
    public class Utility
    {
        public static string CreateAddress(Address address)
        {
            string adr = string.Empty;
            adr += address.street + " " + address.city + ", " + address.state;
            return adr;
        }

        public static string FormatDollar(string strValue){
            double d;
            Double.TryParse(strValue, out d);

            return d.ToString("C", CultureInfo.CreateSpecificCulture("en-US"));
        }
    }
}