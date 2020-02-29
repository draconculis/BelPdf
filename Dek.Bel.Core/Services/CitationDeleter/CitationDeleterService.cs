using Dek.Cls;
using Dek.Bel.Core.Models;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Dek.DB;
using Dek.Bel.Core.GUI;

namespace Dek.Bel.Core.Services
{
    [Export]
    public class CitationDeleterService
    {
        [Import] public IDBService m_DBService { get; set; }
        [Import] public VolumeService m_Volumeservice { get; set; }
        [Import] public CitationService m_CitationService { get; set; }
        [Import] public IMessageboxService m_MessageboxService { get; set; }

        /// <summary>
        /// Delete a citation from the current volume.
        /// </summary>
        /// <returns></returns>
        public bool DeleteCitationById(Id id)
        {
            Id volumeId = m_Volumeservice.CurrentVolume.Id;
            if (volumeId.IsNull)
            {
                m_MessageboxService.Show("Must have a Volume selected to delete a citation.", "No current Volume");
                return false;
            }

            Citation cit = m_CitationService.GetCitation(volumeId, id);
            if (cit == null)
            {
                m_MessageboxService.Show($"Citation with id {id.ToStringShort()} was not found for Volume \"{m_Volumeservice.CurrentVolume.Title}\" with Id {volumeId}.", "Citation not found");
                return false;
            }


            var result = m_MessageboxService.ShowYesNo($"Do you want to delete citation \"{cit.Citation1.Left(50, true)}\" with id {id.ToStringShort()}?", "Delete citation");

            if (result != DekDialogResult.Yes)
                return false;

            m_CitationService.DeleteCitation(cit);
            return true;
        }

        public bool DeleteCitationsById(IEnumerable<Id> ids)
        {
            Id volumeId = m_Volumeservice.CurrentVolume.Id;
            if (volumeId.IsNull)
            {
                m_MessageboxService.Show("Must have a Volume selected to delete a citation.", "No current Volume");
                return false;
            }

            if (ids == null || !ids.Any())
            {
                m_MessageboxService.Show("Must have at least one citation selected in order to delete citations.", "No citations selected");
                return false;
            }

            var result = m_MessageboxService.ShowYesNo($"Do you want to delete {ids.Count()} citations?", "Delete citations");

            if (result != DekDialogResult.Yes)
                return false;

            foreach(Id id in ids)
                m_CitationService.DeleteCitationById(volumeId, id);

            return true;
        }
    }
}
