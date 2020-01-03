using Dek.Bel.DB;
using Dek.Bel.Models;
using Dek.Cls;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dek.Bel.Services.CitationDeleterService
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
                m_MessageboxService.Show("No current Volume", "Must have a Volume selected to delete a citation.");
                return false;
            }

            Citation cit = m_CitationService.GetCitation(volumeId, id);
            if (cit == null)
            {
                m_MessageboxService.Show("Citation not found", $"Citation with id {id} was not found for Volume \"{m_Volumeservice.CurrentVolume.Title}\" with Id {volumeId}.");
                return false;
            }


            var result = m_MessageboxService.ShowYesNo("Delete citation", $"Do you want to delete citation \"{cit.Citation1.Left(50, true)}\" with id {id}?");

            if (result != System.Windows.Forms.DialogResult.Yes)
                return false;

            m_CitationService.DeleteCitation(cit);
            return true;
        }

        public bool DeleteCitationsById(IEnumerable<Id> ids)
        {
            Id volumeId = m_Volumeservice.CurrentVolume.Id;
            if (volumeId.IsNull)
            {
                m_MessageboxService.Show("No current Volume", "Must have a Volume selected to delete a citation.");
                return false;
            }

            if (ids == null || !ids.Any())
            {
                m_MessageboxService.Show("No citations selected", "Must have at least one citation selected in order to delete citations.");
                return false;
            }

            var result = m_MessageboxService.ShowYesNo("Delete citations", $"Do you want to delete {ids.Count()} citations?");

            if (result != System.Windows.Forms.DialogResult.Yes)
                return false;

            foreach(Id id in ids)
                m_CitationService.DeleteCitationById(volumeId, id);

            return true;
        }
    }
}
