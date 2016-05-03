﻿// <copyright>
// Copyright by the Spark Development Network
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
//
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

using Rock;
using Rock.Data;
using Rock.Model;
using Rock.Web.Cache;
using Rock.Web.UI.Controls;
using Rock.Attribute;
using System.Data.Entity;
using Rock.UniversalSearch;
using System.Reflection;
using Rock.UniversalSearch.IndexModels;

namespace RockWeb.Blocks.Core
{
    /// <summary>
    /// Template block for developers to use to start a new block.
    /// </summary>
    [DisplayName( "Universal Search Control Panel" )]
    [Category( "Core" )]
    [Description( "Block to configure Rock's universal search features." )]
    public partial class UniversalSearchControlPanel : Rock.Web.UI.RockBlock
    {
        #region Fields

        // used for private variables

        #endregion

        #region Properties

        // used for public / protected properties

        #endregion

        #region Base Control Methods

        //  overrides of the base RockBlock methods (i.e. OnInit, OnLoad)

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e );

            // this event gets fired after block settings are updated. it's nice to repaint the screen if these settings would alter it
            this.BlockUpdated += Block_BlockUpdated;
            this.AddConfigurationUpdateTrigger( upnlContent );

            gEntityList.RowDataBound += gEntityList_RowDataBound;
            gEntityList.DataKeyNames = new string[] { "Id" };

            mdEditEntityType.SaveClick += MdEditEntityType_SaveClick;
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load" /> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnLoad( EventArgs e )
        {
            base.OnLoad( e );

            if ( !Page.IsPostBack )
            {
                LoadIndexDetails();
                LoadEntities();
                
            }
        }

        #endregion

        #region Events

        // handlers called by the controls on your block

        /// <summary>
        /// Handles the BlockUpdated event of the control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Block_BlockUpdated( object sender, EventArgs e )
        {

        }

        /// <summary>
        /// Handles the SaveClick event of the MdEditEntityType control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        private void MdEditEntityType_SaveClick( object sender, EventArgs e )
        {
            using(RockContext rockContext = new RockContext() )
            {
                EntityTypeService entityTypeService = new EntityTypeService( rockContext );
                var entityType = entityTypeService.Get( hfIdValue.ValueAsInt() );

                entityType.IsIndexingEnabled = cbEnabledIndexing.Checked;

                rockContext.SaveChanges();

                // flush item from cache
                EntityTypeCache.Flush( entityType.Id );
            }

            mdEditEntityType.Hide();
            LoadEntities();
        }

        /// <summary>
        /// Handles the RowDataBound event of the GEntityList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewRowEventArgs"/> instance containing the event data.</param>
        private void gEntityList_RowDataBound( object sender, GridViewRowEventArgs e )
        {
            var dataItem = e.Row.DataItem;
            if ( dataItem != null )
            {
                var isIndexingEnabled = dataItem.GetPropertyValue( "IsIndexingEnabled" ).ToString().AsBoolean();

                if ( !isIndexingEnabled )
                {
                    var refreshCell = e.Row.Cells[2].Controls[0].Visible = false;
                    var bulkDownloadCell = e.Row.Cells[3].Controls[0].Visible = false;
                }
            }
        }

        /// <summary>
        /// Handles the RowSelected event of the gEntityList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RowEventArgs"/> instance containing the event data.</param>
        protected void gEntityList_RowSelected( object sender, RowEventArgs e )
        {
            var entityType = EntityTypeCache.Read( e.RowKeyId );

            hfIdValue.Value = e.RowKeyId.ToString();

            cbEnabledIndexing.Checked = entityType.IsIndexingEnabled;

            mdEditEntityType.Title = entityType.FriendlyName + " Configuration";

            mdEditEntityType.Show();
        }

        /// <summary>
        /// Handles the Click event of the gContentItemBulkLoad control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RowEventArgs"/> instance containing the event data.</param>
        protected void gContentItemBulkLoad_Click( object sender, RowEventArgs e )
        {
            var entityType = EntityTypeCache.Read( e.RowKeyId );
            var component = IndexContainer.GetActiveComponent();

            if ( component != null && component.IsConnected )
            {
                Type type = entityType.GetEntityType();

                if ( type != null )
                {
                    object classInstance = Activator.CreateInstance( type, null );
                    MethodInfo bulkItemsMethod = type.GetMethod( "BulkIndexItems" );

                    if ( classInstance != null && bulkItemsMethod != null )
                    {
                        var bulkItems = bulkItemsMethod.Invoke( classInstance, null ) as IEnumerable<IndexModelBase>;

                        foreach ( var bulkItem in bulkItems ) {
                            component.IndexDocument( entityType.Name, bulkItem );
                        }
                    }
                }
            }
        }

        #endregion

        #region Methods

        private void LoadIndexDetails()
        {
            bool searchEnabled = false;

            foreach ( var indexType in IndexContainer.Instance.Components )
            {
                var component = indexType.Value.Value;
                if ( component.IsActive )
                {
                    hlblEnabled.Text = component.EntityType.FriendlyName;
                    hlblEnabled.LabelType = LabelType.Success;
                    searchEnabled = true;

                    if (!component.IsConnected )
                    {
                        hlblEnabled.LabelType = LabelType.Warning;
                        nbMessages.NotificationBoxType = NotificationBoxType.Warning;
                        nbMessages.Text = string.Format( "Could not connect to the ElasticSearch server at {0}.", component.IndexLocation );
                    }

                    lIndexLocation.Text = component.IndexLocation;
                    lIndexName.Text = component.IndexName;

                    break;
                }
            }

            if ( !searchEnabled )
            {
                nbMessages.NotificationBoxType = NotificationBoxType.Warning;
                nbMessages.Text = "No universal search index components are currently enabled. You must enable a index component under <span class='navigation-tip'>Admin Tools &gt; System Settngs &gt; Universal Search Index Components</span>.";
            }
        }

        /// <summary>
        /// Loads the entities.
        /// </summary>
        private void LoadEntities()
        {
            using ( RockContext rockContext = new RockContext() ) {
                var entities = new EntityTypeService( rockContext ).Queryable().AsNoTracking().ToList();

                var indexableEntities = entities.Where( e => e.IsIndexingSupported == true ).ToList(); ;

                gEntityList.DataSource = indexableEntities;
                gEntityList.DataBind();
            }
        }

        #endregion

    }
}