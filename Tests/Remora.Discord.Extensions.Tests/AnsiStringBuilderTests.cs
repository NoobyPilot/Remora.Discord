﻿//
//  AnsiStringBuilderTests.cs
//
//  Author:
//       Jarl Gullberg <jarl.gullberg@gmail.com>
//
//  Copyright (c) 2017 Jarl Gullberg
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Lesser General Public License for more details.
//
//  You should have received a copy of the GNU Lesser General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//

using System;
using Remora.Discord.Extensions.Formatting;
using Xunit;

namespace Remora.Discord.Extensions.Tests;

/// <summary>
/// Tests to ensure the <see cref="AnsiStringBuilder"/> build a correct ANSI formatted string.
/// </summary>
public class AnsiStringBuilderTests
{
    private const char AnsiEscapeChar = '\u001b';

    /// <summary>
    /// Tests to see if the <see cref="AnsiStringBuilder.Bold"/> method bolds the input text.
    /// </summary>
    /// <param name="data">The input text to format.</param>
    [Theory]
    [InlineData("Sample")]
    [InlineData("Remora.Discord")]
    public void BoldDefaultSuccess(string data)
    {
        var expected = $"{AnsiEscapeChar}[{AnsiStyle.Reset};{AnsiStyle.Bold}m{data}";
        var actual = new AnsiStringBuilder().Bold().Append(data).Build();
        Assert.Equal(expected, actual);
    }

    /// <summary>
    /// Tests to see if the <see cref="AnsiStringBuilder.Bold"/> method bolds the first n characters.
    /// </summary>
    /// <param name="data">The input text to format.</param>
    /// <param name="charCount">The n leading characeters that should be bold.</param>
    [Theory]
    [InlineData("Sample", 3)]
    [InlineData("Remora.Discord", 7)]
    public void BoldLeadingCharsSuccess(string data, int charCount)
    {
        var expected = $"{AnsiEscapeChar}[{AnsiStyle.Reset};{AnsiStyle.Bold}m{data.Remove(charCount)}" +
                       $"{AnsiEscapeChar}[{AnsiStyle.Reset}m{data.Substring(charCount)}";
        var actual = new AnsiStringBuilder().Bold().Append(data.Remove(charCount))
                                            .Bold(false).Append(data.Substring(charCount))
                                            .Build();
        Assert.Equal(expected, actual);
    }

    /// <summary>
    /// Tests to see if the <see cref="AnsiStringBuilder.Underline"/> method underlines the input text.
    /// </summary>
    /// <param name="data">The input text to format.</param>
    [Theory]
    [InlineData("Sample")]
    [InlineData("Remora.Discord")]
    public void UnderlineDefaultSuccess(string data)
    {
        var expected = $"{AnsiEscapeChar}[{AnsiStyle.Reset};{AnsiStyle.Underline}m{data}";
        var actual = new AnsiStringBuilder().Underline().Append(data).Build();
        Assert.Equal(expected, actual);
    }

    /// <summary>
    /// Tests to see if the <see cref="AnsiStringBuilder.Underline"/> method underlines the first n characters.
    /// </summary>
    /// <param name="data">The input text to format.</param>
    /// <param name="charCount">The n leading characeters that should be underlined.</param>
    [Theory]
    [InlineData("Sample", 3)]
    [InlineData("Remora.Discord", 7)]
    public void UnderlineLeadingCharsSuccess(string data, int charCount)
    {
        var expected = $"{AnsiEscapeChar}[{AnsiStyle.Reset};{AnsiStyle.Underline}m{data.Remove(charCount)}" +
                       $"{AnsiEscapeChar}[{AnsiStyle.Reset}m{data.Substring(charCount)}";
        var actual = new AnsiStringBuilder().Underline().Append(data.Remove(charCount))
                                            .Underline(false).Append(data.Substring(charCount))
                                            .Build();
        Assert.Equal(expected, actual);
    }

