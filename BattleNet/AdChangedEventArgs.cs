using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace BNSharp.BattleNet
{
    /// <summary>
    /// Contains information about a change in advertisements.
    /// </summary>
    [Serializable]
    [DataContract]
    public class AdChangedEventArgs : BaseEventArgs
    {
        [DataMember(Name = "AdID")]
        private int m_adID;
        [DataMember(Name = "FileTime")]
        private DateTime m_fileTime;
        [DataMember(Name = "Filename")]
        private string m_filename;
        [DataMember(Name = "LinkUrl")]
        private string m_linkUrl;

        /// <summary>
        /// Creates a new <see>AdChangedEventArgs</see>.
        /// </summary>
        /// <param name="adID">The unique ID of the ad.</param>
        /// <param name="fileTime">The local time of the file's most recent change.</param>
        /// <param name="fileName">The name of the file that contains the ad image.</param>
        /// <param name="linkUrl">The URL to which the ad links.</param>
        public AdChangedEventArgs(int adID, DateTime fileTime, string fileName, string linkUrl)
            : base()
        {
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentNullException("fileName");
            if (string.IsNullOrEmpty(linkUrl))
                throw new ArgumentNullException("linkUrl");

            m_adID = adID;
            m_fileTime = fileTime;
            m_filename = fileName;
            m_linkUrl = linkUrl;
        }

        /// <summary>
        /// Gets the unique ID of the ad.
        /// </summary>
        public int AdID
        {
            get { return m_adID; }
        }

        /// <summary>
        /// Gets the local time of the file's most recent change.
        /// </summary>
        public DateTime FileTime
        {
            get { return m_fileTime; }
        }

        /// <summary>
        /// Gets the name of the file that has the ad image.
        /// </summary>
        public string Filename
        {
            get { return m_filename; }
        }

        /// <summary>
        /// Gets the URL to the link to which the ad links.
        /// </summary>
        public string LinkUrl
        {
            get { return m_linkUrl; }
        }
    }

    /// <summary>
    /// Specifies the contract for handlers wishing to listen for AdChanged events.
    /// </summary>
    /// <param name="sender">The object that originated the event.</param>
    /// <param name="e">The event arguments.</param>
    public delegate void AdChangedEventHandler(object sender, AdChangedEventArgs e);
}
