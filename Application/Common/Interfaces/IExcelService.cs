namespace Application.Common.Interfaces;

public interface IExcelService
{
    IEnumerable<T> Import<T>(Stream fileStream) where T : new();
    byte[] Export<T>(IEnumerable<T> data);
}