    /// <summary>
    /// Tests to see if the <see cref="AnsiStringBuilder.Foreground"/> method sets the foreground color for the input text.
    /// </summary>
    /// <param name="data">The input text to format.</param>
    [Theory]
    [InlineData("Sample")]
    [InlineData("Remora.Discord")]
    public void ForegroundDefaultSuccess(string data)
    {
        AnsiForegroundColor[] foregroundColors = Enum.GetValues<AnsiForegroundColor>();

        foreach (var foregroundColor in foregroundColors)
        {
            string expected;

            // Foreground color None has some special handling
            if (foregroundColor == AnsiForegroundColor.None)
            {
                expected = $"{data}";
            }
            else
            {
                expected = $"{AnsiEscapeChar}[{AnsiStyle.Reset};{(int)foregroundColor}m{data}";
            }

            var actual = new AnsiStringBuilder().Foreground(foregroundColor).Append(data).Build();
            Assert.Equal(expected, actual);
        }
    }

    /// <summary>
    /// Tests to see if the <see cref="AnsiStringBuilder.Foreground"/> method sets the foreground color for the first n characters.
    /// </summary>
    /// <param name="data">The input text to format.</param>
    /// <param name="charCount">The n leading characeters that should have a foreground color.</param>
    [Theory]
    [InlineData("Sample", 3)]
    [InlineData("Remora.Discord", 7)]
    public void ForegroundLeadingCharsSuccess(string data, int charCount)
    {
        AnsiForegroundColor[] foregroundColors = Enum.GetValues<AnsiForegroundColor>();

        foreach (var foregroundColor in foregroundColors)
        {
            string expected;

            // Foreground color None has some special handling
            if (foregroundColor == AnsiForegroundColor.None)
            {
                expected = $"{data}";
            }
            else
            {
                expected = $"{AnsiEscapeChar}[{AnsiStyle.Reset};{(int)foregroundColor}m{data.Remove(charCount)}" +
                           $"{AnsiEscapeChar}[{AnsiStyle.Reset}m{data.Substring(charCount)}";
            }

            var actual = new AnsiStringBuilder().Foreground(foregroundColor).Append(data.Remove(charCount))
                                                .Foreground().Append(data.Substring(charCount))
                                                .Build();
            Assert.Equal(expected, actual);
        }
    }

    /// <summary>
    /// Tests to see if the <see cref="AnsiStringBuilder.Background"/> method sets the background color for the input text.
    /// </summary>
    /// <param name="data">The input text to format.</param>
    [Theory]
    [InlineData("Sample")]
    [InlineData("Remora.Discord")]
    public void BackgroundDefaultSuccess(string data)
    {
        AnsiBackgroundColor[] backgroundColors = Enum.GetValues<AnsiBackgroundColor>();

        foreach (var backgroundColor in backgroundColors)
        {
            string expected;

            // Background color None has some special handling
            if (backgroundColor == AnsiBackgroundColor.None)
            {
                expected = $"{data}";
            }
            else
            {
                expected = $"{AnsiEscapeChar}[{AnsiStyle.Reset};{(int)backgroundColor}m{data}";
            }

            var actual = new AnsiStringBuilder().Background(backgroundColor).Append(data).Build();
            Assert.Equal(expected, actual);
        }
    }

    /// <summary>
    /// Tests to see if the <see cref="AnsiStringBuilder.Background"/> method sets the background color for the first n characters.
    /// </summary>
    /// <param name="data">The input text to format.</param>
    /// <param name="charCount">The n leading characeters that should have a background color.</param>
    [Theory]
    [InlineData("Sample", 3)]
    [InlineData("Remora.Discord", 7)]
    public void BackgroundLeadingCharsSuccess(string data, int charCount)
    {
        AnsiBackgroundColor[] backgroundColors = Enum.GetValues<AnsiBackgroundColor>();

        foreach (var backgroundColor in backgroundColors)
        {
            string expected;

            // Foreground color None has some special handling
            if (backgroundColor == AnsiBackgroundColor.None)
            {
                expected = $"{data}";
            }
            else
            {
                expected = $"{AnsiEscapeChar}[{AnsiStyle.Reset};{(int)backgroundColor}m{data.Remove(charCount)}" +
                           $"{AnsiEscapeChar}[{AnsiStyle.Reset}m{data.Substring(charCount)}";
            }

            var actual = new AnsiStringBuilder().Background(backgroundColor).Append(data.Remove(charCount))
                                                .Background().Append(data.Substring(charCount))
                                                .Build();
            Assert.Equal(expected, actual);
        }
    }

