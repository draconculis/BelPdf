using System;
using System.ComponentModel.Composition;
using System.Windows.Forms;

namespace Dek.Bel.Services
{
    /// <summary>
    /// Takes casre of auto saving edits in richtextbox 3.
    /// Idea is to auto save the 
    /// </summary>
    [Export]
    public class CitationPersisterService
    {
        public delegate void EventHandler(object sender, TimeToSaveEventArgs e);
        public event EventHandler TimeToSave;

        public class TimeToSaveEventArgs : EventArgs
        {
            public Control TheControl {get;set;}
        }

        private int DEFAULT_TIMER = 500;

        RichTextBox m_Rtb = null;
        public RichTextBox Rtb {
            get => m_Rtb;
            set
            {
                m_Timer.Stop();
                if(m_Rtb != null && m_Rtb != value)
                {
                    m_Rtb.KeyDown -= M_Rtb_KeyDown;
                }
                m_Rtb = value;
                if (value != null)
                {
                    m_Rtb.KeyDown += M_Rtb_KeyDown;
                    m_Timer.Start();
                }
            }
        }

        private void M_Rtb_KeyDown(object sender, KeyEventArgs e)
        {
            // Reset timer
            Interval = DEFAULT_TIMER;
        }

        public int Interval
        {
            get => m_Timer.Interval;
            set
            {
                bool wasEnabled = m_Timer.Enabled;
                m_Timer.Stop();
                m_Timer.Interval = value;
                if (wasEnabled && m_Rtb != null)
                    m_Timer.Start();
            }
        }

        private Timer m_Timer = new Timer();

        [ImportingConstructor]
        public CitationPersisterService()
        {
            m_Timer.Interval = DEFAULT_TIMER;
            m_Timer.Tick += M_Timer_Tick;
        }

        private void M_Timer_Tick(object sender, EventArgs e)
        {
            if(m_Rtb != null && m_Rtb.ContainsFocus)
                TimeToSave?.Invoke(this, new TimeToSaveEventArgs { TheControl = m_Rtb });
        }

    }
}
