﻿using System.Text;

public static class StringExtensions
{
    public static string FirstCharToUpper(this string str)
    {
        string ret = str;

        if(!string.IsNullOrEmpty(str))
        {
            StringBuilder sb = new StringBuilder(ret);

            sb[0] = char.ToUpper(sb[0]);

            ret = sb.ToString();
        }

        return ret;
    }
}
