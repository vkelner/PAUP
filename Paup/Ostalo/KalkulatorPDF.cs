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
                        .FontSize(20)
                        .Bold()
                        .AlignCenter();

                    page.Content().Column(col =>
                    {
                        col.Spacing(10);

                        col.Item().Text($"Materijal: {model.VrstaMaterijala}");
                        col.Item().Text($"Debljina izolacije: {model.Debljina} cm");
                        col.Item().Text($"Ukupna površina: {model.UkupnaPovrsina} m²");
                        col.Item().Text($"Cijena po m²: {model.CijenaUkupnoPoM2} €");
                        col.Item().Text($"Ukupna cijena: {model.UkupnaCijena} €");

                        col.Item().Text("Zidovi:").Bold();
                        for (int i = 0; i < model.BrojZidovaLista.Count; i++)
                        {
                            var zid = model.BrojZidovaLista[i];
                            col.Item().Text($"  • Zid {i + 1}: {zid.Visina} m × {zid.Sirina} m");

                            if (zid.Praznine != null && zid.Praznine.Any())
                            {
                                foreach (var praznina in zid.Praznine)
                                {
                                    col.Item().Text($"     - Praznina ({praznina.Tip}): {praznina.Visina} m × {praznina.Sirina} m");
                                }
                            }
                        }

                        if (model.DodatniTroskovi.Any())
                        {
                            col.Item().Text("Dodatni troškovi:").Bold();
                            foreach (var trosak in model.DodatniTroskovi)
                            {
                                col.Item().Text($"  • {trosak.Naslov}: {trosak.Opis} - {trosak.Cijena} €");
                            }
                        }
                    });

                    page.Footer()
                        .AlignCenter()
                        .Text($"Generirano: {DateTime.Now:dd.MM.yyyy HH:mm}");
                });
            }).GeneratePdf();
        }
    }