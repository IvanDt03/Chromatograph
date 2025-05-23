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
            return LoadResult<List<Polymer>>.Failure($"Ошибка при считвании имен листов в Excel: {ex.Message}");
        }
    }

    public LoadResult<List<DataPoint>> LoadPolymerData(string namePolymer)
    {
        try
        {
            var result = new List<DataPoint>();
            using XLWorkbook wb = new XLWorkbook(_pathFile);

            var ws = wb.Worksheet(namePolymer);

            var row = ws.FirstRowUsed().RowBelow();
            
            while (row.Cell(1).IsEmpty() && row.Cell(2).IsEmpty())
            {
                var volume = row.Cell(1).GetDouble();
                var signal = row.Cell(2).GetDouble();

                result.Add(new DataPoint(volume, signal));
            }

            return LoadResult<List<DataPoint>>.Success(result);
        }
        catch (Exception ex)
        {
            return LoadResult<List<DataPoint>>.Failure($"Ошибка загрузи данных полимера {namePolymer}: {ex.Message}");
        }
    }
}
