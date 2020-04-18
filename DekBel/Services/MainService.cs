using BelManagedLib;
using Dek.Cls;
using Dek.Bel.Core.Models;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Dek.DB;
using Dek.Bel.Core.Services;
using Dek.Bel.Core.Services.Toaster;
using Dek.Bel.Core.DB;
using System;

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
        [Import] public IToasterService m_Toaster { get; set; }

        public MainService()
        {
            if (m_DBService == null)
                Mef.Initialize(this, new List<Type> { GetType(), typeof(StorageRepo)});
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

            if (message.Code == (int)InterOp.CodesEnum.DEKBELCODE_ADDCITATIONSILENT)
            {
                List<RawCitation> rawCitations = m_CitationService.GetRawCitations(history.VolumeId).ToList();
                Citation cit = m_CitationService.CreateNewCitation(rawCitations, message, history.VolumeId);
                m_DBService.DeleteAll<RawCitation>();
                m_DBService.InsertOrUpdate(cit);

                m_Toaster.ShowToast("Citation added", cit.Citation1);

                return true;
            }

            return false;
        }

        public RawCitation AddRawCitations(EventData message)
        {
            m_Toaster.ShowToast("Fragment added", message.Text);

            return m_CitationService.AddRawCitations(message);
        }
    }
}

