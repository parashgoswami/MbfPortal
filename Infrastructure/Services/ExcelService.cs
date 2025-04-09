using Application.Common.Interfaces;
using ClosedXML.Excel;

namespace Infrastucture.Services;

public class ExcelService : IExcelService
{
    public IEnumerable<T> Import<T>(Stream fileStream) where T : new()
    {
        using var workbook = new XLWorkbook(fileStream);
        var worksheet = workbook.Worksheets.First();
        var properties = typeof(T).GetProperties();
        var rows = worksheet.RowsUsed().Skip(1); // Skip header row

        var result = new List<T>();

        foreach (var row in rows)
        {
            var item = new T();
            foreach (var property in properties)
            {
                var cell = row.Cell(property.Name);
                if (cell != null && !cell.Value.IsBlank)
                {
                    var value = Convert.ChangeType(cell.Value, property.PropertyType);
                    property.SetValue(item, value);
                }
            }
            result.Add(item);
        }

        return result;
    }

    public byte[] Export<T>(IEnumerable<T> data)
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Sheet1");
        var properties = typeof(T).GetProperties();

        // Add header row
        for (int i = 0; i < properties.Length; i++)
        {
            worksheet.Cell(1, i + 1).Value = properties[i].Name;
        }

        // Add data rows
        var rowIndex = 2;
        foreach (var item in data)
        {
            for (int i = 0; i < properties.Length; i++)
            {
                var value = properties[i].GetValue(item);
                worksheet.Cell(rowIndex, i + 1).Value = XLCellValue.FromObject(value);
            }
            rowIndex++;
        }

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }
}

