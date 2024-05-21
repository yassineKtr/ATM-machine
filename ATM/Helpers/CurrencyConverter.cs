using System.Collections.Generic;

namespace ATM_Machine;
public static class CurrencyConverter
{
    private static readonly Dictionary<string, double> ConversionRates = new ()
    {
        {"euro",2 },
        {"usd",1.5 },
        {"nat",1.25 }
    };
    public static decimal Convert(string currency, decimal amount) => amount * (decimal)ConversionRates[currency];  
}