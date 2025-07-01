using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Paup.Models;

namespace Paup.Ostalo;

public class KalkulatorPDF
{
    public byte[] Generate(Kalkulator model)
    {
        return Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(40);
                page.Size(PageSizes.A4);
                page.DefaultTextStyle(x => x.FontSize(12));

                page.Header()
                    .Text("Kalkulacija fasade")
                    .FontSize(22)
                    .Bold()
                    .AlignCenter();

                page.Content().Column(col =>
                {
                    col.Spacing(15);

                    col.Item().BorderBottom(1).PaddingBottom(10).Column(info =>
                    {
                        info.Spacing(5);
                        info.Item().AlignCenter().Text($"Materijal: {model.VrstaMaterijala}");
                        info.Item().AlignCenter().Text($"Debljina izolacije: {model.Debljina} cm");
                        info.Item().AlignCenter().Text($"Ukupna površina: {model.UkupnaPovrsina} m²");
                        info.Item().AlignCenter().Text($"Cijena po m²: {model.CijenaUkupnoPoM2} €");
                        info.Item().AlignCenter().Text($"Ukupna cijena: {model.UkupnaCijena} €");
                    });

                    col.Item().PaddingTop(10).Text("Zidovi").Bold().FontSize(14).Underline();
                    for (int i = 0; i < model.BrojZidovaLista.Count; i++)
                    {
                        var zid = model.BrojZidovaLista[i];
                        col.Item().Text($"Zid {i + 1}: {zid.Visina} m × {zid.Sirina} m").SemiBold();

                        if (zid.Praznine != null && zid.Praznine.Any())
                        {
                            foreach (var praznina in zid.Praznine)
                            {
                                col.Item().PaddingLeft(20).Text($"- Praznina ({praznina.Tip}): {praznina.Visina} m × {praznina.Sirina} m");
                            }
                        }
                    }

                    if (model.DodatniTroskovi.Any())
                    {
                        col.Item().PaddingTop(10).Text("Dodatni troškovi").Bold().FontSize(14).Underline();

                        foreach (var trosak in model.DodatniTroskovi)
                        {
                            col.Item().Text($"• {trosak.Naslov}: {trosak.Opis} - {trosak.Cijena} €");
                        }
                    }

                    col.Item().PaddingTop(30).Row(row =>
                    {
                        row.RelativeItem().Column(sig =>
                        {
                            sig.Item().BorderBottom(1).Height(1);
                            sig.Item().AlignCenter().Text("Potpis korisnika").FontSize(10).Italic();
                        });

                        row.ConstantItem(50);

                        row.RelativeItem().Column(sig =>
                        {
                            sig.Item().BorderBottom(1).Height(1);
                            sig.Item().AlignCenter().Text("Potpis izvođača").FontSize(10).Italic();
                        });
                    });
                });

                page.Footer()
                    .AlignCenter()
                    .Text($"Generirano: {DateTime.Now:dd.MM.yyyy HH:mm}")
                    .FontSize(10)
                    .Italic();
            });
        }).GeneratePdf();
    }
}
