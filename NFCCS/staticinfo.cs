using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NFCCS
{
   public class staticinfo
    {
       public static string imgpat;
       public static string name;
       public static string addr;
       public static string contact;

        public staticinfo(string im, string nm, string ad, string conta)
       {
           imgpat = im;
           name = nm;
           addr = ad;
           contact = conta;
       }

    }
}
