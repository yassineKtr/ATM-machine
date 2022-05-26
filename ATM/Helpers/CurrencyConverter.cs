using System.Collections.Generic;

namespace ATM_Machine;
public static class CurrencyConverter
{
    private readonly static Dictionary<string, float> ConversionRates = new Dictionary<string, float>
    {
        {"euro",2f },
        {"usd",1.5f },
        {"nat",1.25f }
    };
    public static decimal Convert(string currency, decimal amount) => amount * (decimal)ConversionRates[currency];  
}