﻿// PROJECT : MsCrmTools.Iconator
// This project was developed by Tanguy Touzard
// CODEPLEX: http://xrmtoolbox.codeplex.com
// BLOG: http://mscrmtools.blogspot.com

using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using MsCrmTools.Iconator.AppCode;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Drawing;

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
                else
                {
                    mapping.Entity.IconMediumName = mapping.WebResourceName;
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

        /// <summary>
        /// Recupere la liste des EntityMetadata presente dans la CRM
        /// </summary>
        /// <param name="service">CRM Service</param>
        /// <returns>Liste des entites retrouvees</returns>
        public static List<EntityMetadata> GetEntitiesList(IOrganizationService service)
        {
            var req = new RetrieveAllEntitiesRequest
            {
                EntityFilters = EntityFilters.Entity
            };

            var resp = (RetrieveAllEntitiesResponse)service.Execute(req);
            //resp.EntityMetadata[0].icon

            return resp.EntityMetadata.Where(ent => ent.IsCustomEntity != null && (ent.DisplayName.UserLocalizedLabel != null && ent.IsCustomEntity.Value)).ToList();
        }

        public static void ResetIcons(EntityMetadata emd, IOrganizationService service)
        {
            emd.IconSmallName = "";
            emd.IconMediumName = "";
            emd.IconLargeName = "";
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
                                                string.Join("",entities.Select(e => "<entity>" + e + "</entity>")));

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