    /// <summary>
    /// Tests to see if mixing <see cref="AnsiStringBuilder.Bold"/> and <see cref="AnsiStringBuilder.Underline"/> method format as expected.
    /// </summary>
    [Fact]
    public void MixBoldAndUnderlineSuccess()
    {
        var expected = "Does " +
                       $"{AnsiEscapeChar}[{AnsiStyle.Reset};{AnsiStyle.Bold}mbold{AnsiEscapeChar}[{AnsiStyle.Reset}m" +
                       " and " +
                       $"{AnsiEscapeChar}[{AnsiStyle.Reset};{AnsiStyle.Underline}munderline{AnsiEscapeChar}[{AnsiStyle.Reset}m" +
                       " " +
                       $"{AnsiEscapeChar}[{AnsiStyle.Reset};{AnsiStyle.Bold};{AnsiStyle.Underline}mmix{AnsiEscapeChar}[{AnsiStyle.Reset}m" +
                       "?";

        var actual = new AnsiStringBuilder().Append("Does ")
                                            .Bold().Append("bold").Bold(false)
                                            .Append(" and ")
                                            .Underline().Append("underline").Underline(false)
                                            .Append(" ")
                                            .Bold().Underline().Append("mix").Bold(false).Underline(false)
                                            .Append("?")
                                            .Build();

        Assert.Equal(expected, actual);
    }

    /// <summary>
    /// Tests to see if mixing <see cref="AnsiStringBuilder.Foreground"/> and <see cref="AnsiStringBuilder.Background"/> method format as expected.
    /// </summary>
    /// <param name="foregroundColor">The foreground color to use.</param>
    /// <param name="backgroundColor">The background color to use.</param>
    [Theory]
    [InlineData(AnsiForegroundColor.Red, AnsiBackgroundColor.Violet)]
    [InlineData(AnsiForegroundColor.Magenta, AnsiBackgroundColor.Base0)]
    [InlineData(AnsiForegroundColor.Cyan, AnsiBackgroundColor.Orange)]
    public void MixForegroundAndBackgroundSuccess(AnsiForegroundColor foregroundColor, AnsiBackgroundColor backgroundColor)
    {
        var expected = "Does " +
                       $"{AnsiEscapeChar}[{AnsiStyle.Reset};{(int)foregroundColor}mforeground{AnsiEscapeChar}[{AnsiStyle.Reset}m" +
                       " and " +
                       $"{AnsiEscapeChar}[{AnsiStyle.Reset};{(int)backgroundColor}mbackground{AnsiEscapeChar}[{AnsiStyle.Reset}m" +
                       " " +
                       $"{AnsiEscapeChar}[{AnsiStyle.Reset};{(int)backgroundColor};{(int)foregroundColor}mmix{AnsiEscapeChar}[{AnsiStyle.Reset}m" +
                       "?";

        var actual = new AnsiStringBuilder().Append("Does ")
                                            .Foreground(foregroundColor).Append("foreground").Foreground()
                                            .Append(" and ")
                                            .Background(backgroundColor).Append("background").Background()
                                            .Append(" ")
                                            .Foreground(foregroundColor).Background(backgroundColor).Append("mix").Foreground().Background()
                                            .Append("?")
                                            .Build();

        Assert.Equal(expected, actual);
    }

