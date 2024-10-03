using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Common.Configuration;
using Unity.Core.Model;
using Unity.Core.Model.Edition;

namespace Unity.Core.Interface.Service
{
    public interface IEditionService : IBaseService<Edition>
    {
        Task<long> Insert(Guid createdBy, Edition edition);
        Task<long> Update(Guid createdBy, Edition edition);
        Task<Edition> FindById(int editionId);
        Task<IEnumerable<Edition>> GetAll(EditionStatus status);
        Task<IEnumerable<Edition>> FindBy(string keyword, EditionStatus status, int beginRow, int numRows);
    }
}
