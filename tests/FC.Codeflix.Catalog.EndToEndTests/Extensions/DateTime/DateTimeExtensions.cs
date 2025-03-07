﻿namespace FC.Codeflix.Catalog.EndToEndTests.Extensions.DateTime
{
    internal static class DateTimeExtensions
    {
        public static System.DateTime TrimMilliseconds(this System.DateTime dateTime)
         => new System.DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second, 0, dateTime.Kind);
        
    }
}
