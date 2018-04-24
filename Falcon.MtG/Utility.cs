namespace Falcon.MtG
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    public static class Utility
    {
        /// <summary>
        /// Matches any symbol on a card, including Mana Symbols, Tap Symbols, etc.
        /// </summary>
        public const string AnySymbolRegex = @"\{([WUBRGPTQCXS0-9])([\/])?([WUBRGP0-9])?\}";

        /// <summary>
        /// Matches any colored mana symbol on a card, including mono, split, and phyrexian. Colors
        /// will be in $1 and $3 replacement groups. $1 may include a P or 2 in the case of
        /// phyrexian mana or colorless split symbols.
        /// </summary>
        public const string ColoredManaSymbolRegex = @"\{([WUBRG2])([\/])?([WUBRGP])?\}";

        /// <summary>
        /// ///
        /// </summary>
        /// <param name="cardText"></param>
        /// <returns></returns>
        public static HashSet<string> GetColorIdentityFromText(string cardText)
        {
            var colors = new HashSet<string>();

            if (!string.IsNullOrEmpty(cardText))
            {
                MatchCollection manaSymbols = Regex.Matches(cardText, ColoredManaSymbolRegex);
                foreach (Match symbol in manaSymbols)
                {
                    if (symbol.Value.Contains("W"))
                    {
                        colors.Add("White");
                    }

                    if (symbol.Value.Contains("U"))
                    {
                        colors.Add("Blue");
                    }

                    if (symbol.Value.Contains("B"))
                    {
                        colors.Add("Black");
                    }

                    if (symbol.Value.Contains("R"))
                    {
                        colors.Add("Red");
                    }

                    if (symbol.Value.Contains("G"))
                    {
                        colors.Add("Green");
                    }
                }
            }

            return colors;
        }

        public static HashSet<string> GetColorIdentityFromTypes(IEnumerable<string> subtypes)
        {
            var colors = new HashSet<string>();

            if (subtypes.Contains("Plains"))
            {
                colors.Add("White");
            }

            if (subtypes.Contains("Island"))
            {
                colors.Add("Blue");
            }

            if (subtypes.Contains("Swamp"))
            {
                colors.Add("Black");
            }

            if (subtypes.Contains("Mountain"))
            {
                colors.Add("Red");
            }

            if (subtypes.Contains("Forest"))
            {
                colors.Add("Green");
            }

            return colors;
        }
    }
}