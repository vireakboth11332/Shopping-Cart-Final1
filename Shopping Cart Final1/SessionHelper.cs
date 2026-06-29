using System;
using System.Collections.Generic;
using System.Text;

namespace Shopping_Cart_Final1
{
    internal class SessionHelper
    {
        public static string CurrentUser { get; set; } = "";
        public static string CurrentRole { get; set; } = "";
        public static int UserID { get; set; } = 0;
    }
}
