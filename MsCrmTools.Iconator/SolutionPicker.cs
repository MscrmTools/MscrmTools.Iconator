﻿using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using MsCrmTools.Iconator.AppCode;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Windows.Forms;

namespace MsCrmTools.Iconator
{
    public partial class SolutionPicker : Form
    {
        private readonly IOrganizationService innerService;

        public SolutionPicker(IOrganizationService service, bool isForAddingComponents = false)
        {
            InitializeComponent();

            innerService = service;

            lstSolutions.MultiSelect = !isForAddingComponents;
            lblHeader.Text = isForAddingComponents ? "Select one solution" : "Select one or multiple solutions";
            lblDesc.Text = isForAddingComponents ? "Select the solution where you want to add the images" : "Tables and Images from these solutions will be loaded";
        }

        public List<Entity> SelectedSolutions { get; set; }

        private void btnSolutionPickerCancel_Click(object sender, EventArgs e)
        {
            SelectedSolutions = null;
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnSolutionPickerValidate_Click(object sender, EventArgs e)
        {
            if (lstSolutions.SelectedItems.Count > 0)
            {
                SelectedSolutions = lstSolutions.SelectedItems.Cast<ListViewItem>().Select(i => (Entity)i.Tag).ToList();
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show(this, "Please select a solution!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void lstSolutions_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            var list = (ListView)sender;
            list.Sorting = list.Sorting == SortOrder.Ascending ? SortOrder.Descending : SortOrder.Ascending;
            list.ListViewItemSorter = new ListViewItemComparer(e.Column, list.Sorting);
        }

        private void lstSolutions_DoubleClick(object sender, EventArgs e)
        {
            btnSolutionPickerValidate_Click(null, null);
        }

        private EntityCollection RetrieveSolutions()
        {
            try
            {
                QueryExpression qe = new QueryExpression("solution");
                qe.Distinct = true;
                qe.ColumnSet = new ColumnSet(true);
                qe.Criteria = new FilterExpression();
                qe.Criteria.AddCondition(new ConditionExpression("ismanaged", ConditionOperator.Equal, false));
                qe.Criteria.AddCondition(new ConditionExpression("isvisible", ConditionOperator.Equal, true));
                qe.Criteria.AddCondition(new ConditionExpression("uniquename", ConditionOperator.NotEqual, "Default"));
                qe.LinkEntities.Add(new LinkEntity
                {
                    LinkFromEntityName = "solution",
                    LinkFromAttributeName = "publisherid",
                    LinkToAttributeName = "publisherid",
                    LinkToEntityName = "publisher",
                    EntityAlias = "publisher",
                    Columns = new ColumnSet("customizationprefix")
                });

                return innerService.RetrieveMultiple(qe);
            }
            catch (Exception error)
            {
                if (error.InnerException != null && error.InnerException is FaultException)
                {
                    throw new Exception("Error while retrieving solutions: " + (error.InnerException).Message);
                }
                else
                {
                    throw new Exception("Error while retrieving solutions: " + error.Message);
                }
            }
        }

        private void SolutionPicker_Load(object sender, EventArgs e)
        {
            lstSolutions.Items.Clear();

            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += worker_DoWork;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            worker.RunWorkerAsync();
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            e.Result = RetrieveSolutions();
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            foreach (Entity solution in ((EntityCollection)e.Result).Entities)
            {
                ListViewItem item = new ListViewItem(solution["friendlyname"].ToString());
                item.SubItems.Add(solution["version"].ToString());
                item.SubItems.Add(((EntityReference)solution["publisherid"]).Name);
                item.Tag = solution;

                lstSolutions.Items.Add(item);
            }

            lstSolutions.Enabled = true;
            btnSolutionPickerValidate.Enabled = true;
        }
    }
}