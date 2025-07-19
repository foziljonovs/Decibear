using Decibear.Bot.Models;

namespace Decibear.Bot.Extensions;

public static class StyleExtension
{
    public static string UrlStyle(this Style style)
        => style switch
        { 
            Style.FunEmoji => "fun-emoji",
            Style.Avataaars => "avataaars",
            Style.Bottts => "bottts",
            Style.PixelArt => "pixel-art",
            _ => throw new Exception($"{style} topilmadi!")
        };
}
