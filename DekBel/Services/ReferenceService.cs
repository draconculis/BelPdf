using BelManagedLib;
using Dek.Bel.Cls;
using Dek.Bel.DB;
using Dek.Bel.Models;
using Dek.Bel.ReferenceGui;
using Dek.Cls;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dek.Bel.Services
{

    /// <summary>
    /// Instantiated & called from outer space (eg Sumatra).
    /// Responsible for adding references.
    /// </summary>
    public class ReferenceService : IDisposable
    {
        [Import] public IDBService m_DBService { get; set; }
        [Import] public HistoryRepo m_HistoryRepo { get; set; }
        [Import] public IMessageboxService m_MessageBoxService { get; set; }
        private History LastHistory;

        public ReferenceService()
        {
            if (m_DBService == null)
                Mef.Initialize(this);

            LastHistory = m_HistoryRepo.GetLastOpened(); // Our currently open file in Sumatra

        }

        public Page AddPage(EventData message)
        {
            // Need to call in order to not mess up stack
            var dummy = ArrayStuff.ExtractArrayFromIntPtr(message.SelectionRects, 1);

            int decodedPage = message.StartPage;
            if (!string.IsNullOrEmpty(message.Text))
                try
                {
                    string allowed = "0123456789";
                    string clean = "";
                    foreach (char c in message.Text)
                    {
                        if (allowed.Contains(c.ToString()))
                            clean += c;
                    }
                    decodedPage = int.Parse(clean);
                }
                catch { }

            bool valid = true;
            string formValue = decodedPage.ToString();
            do
            {
                var form = new Form_AddReference($"{(valid? "" : "[Invalid] ")}Set page number", formValue);
                if (form.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                    return null;

                try
                {
                    decodedPage = int.Parse(form.Value.Trim());
                    valid = true;
                }
                catch {
                    valid = false;
                    formValue = form.Value;
                }

            } while (!valid);

            m_DBService.Delete<Page>($"`PhysicalPage`={message.StartPage}");// There can be only one (per page)
            Page page = new Page
            {
                Id = Id.NewId(),
                VolumeId = LastHistory.VolumeId,
                PhysicalPage = message.StartPage,
                Glyph = -1,
                Title = message.Text,
                PaginationStart = decodedPage
            };

            m_DBService.InsertOrUpdate(page);

            return page;
        }


        /// <summary>
        /// The odd one out, belongs to volume, not ref. But here we go...
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        internal string EditVolumeTitle(EventData message)
        {
            // Need to call in order to not mess up stack
            var dummy = ArrayStuff.ExtractArrayFromIntPtr(message.SelectionRects, 1);

            var volume = m_DBService.SelectById<Volume>(LastHistory.VolumeId);
            // If no volume, warn and exit
            if(volume == null)
            {
                m_MessageBoxService.Show($"No volume found for {message.FilePath}.", "Volume not found");
                return null;
            }

            string title = message.Text.RemoveCRLF().RemoveDoubleSpace();
            if (string.IsNullOrWhiteSpace(title.Trim()))
                title = volume.Title;

            var form = new Form_AddReference($"Volume title", title);
            if (form.ShowDialog() == System.Windows.Forms.DialogResult.Cancel
                || string.IsNullOrWhiteSpace(form.Value))
                return null;

            string newTitle = form.Value;
            volume.Title = newTitle;
            m_DBService.InsertOrUpdate(volume);
            return newTitle;
        }

        public string AddReference<T>(EventData message) where T : Reference, new()
        {
            // Need to call in order to not mess up stack
            var dummy = ArrayStuff.ExtractArrayFromIntPtr(message.SelectionRects, 1);

            var volume = m_DBService.SelectById<Volume>(LastHistory.VolumeId); // Current volume

            var form = new Form_AddReference($"Add {new T().GetType().Name} title", message.Text);
            if (form.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                return null;

            T reference = new T
            {
                Id = Id.NewId(),
                Title = form.Value,
                VolumeId = volume.Id,
                PhysicalPage = message.StartPage,
                Glyph = message.StartGlyph,
            };

            m_DBService.InsertOrUpdate(reference);

            return reference.Title;
        }


        public void Dispose()
        {
            LastHistory = null;
            m_DBService = null;
            m_HistoryRepo = null;
        }
    }
}
