using System.Globalization;
using FuchsControls;
using Xunit;

namespace FuchsControls.Tests;

public class ConverterTests
{
	[Theory]
	[InlineData("", null)]
	[InlineData(" ", null)]
	[InlineData("text", "text")]
	[InlineData(null, null)]
	public void EmptyStringToNullConverter_Tests(string? input, string? expected)
	{
		var converter = new EmptyStringToNullConverter();
		var result = converter.Convert(input, typeof(string), null, CultureInfo.InvariantCulture);
		Assert.Equal(expected, result);
	}

	[Theory]
	[InlineData("", false)]
	[InlineData("text", true)]
	[InlineData(null, false)]
	public void StringIsNotNullOrEmptyConverter_Tests(string? input, bool expected)
	{
		var converter = new StringIsNotNullOrEmptyConverter();
		var result = (bool)converter.Convert(input!, typeof(bool), null, CultureInfo.InvariantCulture);
		Assert.Equal(expected, result);
	}

	[Fact]
	public void HelpTextVisibilityConverter_Tests()
	{
		var converter = new HelpTextVisibilityConverter();
		
		// Visible and text present
		Assert.True((bool)converter.Convert([true, "Help"], typeof(bool), null, CultureInfo.InvariantCulture));
		
		// Visible but no text
		Assert.False((bool)converter.Convert([true, ""], typeof(bool), null, CultureInfo.InvariantCulture));
		Assert.False((bool)converter.Convert([true, null!], typeof(bool), null, CultureInfo.InvariantCulture));
		
		// Not visible
		Assert.False((bool)converter.Convert([false, "Help"], typeof(bool), null, CultureInfo.InvariantCulture));
	}

	[Theory]
	[InlineData(true, true)]
	[InlineData(false, false)]
	[InlineData(null, false)]
	public void NullableBoolToBoolConverter_Tests(bool? input, bool expected)
	{
		var converter = new NullableBoolToBoolConverter();
		var result = (bool)converter.Convert(input, typeof(bool), null, CultureInfo.InvariantCulture);
		Assert.Equal(expected, result);
	}
}