using System.ComponentModel;
using XrmToolBox.Extensibility;

namespace MsCrmTools.Iconator.AppCode
{
    public class Settings
    {
        [Category("Behavior")]
        [DisplayName("Apply image automatically")]
        [Description("Defines if the table must be updated as soon as the image has been dropped")]
        public bool ApplyImageAutomatically { get; set; }

        [Category("General")]
        [DisplayName("Use legacy iconator")]
        [Description("Defines if you want to use previous major version of Iconator. Requires to close and reopen the tool after having changed this setting")]
        public bool UseLegacyIconator { get; set; }

        [Category("Web resources management")]
        [DisplayName("Webresource name mask")]
        [Description("The mask to create the webresource name. {prefix} and {filename} are mandatories")]
        public string WebresourceNameMask { get; set; } = "{prefix}_/Images/Table/{filename}.svg";

        public void Save(string name = null)
        {
            if (WebresourceNameMask.IndexOf("{prefix}") < 0)
            {
                throw new System.Exception("Webresource name mask must contain {prefix} token");
            }

            if (WebresourceNameMask.IndexOf("{filename}") < 0)
            {
                throw new System.Exception("Webresource name mask must contain {filename} token");
            }

            SettingsManager.Instance.Save(typeof(Iconator), this);
        }
    }
}