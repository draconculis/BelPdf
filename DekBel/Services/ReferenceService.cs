using BelManagedLib;
using Dek.Bel.Cls;
using Dek.Bel.DB;
using Dek.Bel.Models;
using Dek.Bel.ReferenceGui;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dek.Bel.Services
{
    public class ReferenceService
    {
        [Import] public IDBService m_DBService { get; set; }

        ReferenceService()
        {
            if (m_DBService == null)
                Mef.Initialize(this);
        }

        void AddPage(EventData message)
        {
            var form = new Form_AddReference("Set page number", message.Text);
            if (form.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                return;

            Page page = new Page
            {

            };



        }
    }
}
