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
    public interface IEditionSummaryService : IBaseService<EditionSummary>
    {
        Task<long> Insert(Guid createdBy, EditionSummary editionSummary);
        Task<long> Update(Guid createdBy, EditionSummary editionSummary);
        Task<EditionSummary> FindByEditionTOC(int editionTOCId);

        Task<IEnumerable<EditionSummary>> FindByEdition(int editionId);
    }
}
