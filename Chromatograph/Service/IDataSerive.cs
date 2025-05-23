using Chromatograph.Models;
using System.Collections.Generic;

namespace Chromatograph.Service;

public interface IDataSerive
{
    LoadResult<List<Polymer>> LoadPolymers();
    LoadResult<List<DataPoint>> LoadPolymerData(string namePolymer);
}
