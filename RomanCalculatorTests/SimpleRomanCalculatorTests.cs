using System;
using NUnit.Framework;
using RomanCalculator.Impl;

namespace RomanCalculator.Tests
{
	public class Tests
	{
		private SimpleRomanCalculator _sut;

		[SetUp]
		public void Setup()
		{
			_sut = new SimpleRomanCalculator();
		}

		[TestCase("III", "II", ExpectedResult = "V")]
		[TestCase("II", "II", ExpectedResult = "IV")]
		[TestCase("VI", "III", ExpectedResult = "IX")]
		[TestCase("I", "V", ExpectedResult = "VI")]
		[TestCase("XX", "II", ExpectedResult = "XXII")]
		[TestCase("CC", "CC", ExpectedResult = "CD")]
		[TestCase("XX", "XX", ExpectedResult = "XL")]
		[TestCase("D", "D", ExpectedResult = "M")]
		[TestCase("XIV", "LX", ExpectedResult = "LXXIV")]
		[TestCase("CXXIII", "LXIX", ExpectedResult = "CXCII")]
		public string Add_TwoRomanNumbers_CorrectResult(string numberOne, string numberTwo)
		{
			return _sut.Add(numberOne, numberTwo);
		}

		[TestCase(null)]
		[TestCase("")]
		public void Add_FirstNumberEmptyString_ThrowsException(string firstNumber)
		{
			Assert.Throws(Is.TypeOf<ArgumentException>()
					.And.Message.EqualTo("First number is an empty string"),
				delegate { _sut.Add(firstNumber, "I"); });
		}

		[TestCase(null)]
		[TestCase("")]
		public void Add_SecondNumberEmptyString_ThrowsException(string secondNumber)
		{
			Assert.Throws(Is.TypeOf<ArgumentException>()
					.And.Message.EqualTo("Second number is an empty string"),
				delegate { _sut.Add("I", secondNumber); });
		}

		[TestCase("I")] 
		[TestCase("X")] 
		[TestCase("XIII")] //ugh, go on with it... :)
		public void ValidateRomanNumber_ValidNumber_DoesntThrow(string validRomanNumeral)
		{
			Assert.DoesNotThrow(delegate { _sut.ValidateRomanNumber(validRomanNumeral); });
		}

		[TestCase("IIIII")]
		[TestCase("XIXIXIX")]
		[TestCase("MCMCMCMCMC")]
		[TestCase("")] //ugh, go on with it... :)
		public void ValidateRomanNumber_InvalidNumber_ThrowsException(string invalidRomanNumeral)
		{
			Assert.Throws(Is.TypeOf<ArgumentException>()
					.And.Message.EqualTo($"[{invalidRomanNumeral}] is not a valid roman number"),
				delegate { _sut.ValidateRomanNumber(invalidRomanNumeral); });
		}

		[TestCase("IIIII", ExpectedResult = "V")]
		[TestCase("IIIIIIIIII", ExpectedResult = "X")]
		[TestCase("VV", ExpectedResult = "X")]
		[TestCase("XVV", ExpectedResult = "XX")]
		[TestCase("XXXXX", ExpectedResult = "L")]
		[TestCase("LL", ExpectedResult = "C")]
		[TestCase("CCCCC", ExpectedResult = "D")]
		[TestCase("DD", ExpectedResult = "M")]
		[TestCase("DDDD", ExpectedResult = "MM")]
		[TestCase("DDDDCCCCCLLXXXXXVVIIIIII", ExpectedResult = "MMDCLXVI")]
		public string SumUp_ValidNumeralToSumUp_CorrectResults(string summableString)
		{
			return _sut.SumUp(summableString);
		}

		[TestCase("IVXLCDM", ExpectedResult = "MDCLXVI")]
		[TestCase("IIIVXXLCCDDM", ExpectedResult = "MDDCCLXXVIII")]
		public string SortLargeBySmall_UnsortedString_IsSortedCorrectly(string unsortedString)
		{
			return _sut.SortLargeBySmall(unsortedString);
		}

		[TestCase("XXIV", ExpectedResult = "XXIIII")]
		[TestCase("XIX", ExpectedResult = "XVIIII")]
		[TestCase("XL", ExpectedResult = "XXXX")]
		[TestCase("XC", ExpectedResult = "LXXXX")]
		[TestCase("CD", ExpectedResult = "CCCC")]
		[TestCase("CM", ExpectedResult = "DCCCC")]
		public string PrefixSubtractionsToSuffixAdditions_CorrectlyConverted(string legalRomanNumber)
		{
			return _sut.PrefixSubtractionsToSuffixAdditions(legalRomanNumber);

		}

		[TestCase("XXIIII", ExpectedResult = "XXIV")]
		[TestCase("XVIIII", ExpectedResult = "XIX")]
		[TestCase("XXXX", ExpectedResult = "XL")]
		[TestCase("LXXXX", ExpectedResult = "XC")]
		[TestCase("CCCC", ExpectedResult = "CD")]
		[TestCase("DCCCC", ExpectedResult = "CM")]
		public string SuffixAdditionsToPrefixSubtractions_CorrectlyConverted(string illegalRomanNumber)
		{
			return _sut.SuffixAdditionsToPrefixSubtractions(illegalRomanNumber);
		}
	}

}
