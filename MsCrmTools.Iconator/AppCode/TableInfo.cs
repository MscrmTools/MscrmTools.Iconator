using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.Drawing;

namespace MsCrmTools.Iconator.AppCode
{
    public class TableInfo
    {
        public TableInfo(EntityMetadata emd)
        {
            Entity = emd;

            WebResourceName = emd.IconVectorName;
        }

        public event EventHandler<EntityVectorIconChangeEventsArgs> OnStateChange;

        public EntityMetadata Entity { get; set; }
        public bool IsPending { get; set; }
        public bool IsVectorImageSet => !string.IsNullOrEmpty(Entity.IconVectorName);
        public Image PendingWebResourceImage { get; set; }
        public string PendingWebResourceName { get; set; }
        public string WebResourceName { get; set; }

        public void ApplyPendingWebResource(IOrganizationService service)
        {
            if (!IsPending)
            {
                throw new System.Exception($"Table {Entity.DisplayName?.UserLocalizedLabel?.Label ?? Entity.LogicalName} has no pending vector image to set");
            }

            Entity.IconVectorName = PendingWebResourceName;

            service.Execute(new UpdateEntityRequest
            {
                Entity = Entity
            });

            IsPending = false;
            WebResourceName = PendingWebResourceName;
            PendingWebResourceName = null;
            PendingWebResourceImage = null;

            OnStateChange?.Invoke(this, new EntityVectorIconChangeEventsArgs { Table = this });
        }

        public void ApplyWebResourceRemoval(IOrganizationService service)
        {
            if (string.IsNullOrEmpty(WebResourceName))
            {
                return;
            }

            Entity.IconVectorName = "";

            service.Execute(new UpdateEntityRequest
            {
                Entity = Entity
            });

            Entity.IconVectorName = null;
            IsPending = false;
            WebResourceName = null;
            PendingWebResourceName = null;
            PendingWebResourceImage = null;

            OnStateChange?.Invoke(this, new EntityVectorIconChangeEventsArgs { Table = this });
        }

        public void CancelPending()
        {
            PendingWebResourceName = null;
            IsPending = false;

            OnStateChange?.Invoke(this, new EntityVectorIconChangeEventsArgs { Table = this });
        }

        public void SetPendingVectorIcon(string webresourceName, Image image)
        {
            PendingWebResourceName = webresourceName;
            PendingWebResourceImage = image;
            IsPending = true;

            OnStateChange?.Invoke(this, new EntityVectorIconChangeEventsArgs { Table = this });
        }
    }
}