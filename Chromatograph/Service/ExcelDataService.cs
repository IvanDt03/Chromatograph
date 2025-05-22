using Chromatograph.Models;
using System;
using System.Collections.Generic;
using ClosedXML.Excel;

namespace Chromatograph.Service;

public class ExcelDataService : IDataSerive
{
    private string _pathFile;
    private Random _random;

    public ExcelDataService(string pathFile)
    {
        _pathFile = pathFile;
        _random = new Random();
    }

    public LoadResult<List<Polymer>> LoadPolymers()
    {
        try
        {
            var result = new List<Polymer>();
            using XLWorkbook wb = new XLWorkbook(_pathFile);

            foreach (var s in wb.Worksheets)
                result.Add(new Polymer(s.Name));

            return LoadResult<List<Polymer>>.Success(result);
        }
        catch(Exception ex)
        {
            return LoadResult<List<Polymer>>.Failure($"Ошибка при загрузке данных Excel: {ex.Message}");
        }
    }
}
