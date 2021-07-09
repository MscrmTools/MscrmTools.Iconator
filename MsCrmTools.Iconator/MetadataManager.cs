// PROJECT : MsCrmTools.Iconator
// This project was developed by Tanguy Touzard
// CODEPLEX: http://xrmtoolbox.codeplex.com
// BLOG: http://mscrmtools.blogspot.com

using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Metadata.Query;
using Microsoft.Xrm.Sdk.Query;
using MsCrmTools.Iconator.AppCode;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace MsCrmTools.Iconator
{
    public static class MetadataManager
    {
        public static void ApplyImagesToEntities(List<EntityImageMap> mappings, IOrganizationService service)
        {
            foreach (var mapping in mappings)
            {
                if (mapping.ImageSize == 16)
                {
                    mapping.Entity.IconSmallName = mapping.WebResourceName;
                }
                else if (mapping.ImageSize == 32)
                {
                    mapping.Entity.IconMediumName = mapping.WebResourceName;
                }
                else
                {
                    mapping.Entity.IconVectorName = mapping.WebResourceName;
                }

                var request = new UpdateEntityRequest { Entity = mapping.Entity };
                service.Execute(request);
            }

            string parameter = mappings.Aggregate(string.Empty, (current, mapping) => current + ("<entity>" + mapping.Entity.LogicalName + "</entity>"));

            string parameterXml = string.Format("<importexportxml ><entities>{0}</entities></importexportxml>",
                                                parameter);

            var publishRequest = new PublishXmlRequest { ParameterXml = parameterXml };
            service.Execute(publishRequest);
        }

        public static void CleanOldIcons(EntityMetadata emd, List<string> imageList, IOrganizationService service)
        {
            if (!string.IsNullOrEmpty(emd.IconSmallName) && !imageList.Contains(emd.IconSmallName)) imageList.Add(emd.IconSmallName);
            if (!string.IsNullOrEmpty(emd.IconMediumName) && !imageList.Contains(emd.IconMediumName)) imageList.Add(emd.IconMediumName);
            if (!string.IsNullOrEmpty(emd.IconLargeName) && !imageList.Contains(emd.IconLargeName)) imageList.Add(emd.IconLargeName);

            emd.IconSmallName = "";
            emd.IconMediumName = "";
            emd.IconLargeName = "";

            var request = new UpdateEntityRequest { Entity = emd };
            service.Execute(request);
        }

        /// <summary>
        /// Recupere la liste des EntityMetadata presente dans la CRM
        /// </summary>
        /// <param name="service">CRM Service</param>
        /// <param name="solutionIds"></param>
        /// <param name="majorVersion"></param>
        /// <returns>Liste des entites retrouvees</returns>
        public static List<EntityMetadata> GetEntitiesList(IOrganizationService service, List<Guid> solutionIds, int majorVersion, int minorVersion)
        {
            if (solutionIds.Count > 0)
            {
                var query = new QueryExpression("solutioncomponent")
                {
                    NoLock = true,
                    ColumnSet = new ColumnSet(true),
                    Criteria = new FilterExpression
                    {
                        Conditions =
                        {
                            new ConditionExpression("solutionid", ConditionOperator.In, solutionIds.ToArray()),
                            new ConditionExpression("componenttype", ConditionOperator.Equal, 1)
                        }
                    }
                };

                var components = service.RetrieveMultiple(query).Entities;

                var list = components.Select(component => component.GetAttributeValue<Guid>("objectid"))
                    .ToList();

                if (list.Count > 0)
                {
                    EntityQueryExpression entityQueryExpression = new EntityQueryExpression()
                    {
                        Criteria = new MetadataFilterExpression
                        {
                            Conditions =
                            {
                                new MetadataConditionExpression("MetadataId", MetadataConditionOperator.In,
                                    list.ToArray()),
                                new MetadataConditionExpression("IsCustomEntity", MetadataConditionOperator.Equals, true)
                            }
                        },
                        Properties = new MetadataPropertiesExpression
                        {
                            AllProperties = false,
                            PropertyNames =
                            {
                                "DisplayName",
                                "LogicalName",
                                "IconSmallName",
                                "IconMediumName"
                            }
                        }
                    };

                    if (majorVersion == 7 && minorVersion >= 1 || majorVersion > 7)
                    {
                        entityQueryExpression.Properties.PropertyNames.Add("EntityColor");
                    }

                    if (majorVersion >= 9)
                    {
                        entityQueryExpression.Properties.PropertyNames.Add("IconVectorName");
                    }

                    RetrieveMetadataChangesRequest retrieveMetadataChangesRequest = new RetrieveMetadataChangesRequest
                    {
                        Query = entityQueryExpression,
                        ClientVersionStamp = null
                    };

                    var response = (RetrieveMetadataChangesResponse)service.Execute(retrieveMetadataChangesRequest);

                    return response.EntityMetadata.ToList();
                }

                return new List<EntityMetadata>();
            }

            EntityQueryExpression entityQueryExpressionFull = new EntityQueryExpression()
            {
                Criteria = new MetadataFilterExpression
                {
                    Conditions =
                            {
                                new MetadataConditionExpression("IsCustomEntity", MetadataConditionOperator.Equals, true)
                            }
                },
                Properties = new MetadataPropertiesExpression
                {
                    AllProperties = false,
                    PropertyNames = { "DisplayName", "LogicalName", "IconSmallName", "IconMediumName" }
                }
            };

            if (majorVersion == 7 && minorVersion >= 1 || majorVersion > 7)
            {
                entityQueryExpressionFull.Properties.PropertyNames.Add("EntityColor");
            }
            if (majorVersion >= 9)
            {
                entityQueryExpressionFull.Properties.PropertyNames.Add("IconVectorName");
            }

            RetrieveMetadataChangesRequest request = new RetrieveMetadataChangesRequest
            {
                Query = entityQueryExpressionFull,
                ClientVersionStamp = null
            };

            var fullResponse = (RetrieveMetadataChangesResponse)service.Execute(request);

            return fullResponse.EntityMetadata.Where(e => e.DisplayName?.UserLocalizedLabel != null).ToList();
        }

        public static void ResetIcons(EntityMetadata emd, IOrganizationService service)
        {
            emd.IconSmallName = "";
            emd.IconMediumName = "";
            emd.IconLargeName = "";
            emd.IconVectorName = "";
            var request = new UpdateEntityRequest { Entity = emd };
            service.Execute(request);
        }

        internal static void AddColor(EntityMetadata emd, Color color, IOrganizationService service)
        {
            var sColor = string.Format("#{0}{1}{2}", color.R.ToString("X").PadLeft(2, '0'), color.G.ToString("X").PadLeft(2, '0'), color.B.ToString("X").PadLeft(2, '0'));

            emd.EntityColor = sColor;

            var request = new UpdateEntityRequest { Entity = emd };
            service.Execute(request);
        }

        internal static void PublishEntities(List<string> entities, IOrganizationService service)
        {
            string parameterXml = string.Format("<importexportxml><entities>{0}</entities></importexportxml>",
                                                string.Join("", entities.Select(e => "<entity>" + e + "</entity>")));

            var publishRequest = new PublishXmlRequest { ParameterXml = parameterXml };
            service.Execute(publishRequest);
        }

        internal static void ResetColor(EntityMetadata emd, IOrganizationService service)
        {
            emd.EntityColor = null;

            var request = new UpdateEntityRequest { Entity = emd };
            service.Execute(request);
        }
    }
}