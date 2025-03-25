// PROJECT : MsCrmTools.Iconator
// This project was developed by Tanguy Touzard
// CODEPLEX: http://xrmtoolbox.codeplex.com
// BLOG: http://mscrmtools.blogspot.com

using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace MsCrmTools.Iconator
{
    public static class WebResourcesManager
    {
        /// <summary>
        /// Recupere la liste des web resources d'un solution ciblee
        /// </summary>
        /// <param name="service">CRM Service</param>
        /// <param name="solutionIds">Identifiant unique de la solution à partir de laquelle retrouver les images</param>
        /// <returns>Liste des web resources retrouvees</returns>
        public static EntityCollection GetWebResourcesOnSolution(IOrganizationService service, List<Guid> solutionIds)
        {
            return GetWebResourcesOnSolution(service, solutionIds, new int[] { 5, 6, 7, 11 });
        }

        /// <summary>
        /// Recupere la liste des web resources d'un solution ciblee
        /// </summary>
        /// <param name="service">CRM Service</param>
        /// <param name="solutionIds">Identifiant unique de la solution à partir de laquelle retrouver les images</param>
        /// <returns>Liste des web resources retrouvees</returns>
        public static EntityCollection GetWebResourcesOnSolution(IOrganizationService service, List<Guid> solutionIds, int[] imageTypes)
        {
            if (solutionIds.Count == 0)
            {
                var queryWr = new QueryExpression
                {
                    EntityName = "webresource",
                    ColumnSet = new ColumnSet("name", "webresourcetype", "displayname", "content"),
                    Criteria = new FilterExpression
                    {
                        Conditions =
                        {
                            new ConditionExpression("webresourcetype", ConditionOperator.In, imageTypes),
                            new ConditionExpression("ishidden", ConditionOperator.Equal, false)
                        }
                    }
                };

                return service.RetrieveMultiple(queryWr);
            }

            var query = new QueryExpression("solutioncomponent")
            {
                NoLock = true,
                ColumnSet = new ColumnSet(true),
                Criteria = new FilterExpression
                {
                    Conditions =
                    {
                        new ConditionExpression("solutionid", ConditionOperator.In, solutionIds.ToArray()),
                        new ConditionExpression("componenttype", ConditionOperator.Equal, 61)
                    }
                }
            };

            var components = service.RetrieveMultiple(query).Entities;

            var list = components.Select(component => component.GetAttributeValue<Guid>("objectid")
                .ToString("B"))
                .ToList();

            if (list.Count > 0)
            {
                var qe = new QueryExpression("webresource")
                {
                    ColumnSet = new ColumnSet(true),
                    Criteria = new FilterExpression
                    {
                        Filters =
                        {
                            new FilterExpression
                            {
                                FilterOperator = LogicalOperator.And,
                                Conditions =
                                {
                                    new ConditionExpression("ishidden", ConditionOperator.Equal, false),
                                    new ConditionExpression("webresourceid", ConditionOperator.In, list.ToArray()),
                                    new ConditionExpression("webresourcetype", ConditionOperator.In, imageTypes)
                                }
                            },
                            new FilterExpression
                            {
                                FilterOperator = LogicalOperator.Or,
                                Conditions =
                                {
                                    new ConditionExpression("ismanaged", ConditionOperator.Equal, false),
                                    new ConditionExpression("iscustomizable", ConditionOperator.Equal, true),
                                }
                            }
                        }
                    },
                    Orders = { new OrderExpression("name", OrderType.Ascending) }
                };

                return service.RetrieveMultiple(qe);
            }

            return new EntityCollection();
        }

        /// <summary>
        /// Class representant l'objet web ressource
        /// </summary>
        public class WebResource
        {
            public string DisplayName { get; set; }
            public Guid WebResourceId { get; set; }
        }

        public class WebResourceAndImage
        {
            public Image Image { get; set; }
            public Entity Webresource { get; set; }
        };
    }
}