using OfficeOpenXml;
using OfficeOpenXml.Style;
using UniversityMonitoring.Data;
using UniversityMonitoring.Data.Models;

namespace UniversitiesMonitoring.Api.Services;

public static class ServiceExcelReportBuilder
{
    public static byte[] BuildExcel(UniversityService service, double? serviceUptime, int offset)
    {
        using var package = new ExcelPackage();
        var sheet = package.Workbook.Worksheets.Add("Отчет");
        var first20StateChanges = service.UniversityServiceStateChanges.OrderByDescending(x => x.ChangedAt)
            .Take(20).ToArray();

        var yEnd = first20StateChanges.Length >= 3 ? first20StateChanges.Length + 3 : 6;

        // Общая заливка
        var table = sheet.Cells[$"B2:C{yEnd}"];
        table.Style.Fill.PatternType = ExcelFillStyle.Solid;
        table.Style.Fill.BackgroundColor.SetColor(1, 255, 255, 255);

        // Обводка
        sheet.Cells["B2:C2"].Style.Border.Top.Style = ExcelBorderStyle.Medium;
        sheet.Cells[$"B2:B{yEnd}"].Style.Border.Left.Style = ExcelBorderStyle.Medium;
        sheet.Cells[$"B{yEnd}:C{yEnd}"].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
        sheet.Cells[$"C2:C{yEnd}"].Style.Border.Right.Style = ExcelBorderStyle.Medium;
        sheet.Cells["B3:C3"].Style.Border.Bottom.Style = ExcelBorderStyle.Thick;
        sheet.Cells[$"B4:B{yEnd}"].Style.Border.Right.Style = ExcelBorderStyle.Thick;

        sheet.Cells["B2:C2"].Merge = true;

        sheet.Cells["B2:C3"].Value = $"Изменения {service.University.Name}"; // Название ВУЗа
        sheet.Cells["B3"].Value = "Состояния"; // Заголовок для состояний
        sheet.Cells["C3"].Value = service.Name; // Имя сервиса
        sheet.Cells["C4"].Value = "Офлайн"; // Пример офлайна
        sheet.Cells["C5"].Value = "Онлайн"; // Пример онлайна

        sheet.Cells["C3"].Hyperlink = new Uri(service.GenerateUrl()); // Ссылка на сервис
        sheet.Cells["C3"].Style.Font.UnderLine = true; // Подчеркивание ссылки

        var head = sheet.Cells["B2:C3"];
        head.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        head.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        head.Style.Fill.BackgroundColor.SetColor(1, 7, 152, 234); // Цвет шапки таблицы
        head.Style.Font.Bold = true; // Делаем жирной таблицу
        head.Style.Font.Color.SetColor(1, 255, 255, 255);

        SetOnlineStyle(sheet.Cells["C4"]);
        SetOfflineStyle(sheet.Cells["C5"]);

        if (serviceUptime != null)
        {
            sheet.Cells["C6"].Value = $"Uptime: {Math.Round(serviceUptime.Value * 100, 0)}%";
            var r = (int) Math.Round(100 + 155 * (1 - serviceUptime.Value), 0);
            var g = (int) Math.Round(100 + 155 * serviceUptime.Value, 0);

            sheet.Cells["C6"].Style.Fill.BackgroundColor.SetColor(1, r, g, 100);
            sheet.Cells["C6"].Style.Font.Color.SetColor(1, (int) (r * .3), (int) (g * .3), 50);
        }

        for (var i = 0; i < first20StateChanges.Length; i++)
        {
            var ctxCell = sheet.Cells[$"B{i + 4}"];
            var ctxChange = first20StateChanges[i];

            ctxCell.Value = ctxChange.ChangedAt.AddMinutes(offset).ToString("dd.MM.yyyy HH:mm");
            if (ctxChange.IsOnline) SetOnlineStyle(ctxCell);
            else SetOfflineStyle(ctxCell);
        }

        sheet.Cells["B:C"].AutoFitColumns();
        sheet.Columns[3].Width = sheet.Columns[2].Width;

        GC.Collect(0);

        return package.GetAsByteArray();
    }

    private static void SetOfflineStyle(ExcelRange range)
    {
        range.Style.Font.Color.SetColor(1, 153, 0, 0);
        range.Style.Fill.BackgroundColor.SetColor(1, 255, 205, 205);
    }

    private static void SetOnlineStyle(ExcelRange range)
    {
        range.Style.Font.Color.SetColor(1, 0, 153, 0);
        range.Style.Fill.BackgroundColor.SetColor(1, 205, 255, 205);
    }
}