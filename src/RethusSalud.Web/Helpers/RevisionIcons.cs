using Microsoft.AspNetCore.Html;

namespace RethusSalud.Web.Helpers;

public static class RevisionIcons
{
    public static IHtmlContent Icon(string name) => new HtmlString(name switch
    {
        "home" =>
            "<svg class=\"revision-icon\" viewBox=\"0 0 24 24\" fill=\"none\"><path d=\"M4 11.5 12 4l8 7.5\" stroke=\"currentColor\" stroke-width=\"1.8\" stroke-linecap=\"round\" stroke-linejoin=\"round\"/><path d=\"M6 10v9h12v-9\" stroke=\"currentColor\" stroke-width=\"1.8\" stroke-linecap=\"round\" stroke-linejoin=\"round\"/></svg>",
        "chevron" =>
            "<svg class=\"revision-icon\" viewBox=\"0 0 24 24\" fill=\"none\"><path d=\"m9 6 6 6-6 6\" stroke=\"currentColor\" stroke-width=\"1.8\" stroke-linecap=\"round\" stroke-linejoin=\"round\"/></svg>",
        "back" =>
            "<svg class=\"revision-icon\" viewBox=\"0 0 24 24\" fill=\"none\"><path d=\"m14 6-6 6 6 6\" stroke=\"currentColor\" stroke-width=\"1.8\" stroke-linecap=\"round\" stroke-linejoin=\"round\"/></svg>",
        "shield" =>
            "<svg class=\"revision-icon\" viewBox=\"0 0 24 24\" fill=\"none\"><path d=\"M12 3l7 3v6c0 4.4-3 7.6-7 9-4-1.4-7-4.6-7-9V6l7-3z\" stroke=\"currentColor\" stroke-width=\"1.8\" stroke-linejoin=\"round\"/></svg>",
        "calendar" =>
            "<svg class=\"revision-icon\" viewBox=\"0 0 24 24\" fill=\"none\"><rect x=\"4\" y=\"5\" width=\"16\" height=\"15\" rx=\"2\" stroke=\"currentColor\" stroke-width=\"1.8\"/><path d=\"M4 9.5h16M8 3v4M16 3v4\" stroke=\"currentColor\" stroke-width=\"1.8\" stroke-linecap=\"round\"/></svg>",
        "id" =>
            "<svg class=\"revision-icon\" viewBox=\"0 0 24 24\" fill=\"none\"><rect x=\"3\" y=\"5\" width=\"18\" height=\"14\" rx=\"2\" stroke=\"currentColor\" stroke-width=\"1.8\"/><circle cx=\"8.5\" cy=\"11\" r=\"1.8\" stroke=\"currentColor\" stroke-width=\"1.6\"/><path d=\"M6 15.5c.6-1.4 1.8-2 2.5-2s1.9.6 2.5 2M14 9.5h5M14 13h5M14 16h3\" stroke=\"currentColor\" stroke-width=\"1.6\" stroke-linecap=\"round\"/></svg>",
        "hash" =>
            "<svg class=\"revision-icon\" viewBox=\"0 0 24 24\" fill=\"none\"><path d=\"M9 4 7 20M17 4l-2 16M4 9h16M3 15h16\" stroke=\"currentColor\" stroke-width=\"1.8\" stroke-linecap=\"round\"/></svg>",
        "pin" =>
            "<svg class=\"revision-icon\" viewBox=\"0 0 24 24\" fill=\"none\"><path d=\"M12 21s7-6.4 7-11.5A7 7 0 0 0 5 9.5C5 14.6 12 21 12 21z\" stroke=\"currentColor\" stroke-width=\"1.8\" stroke-linejoin=\"round\"/><circle cx=\"12\" cy=\"9.5\" r=\"2.3\" stroke=\"currentColor\" stroke-width=\"1.6\"/></svg>",
        "person" =>
            "<svg class=\"revision-icon\" viewBox=\"0 0 24 24\" fill=\"none\"><circle cx=\"12\" cy=\"8\" r=\"3.4\" stroke=\"currentColor\" stroke-width=\"1.8\"/><path d=\"M5 20c0-3.6 3.1-6.5 7-6.5s7 2.9 7 6.5\" stroke=\"currentColor\" stroke-width=\"1.8\" stroke-linecap=\"round\"/></svg>",
        "phone" =>
            "<svg class=\"revision-icon\" viewBox=\"0 0 24 24\" fill=\"none\"><path d=\"M6 4h3l1.5 4-2 1.5a11 11 0 0 0 5 5l1.5-2 4 1.5v3c0 1-1 1.7-2 1.5-7-1.4-11.5-6-13-13-.2-1 .5-2 1-2z\" stroke=\"currentColor\" stroke-width=\"1.6\" stroke-linejoin=\"round\"/></svg>",
        "mail" =>
            "<svg class=\"revision-icon\" viewBox=\"0 0 24 24\" fill=\"none\"><rect x=\"3\" y=\"5\" width=\"18\" height=\"14\" rx=\"2\" stroke=\"currentColor\" stroke-width=\"1.8\"/><path d=\"m4 6.5 8 6.5 8-6.5\" stroke=\"currentColor\" stroke-width=\"1.8\" stroke-linecap=\"round\" stroke-linejoin=\"round\"/></svg>",
        "globe" =>
            "<svg class=\"revision-icon\" viewBox=\"0 0 24 24\" fill=\"none\"><circle cx=\"12\" cy=\"12\" r=\"8.5\" stroke=\"currentColor\" stroke-width=\"1.8\"/><path d=\"M3.5 12h17M12 3.5c2.4 2.3 3.7 5.3 3.7 8.5s-1.3 6.2-3.7 8.5c-2.4-2.3-3.7-5.3-3.7-8.5S9.6 5.8 12 3.5z\" stroke=\"currentColor\" stroke-width=\"1.6\"/></svg>",
        "eye" =>
            "<svg class=\"revision-icon\" viewBox=\"0 0 24 24\" fill=\"none\"><path d=\"M2 12s3.5-7 10-7 10 7 10 7-3.5 7-10 7-10-7-10-7z\" stroke=\"currentColor\" stroke-width=\"1.6\"/><circle cx=\"12\" cy=\"12\" r=\"3\" stroke=\"currentColor\" stroke-width=\"1.6\"/></svg>",
        "chat" =>
            "<svg class=\"revision-icon\" viewBox=\"0 0 24 24\" fill=\"none\"><path d=\"M4 5h16v11H9l-5 4V5z\" stroke=\"currentColor\" stroke-width=\"1.8\" stroke-linejoin=\"round\"/></svg>",
        "send" =>
            "<svg class=\"revision-icon\" viewBox=\"0 0 24 24\" fill=\"none\"><path d=\"M21 3 3 10.5l7 2.5 2.5 7L21 3z\" stroke=\"currentColor\" stroke-width=\"1.8\" stroke-linejoin=\"round\"/></svg>",
        "check" =>
            "<svg class=\"revision-icon\" viewBox=\"0 0 24 24\" fill=\"none\"><path d=\"M5 13l4 4L19 7\" stroke=\"currentColor\" stroke-width=\"2.2\" stroke-linecap=\"round\" stroke-linejoin=\"round\"/></svg>",
        "x" =>
            "<svg class=\"revision-icon\" viewBox=\"0 0 24 24\" fill=\"none\"><path d=\"M6 6l12 12M18 6 6 18\" stroke=\"currentColor\" stroke-width=\"2.2\" stroke-linecap=\"round\"/></svg>",
        "cap" =>
            "<svg class=\"revision-icon\" viewBox=\"0 0 24 24\" fill=\"none\"><path d=\"M12 5 2 9l10 4 10-4-10-4z\" stroke=\"currentColor\" stroke-width=\"1.8\" stroke-linejoin=\"round\"/><path d=\"M6 11v4.5c0 1.4 2.7 2.5 6 2.5s6-1.1 6-2.5V11\" stroke=\"currentColor\" stroke-width=\"1.8\" stroke-linecap=\"round\"/></svg>",
        "file" =>
            "<svg class=\"revision-icon\" viewBox=\"0 0 24 24\" fill=\"none\"><path d=\"M7 3h7l4 4v14H7z\" stroke=\"currentColor\" stroke-width=\"1.8\" stroke-linejoin=\"round\"/><path d=\"M14 3v4h4\" stroke=\"currentColor\" stroke-width=\"1.8\" stroke-linejoin=\"round\"/></svg>",
        "clock" =>
            "<svg class=\"revision-icon\" viewBox=\"0 0 24 24\" fill=\"none\"><circle cx=\"12\" cy=\"12\" r=\"8.5\" stroke=\"currentColor\" stroke-width=\"1.8\"/><path d=\"M12 7.5V12l3 2\" stroke=\"currentColor\" stroke-width=\"1.8\" stroke-linecap=\"round\" stroke-linejoin=\"round\"/></svg>",
        _ => ""
    });
}
