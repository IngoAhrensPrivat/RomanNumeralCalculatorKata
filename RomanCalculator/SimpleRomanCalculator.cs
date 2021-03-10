using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

namespace RomanCalculator.Impl
{
	public class SimpleRomanCalculator
	{
		/// <summary>
		/// Add roman numbers just like the old romans did - w/o translating them to real numbers & use real math!
		///
		/// I hope they knew Regular Expressions and Exceptions, but no mention of that in the kata, so I go for it all the way :)
		/// </summary>
		/// <param name="romanNumberOne">First roman number to add</param>
		/// <param name="romanNumberTwo">Second roman number to add to the first</param>
		/// <returns>Result of the addition of both numbers provided as another roman number</returns>
		public string Add(string romanNumberOne, string romanNumberTwo)
		{
			if (string.IsNullOrEmpty(romanNumberOne))
				throw new ArgumentException("First number is an empty string");
			if (string.IsNullOrEmpty(romanNumberTwo))
				throw new ArgumentException("Second number is an empty string");

			romanNumberOne = romanNumberOne.ToUpper();
			romanNumberTwo = romanNumberTwo.ToUpper();

			ValidateRomanNumber(romanNumberOne);
			ValidateRomanNumber(romanNumberTwo);

			var transformNumberOne = PrefixSubtractionsToSuffixAdditions(romanNumberOne);
			var transformNumberTwo = PrefixSubtractionsToSuffixAdditions(romanNumberTwo);

			var concatenated = transformNumberOne + transformNumberTwo;

			var sortedLargeBySmall = SortLargeBySmall(concatenated);

			var summedUp = SumUp(sortedLargeBySmall);

			var backToPrecedingSubstractions = SuffixAdditionsToPrefixSubtractions(summedUp);

			return backToPrecedingSubstractions;
		}

		internal void ValidateRomanNumber(string suspectedRomanNumber)
		{
			Regex regex = new Regex(@"^(?=[MDCLXVI])M*(C[MD]|D?C{0,3})(X[CL]|L?X{0,3})(I[XV]|V?I{0,3})$");
			Match match = regex.Match(suspectedRomanNumber);

			if (!match.Success)
				throw new ArgumentException($"[{suspectedRomanNumber}] is not a valid roman number");
		}

		internal string SumUp(string sortedLargeBySmall)
		{
			var result = sortedLargeBySmall;

			//Summing up what we have so far after sorting the string.
			//Low to high, to avoid any mishaps.

			while (result.Contains("IIIII"))
				result = result.Replace("IIIII", "V");

			while (result.Contains("VV"))
				result = result.Replace("VV", "X");

			while (result.Contains("XXXXX"))
				result = result.Replace("XXXXX", "L");

			while (result.Contains("LL"))
				result = result.Replace("LL", "C");

			while (result.Contains("CCCCC"))
				result = result.Replace("CCCCC", "D");

			while (result.Contains("DD"))
				result = result.Replace("DD", "M");

			return result;
		}

		internal string SortLargeBySmall(IEnumerable<char> concatenated)
		{
			var result = string.Empty;

			var romanHighToLow = new[] { 'M', 'D', 'C', 'L', 'X', 'V', 'I' };

			//We build a new string in order High to low from "roman perspective", not alphabetically,
			//each letter appearing in it's number of occurrences in correct order "legal" to the old roman bookkeepers...
			foreach (var romanLetter in romanHighToLow)
			{
				result += new string(romanLetter, concatenated.Count(x => x == romanLetter));
			}

			return result;
		}

		internal string PrefixSubtractionsToSuffixAdditions(string romanNumber)
		{
			// Preceding substractions get translated to additive suffixes
			// (illegal roman notation, but great for further processing!)
			// IX => VIIII (9)
			// XC => LXXXX (90)

			var result = romanNumber
				.Replace("IV", "IIII")
				.Replace("IX", "VIIII")
				.Replace("XL", "XXXX")
				.Replace("XC", "LXXXX")
				.Replace("CD", "CCCC")
				.Replace("CM", "DCCCC");
			return result;
		}

		internal string SuffixAdditionsToPrefixSubtractions(string romanNumber)
		{
			// Illegal succeeding substractions get translated to substracting prefixes again
			// VIIII => IX (9)
			// LXXXX => XC (90)
			// High to low, mind this!

			var result = romanNumber
					.Replace("DCCCC", "CM")
					.Replace("CCCC", "CD")
					.Replace("LXXXX", "XC")
					.Replace("XXXX", "XL")
					.Replace("VIIII", "IX")
					.Replace("IIII", "IV")
				;
			;
			return result;
		}

	}
}
