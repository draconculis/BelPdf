using BelManagedLib;
using Dek.Bel.Cls;
using Dek.Bel.DB;
using Dek.Bel.Models;
using Dek.Bel.Services.Toaster;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dek.Bel.Services
{
    // ---- Special service ----
    // Called & instantiated from interOp

    [Export]
    public class MainService
    {
        [Import] CitationService m_CitationService { get; set; }
        [Import] IDBService m_DBService { get; set; }
        [Import] public VolumeService m_VolumeService { get; set; }
        [Import] public HistoryRepo m_HistoryRepo { get; set; }

        public MainService()
        {
            if (m_DBService == null)
                Mef.Initialize(this);
        }

        /// <summary>
        /// Silent version of add citation
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool AddCitationSilent(EventData message)
        {
            History history = m_HistoryRepo.GetLastOpened(); // Our currently open file in Sumatra
            //m_VolumeService.LoadVolume(history.VolumeId);

            if (message.Code == (int)InterOp.CodesEnum.DEKBELCODE_ADDANDSHOWCITATION)
            {
                List<RawCitation> rawCitations = m_CitationService.GetRawCitations(history.VolumeId).ToList();
                Citation cit = m_CitationService.CreateNewCitation(rawCitations, message, m_VolumeService.CurrentVolume.Id);
                m_DBService.DeleteAll<RawCitation>();

                Toast toast = new Toast("Citation added", cit.Citation1);
                toast.Show();

                return true;
            }

            return false;
        }

        public RawCitation AddRawCitations(EventData message)
        {
            Toast toast = new Toast("Fragment added", message.Text);
            toast.Show();

            return m_CitationService.AddRawCitations(message);
        }
    }
}

