using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class Helper
{
    public static void Log(string str, object obj)
    {
        //DateTime currentTime = new DateTime();
        //currentTime = DateTime.Now;
        Console.WriteLine(DateTime.Now + ":" + str, obj);
    }
    public static void Log(string str)
    {

        Console.WriteLine(DateTime.Now + ":" + str);
    }
}
