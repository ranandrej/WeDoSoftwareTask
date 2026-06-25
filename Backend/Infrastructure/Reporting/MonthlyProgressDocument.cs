using Application.DTOs;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Globalization;

namespace Infrastructure.Reporting;

public class MonthlyProgressDocument : IDocument
{
    private readonly MonthlyProgressDTO _data;
    private readonly string _userName;
    private readonly string _userSurename;

    public MonthlyProgressDocument(MonthlyProgressDTO data, string userName, string userSurename)
    {
        _data = data;
        _userName = userName;
        _userSurename = userSurename;
    }

    public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

    public void Compose(IDocumentContainer container)
    {
        var monthYear = new DateTime(_data.Year, _data.Month, 1)
            .ToString("MMMM yyyy", CultureInfo.InvariantCulture);

        container.Page(page =>
        {
            page.Margin(40);
            page.DefaultTextStyle(x => x.FontSize(11));

            page.Header().Column(col =>
            {
                col.Spacing(4);

                col.Item().Text($"{_userName} {_userSurename}")
                    .FontSize(22)
                    .Bold()
                    .FontColor(Colors.Blue.Darken3);

                col.Item().Text($"{monthYear} Report")
                    .FontSize(15)
                    .FontColor(Colors.Grey.Darken1);

                col.Item().PaddingTop(8).LineHorizontal(1).LineColor(Colors.Grey.Lighten2);
            });

            page.Content().PaddingTop(20).Column(col =>
            {
                col.Spacing(12);

                foreach (var week in _data.Weeks)
                {
                    col.Item().Element(c => ComposeWeekCard(c, week));
                }
            });

            page.Footer().AlignCenter().DefaultTextStyle(x => x.FontSize(9).FontColor(Colors.Grey.Medium)).Text(text =>
            {
                text.Span("Generated on ");
                text.Span(DateTime.UtcNow.ToString("dd MMM yyyy HH:mm", CultureInfo.InvariantCulture))
                    .SemiBold();
                text.Span(" UTC");
            });
        });
    }

    private static void ComposeWeekCard(IContainer container, WeeklyProgressDTO week)
    {
        container
            .Border(1)
            .BorderColor(Colors.Grey.Lighten2)
            .Background(Colors.Grey.Lighten5)
            .Padding(14)
            .Column(col =>
            {
                col.Spacing(6);

                col.Item().Text($"Week {week.WeekNumber}")
                    .FontSize(14)
                    .Bold()
                    .FontColor(Colors.Blue.Darken2);

                if (week.WorkoutCount == 0)
                {
                    col.Item().Text("No workouts recorded")
                        .FontColor(Colors.Grey.Medium)
                        .Italic();
                    return;
                }

                col.Item().Text($"Workouts: {week.WorkoutCount}");
                col.Item().Text($"Total duration: {week.TotalDurationMinutes} min");
                col.Item().Text($"Avg. difficulty: {week.AverageDifficulty:F2} / 10");
                col.Item().Text($"Avg. fatigue: {week.AverageFatigue:F2} / 10");
            });
    }
}
