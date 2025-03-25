using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System.Drawing;
using System.Linq;

namespace MsCrmTools.Iconator.AppCode
{
    public class VectorImageInfo
    {
        public VectorImageInfo(string base64Content)
        {
            Image = ImageHelper.ConvertWebResContent(base64Content, true);
        }

        public Image Image { get; set; }
        public int Index { get; set; }
        public string Name { get; set; }

        public void Delete(IOrganizationService service)
        {
            var image = service.RetrieveMultiple(new QueryExpression("webresource")
            {
                Criteria = new FilterExpression
                {
                    Conditions =
                    {
                        new ConditionExpression("name", ConditionOperator.Equal, Name)
                    }
                }
            }).Entities.FirstOrDefault();

            if (image != null)
            {
                service.Delete(image.LogicalName, image.Id);
            }
        }
    }
}