    /// <summary>
    /// Tests to see if mixing <see cref="AnsiStringBuilder.Bold"/> and <see cref="AnsiStringBuilder.Underline"/> and
    /// <see cref="AnsiStringBuilder.Foreground"/> and <see cref="AnsiStringBuilder.Background"/> method format as expected.
    /// </summary>
    /// <param name="foregroundColor">The foreground color to use.</param>
    /// <param name="backgroundColor">The background color to use.</param>
    [Theory]
    [InlineData(AnsiForegroundColor.Red, AnsiBackgroundColor.Violet)]
    [InlineData(AnsiForegroundColor.Magenta, AnsiBackgroundColor.Base0)]
    [InlineData(AnsiForegroundColor.Cyan, AnsiBackgroundColor.Orange)]
    public void MixAllSuccess(AnsiForegroundColor foregroundColor, AnsiBackgroundColor backgroundColor)
    {
        var expected = "Does " +
                       $"{AnsiEscapeChar}[{AnsiStyle.Reset};{AnsiStyle.Bold}mbold{AnsiEscapeChar}[{AnsiStyle.Reset}m" +
                       " and " +
                       $"{AnsiEscapeChar}[{AnsiStyle.Reset};{AnsiStyle.Underline}munderline{AnsiEscapeChar}[{AnsiStyle.Reset}m" +
                       " and " +
                       $"{AnsiEscapeChar}[{AnsiStyle.Reset};{(int)foregroundColor}mforeground{AnsiEscapeChar}[{AnsiStyle.Reset}m" +
                       " and " +
                       $"{AnsiEscapeChar}[{AnsiStyle.Reset};{(int)backgroundColor}mbackground{AnsiEscapeChar}[{AnsiStyle.Reset}m" +
                       " " +
                       $"{AnsiEscapeChar}[{AnsiStyle.Reset};{AnsiStyle.Bold};{AnsiStyle.Underline};{(int)backgroundColor};{(int)foregroundColor}m" +
                           "mix" +
                       $"{AnsiEscapeChar}[{AnsiStyle.Reset}m" +
                       "?";

        var actual = new AnsiStringBuilder().Append("Does ")
                                            .Bold().Append("bold").Bold(false)
                                            .Append(" and ")
                                            .Underline().Append("underline").Underline(false)
                                            .Append(" and ")
                                            .Foreground(foregroundColor).Append("foreground").Foreground()
                                            .Append(" and ")
                                            .Background(backgroundColor).Append("background").Background()
                                            .Append(" ")
                                            .Bold().Underline().Foreground(foregroundColor).Background(backgroundColor)
                                                .Append("mix")
                                            .Bold(false).Underline(false).Foreground().Background()
                                            .Append("?")
                                            .Build();

        Assert.Equal(expected, actual);
    }

    /// <summary>
    /// Tests to see if the <see cref="AnsiStringBuilder.Reset"/> method resets the styling.
    /// </summary>
    [Fact]
    public void ResetSuccess()
    {
        var expected = $"{AnsiEscapeChar}[{AnsiStyle.Reset};{AnsiStyle.Bold}mBold{AnsiEscapeChar}[{AnsiStyle.Reset}m" +
                       " and " +
                       $"{AnsiEscapeChar}[{AnsiStyle.Reset};{AnsiStyle.Underline}munderlined{AnsiEscapeChar}[{AnsiStyle.Reset}m" +
                       " " +
                       $"{AnsiEscapeChar}[{AnsiStyle.Reset};{AnsiStyle.Bold};{AnsiStyle.Underline}mcombined{AnsiEscapeChar}[{AnsiStyle.Reset}m" +
                       "!\n" +
                       $"{AnsiEscapeChar}[{AnsiStyle.Reset};{(int)AnsiForegroundColor.Red}mShould " +
                       $"{AnsiEscapeChar}[{AnsiStyle.Reset};{(int)AnsiBackgroundColor.Base0};{(int)AnsiForegroundColor.Red}mreset " +
                       $"{AnsiEscapeChar}[{AnsiStyle.Reset};{AnsiStyle.Underline};{(int)AnsiBackgroundColor.Base0};{(int)AnsiForegroundColor.Red}malso " +
                       $"{AnsiEscapeChar}[{AnsiStyle.Reset};{AnsiStyle.Bold};{AnsiStyle.Underline};{(int)AnsiBackgroundColor.Base0};{(int)AnsiForegroundColor.Red}mcolors" +
                       $"{AnsiEscapeChar}[{AnsiStyle.Reset}m!";

        var actual = new AnsiStringBuilder().Bold().Append("Bold").Reset()
                                            .Append(" and ")
                                            .Underline().Append("underlined").Reset()
                                            .Append(" ")
                                            .Bold().Underline().Append("combined").Reset()
                                            .AppendLine("!")
                                            .Foreground(AnsiForegroundColor.Red).Append("Should ")
                                            .Background(AnsiBackgroundColor.Base0).Append("reset ")
                                            .Underline().Append("also ")
                                            .Bold().Append("colors").Reset()
                                            .Append("!")
                                            .Build();

        Assert.Equal(expected, actual);
    }
}
