public static class CurrencyHelper
{
    const decimal THOUSAND = 1000;
    const decimal MILLION = THOUSAND * THOUSAND;
    const decimal BILLION = MILLION * THOUSAND;
    const decimal TRILLION = BILLION * THOUSAND;
    const decimal QUADRILLION = TRILLION * THOUSAND;
    const decimal QUINTILLION = QUADRILLION * THOUSAND;
    const decimal SEXTILLION = QUINTILLION * THOUSAND;

    public static string Beautify(decimal value)
    {
        if (value < 100m)
            return value.ToString("0.00");
        else if (value < THOUSAND)
            return value.ToString("0");
        else if (value < MILLION)
            return (value / THOUSAND).ToString("0.#") + "k";
        else if (value < BILLION)
            return (value / MILLION).ToString("0.#") + "M";
        else if (value < TRILLION)
            return (value / BILLION).ToString("0.#") + "B";
        else if (value < QUADRILLION)
            return (value / TRILLION).ToString("0.#") + "T";
        else if (value < QUINTILLION)
            return (value / QUADRILLION).ToString("0.#") + "qd";
        else if (value < SEXTILLION)
            return (value / QUINTILLION).ToString("0.#") + "Qn";
        else
            return (value / SEXTILLION).ToString("0.#") + "sx";
    }
}
