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
    public interface IEditionTOCService : IBaseService<EditionTOC>
    {
        Task<long> Insert(Guid createdBy, EditionTOC editionTOC);
        Task<long> Update(Guid createdBy, EditionTOC editionTOC);
        Task<long> Delete(Guid createdBy, int editionTOCId);
        Task<long> UpdateIndex(Guid createdBy, List<SortEditionTOCInexModel> sortingModel);
        Task<IEnumerable<EditionTOC>> FindByEdition(int editionId, Status status);
        Task<EditionTOC> FindById(int editionTOCId);
    }
}
