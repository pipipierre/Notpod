﻿/*
 * Created by SharpDevelop.
 * User: Jaran
 * Date: 26.07.2011
 * Time: 15:12
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using iTunesAgent.UI.Components.Wizard;
using iTunesAgent.Domain;
using iTunesAgent.UI.Properties;
using iTunesAgent.Connectors;
using iTunesAgent.Connectors.Domain;

namespace iTunesAgent.UI.Controls
{
    /// <summary>
    /// Description of PlaylistAssociationBrowseFolder.
    /// </summary>
    public partial class PlaylistAssociationSettings : AbstractWizardPage
    {
        
        private IDevicesManager devicesManager;
        
        
        public PlaylistAssociationSettings()
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();
            
            //
            // TODO: Add constructor code after the InitializeComponent() call.
            //
        }
        
        
        public override void Populate()
        {
            ResetSelectedPath();
            
        }
        
        private void ResetSelectedPath()
        {
            // We ensure the selected path is reset.
            this.DataStore[WizardDataStoreKeys.PLAYLIST_ASSOCIATION_SELECTEDPATH] = null;
            lblSelectedDestination.Text = "";
        }
        
        public override bool ValidateBeforeNext()
        {
            return true;
        }
        
        void BtnBrowseClick(object sender, EventArgs e)
        {
            Device selectedDevice = (Device)DataStore[WizardDataStoreKeys.PLAYLIST_ASSOCIATION_SELECTEDDEVICE];
            
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
            
            folderBrowser.RootFolder = Environment.SpecialFolder.MyComputer;
            
            // TODO Need to set root folder of the browser to the root of the selected device.
            
            DialogResult result = folderBrowser.ShowDialog(this);
            if(result == DialogResult.OK) {
                
                
                CompatibleDevice device = devicesManager.GetConnectedDevice(selectedDevice.Identifier);
                bool folderExistsOnDevice = device.CheckPath(folderBrowser.SelectedPath);
                
                if(!folderExistsOnDevice)
                {
                    
                    MessageBox.Show(this, String.Format("The folder you selected ({0}) was not found on the device '{1}'. Are you sure you browsed the correct device?",
                                                        folderBrowser.SelectedPath, selectedDevice.Name), "Unable to verify folder location",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                    
                }
                
                this.DataStore[WizardDataStoreKeys.PLAYLIST_ASSOCIATION_SELECTEDPATH] = folderBrowser.SelectedPath;
                lblSelectedDestination.Text = String.Format("{0}:\n{1}", Resources.StrPlaylistAssociationCurrentlySelected, folderBrowser.SelectedPath);
                
            }
            
        }
        
        public IDevicesManager DevicesManager {
            get { return devicesManager; }
            set { devicesManager = value; }
        }

    }
}
