﻿//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Rock.Model;

namespace Rock.Apps.CheckScannerUtility
{
    /// <summary>
    /// Interaction logic for BatchPage.xaml
    /// </summary>
    public partial class BatchPage : System.Windows.Controls.Page
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BatchPage"/> class.
        /// </summary>
        public BatchPage()
        {
            InitializeComponent();
            ScanningPage = new ScanningPage( this );
        }

        /// <summary>
        /// The binary file types
        /// </summary>
        public static List<BinaryFileType> BinaryFileTypes { get; set; }

        /// <summary>
        /// Gets or sets the type of the feeder.
        /// </summary>
        /// <value>
        /// The type of the feeder.
        /// </value>
        public FeederType ScannerFeederType { get; set; }

        /// <summary>
        /// The scanning page
        /// </summary>
        public ScanningPage ScanningPage { get; set; }

        #region Ranger (Canon CR50/80) Scanner Events

        /// <summary>
        /// Rangers the new state of the scanner_ transport.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void RangerScanner_TransportNewState( object sender, AxRANGERLib._DRangerEvents_TransportNewStateEvent e )
        {
            mnuConnect.IsEnabled = false;
            btnScan.Visibility = Visibility.Hidden;
            ScanningPage.btnDone.Visibility = Visibility.Visible;
            string status = RangerScanner.GetTransportStateString().Replace( "Transport", string.Empty ).SplitCase();
            shapeStatus.ToolTip = status;

            switch ( (XportStates)e.currentState )
            {
                case XportStates.TransportReadyToFeed:
                    shapeStatus.Fill = new SolidColorBrush( Colors.LimeGreen );
                    btnScan.Content = "Scan";
                    if ( ScannerFeederType.Equals( FeederType.MultipleItems ) )
                    {
                        ScanningPage.btnStartStop.Content = "Start";
                    }
                    else
                    {
                        ScanningPage.btnStartStop.Content = "Scan Check";
                    }

                    btnScan.Visibility = Visibility.Visible;
                    break;
                case XportStates.TransportShutDown:
                    shapeStatus.Fill = new SolidColorBrush( Colors.Red );
                    mnuConnect.IsEnabled = true;
                    break;
                case XportStates.TransportFeeding:
                    shapeStatus.Fill = new SolidColorBrush( Colors.Blue );
                    btnScan.Content = "Stop";
                    ScanningPage.btnStartStop.Content = "Stop";
                    ScanningPage.btnDone.Visibility = Visibility.Hidden;
                    btnScan.Visibility = Visibility.Visible;
                    break;
                case XportStates.TransportStartingUp:
                    shapeStatus.Fill = new SolidColorBrush( Colors.Yellow );
                    break;
                default:
                    shapeStatus.Fill = new SolidColorBrush( Colors.White );
                    break;
            }

            ScanningPage.shapeStatus.ToolTip = this.shapeStatus.ToolTip;
            ScanningPage.shapeStatus.Fill = this.shapeStatus.Fill;
        }

        /// <summary>
        /// Rangers the state of the scanner_ transport change options.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void RangerScanner_TransportChangeOptionsState( object sender, AxRANGERLib._DRangerEvents_TransportChangeOptionsStateEvent e )
        {
            if ( e.previousState == (int)XportStates.TransportStartingUp )
            {
                // enable imaging
                RangerScanner.SetGenericOption( "OptionalDevices", "NeedImaging", "True" );

                // limit splash screen
                RangerScanner.SetGenericOption( "Ranger GUI", "DisplaySplashOncePerDay", "true" );

                // turn on either color, grayscale, or bitonal options depending on selected option
                RangerScanner.SetGenericOption( "OptionalDevices", "NeedFrontImage1", "False" );
                RangerScanner.SetGenericOption( "OptionalDevices", "NeedRearImage1", "False" );
                RangerScanner.SetGenericOption( "OptionalDevices", "NeedFrontImage2", "False" );
                RangerScanner.SetGenericOption( "OptionalDevices", "NeedRearImage2", "False" );
                RangerScanner.SetGenericOption( "OptionalDevices", "NeedFrontImage3", "False" );
                RangerScanner.SetGenericOption( "OptionalDevices", "NeedRearImage3", "False" );
                RangerScanner.SetGenericOption( "OptionalDevices", "NeedFrontImage4", "False" );
                RangerScanner.SetGenericOption( "OptionalDevices", "NeedRearImage4", "False" );

                switch ( RockConfig.Load().ImageColorType )
                {
                    case ImageColorType.ImageColorTypeColor:
                        RangerScanner.SetGenericOption( "OptionalDevices", "NeedFrontImage3", "True" );
                        RangerScanner.SetGenericOption( "OptionalDevices", "NeedRearImage3", "True" );
                        break;
                    case ImageColorType.ImageColorTypeGrayscale:
                        RangerScanner.SetGenericOption( "OptionalDevices", "NeedFrontImage2", "True" );
                        RangerScanner.SetGenericOption( "OptionalDevices", "NeedRearImage2", "True" );
                        break;
                    default:
                        RangerScanner.SetGenericOption( "OptionalDevices", "NeedFrontImage1", "True" );
                        RangerScanner.SetGenericOption( "OptionalDevices", "NeedRearImage1", "True" );
                        break;
                }

                RangerScanner.EnableOptions();
            }
        }

        /// <summary>
        /// Rangers the scanner_ transport item in pocket.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void RangerScanner_TransportItemInPocket( object sender, AxRANGERLib._DRangerEvents_TransportItemInPocketEvent e )
        {
            string fileDirectory = GetScannerOutputDirectory();

            BitmapImage bitImageFront = GetCheckImage( Sides.TransportFront );
            BitmapImage bitImageBack = GetCheckImage( Sides.TransportRear );

            ScanningPage.imgFront.Source = bitImageFront;
            ScanningPage.imgBack.Source = bitImageBack;

            string checkMicr = RangerScanner.GetMicrText( 1 ).Replace( "-", string.Empty ).Replace( "!", string.Empty ).Trim();
            string fileName = checkMicr.Replace( " ", "_" );

            string[] micrParts = checkMicr.Split( new char[] { 'c', 'd', ' ' }, StringSplitOptions.RemoveEmptyEntries );
            string accountNumber = micrParts.Length > 0 ? micrParts[0] : "<not found>";
            string routingNumber = micrParts.Length > 1 ? micrParts[1] : "<not found>";
            string checkNumber = micrParts.Length > 2 ? micrParts[2] : "<not found>";

            ScanningPage.lblAccountNumber.Content = string.Format( "Account Number: {0}", accountNumber );
            ScanningPage.lblRoutingNumber.Content = string.Format( "Routing Number: {0}", routingNumber );
            ScanningPage.lblCheckNumber.Content = string.Format( "Check Number: {0}", checkNumber );

            string frontFilePath = System.IO.Path.Combine( fileDirectory, fileName + "_front.jpg" );
            File.Delete( frontFilePath );

            Bitmap bmpFront = new Bitmap( bitImageFront.StreamSource );
            bmpFront.Save( frontFilePath, System.Drawing.Imaging.ImageFormat.Jpeg );

            string backFilePath = System.IO.Path.Combine( fileDirectory, fileName + "_back.jpg" );
            File.Delete( backFilePath );
            Bitmap bmpBack = new Bitmap( bitImageBack.StreamSource );
            bmpBack.Save( backFilePath, System.Drawing.Imaging.ImageFormat.Jpeg );
        }

        #endregion

        #region Scanner (MagTek MICRImage RS232) Events

        /// <summary>
        /// Determines whether the specified value is integer.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///   <c>true</c> if the specified value is integer; otherwise, <c>false</c>.
        /// </returns>
        private bool IsInteger( string value )
        {
            int temp;
            return int.TryParse( value, out temp );
        }

        /// <summary>
        /// Handles the MicrDataReceived event of the micrImage control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void micrImage_MicrDataReceived( object sender, System.EventArgs e )
        {
            object dummy = null;

            string imagePath = string.Empty;
            string imageIndex = string.Empty;
            string statusMsg = string.Empty;

            // from MagTek Sample Code
            string accountNumber = micrImage.FindElement( 0, "TT", 0, "A", ref dummy );
            string routingNumber = micrImage.FindElement( 0, "T", 0, "TT", ref dummy );
            string checkNumber = micrImage.FindElement( 0, "A", 0, "12", ref dummy );

            // MagTek OCX only likes short paths
            imagePath = Path.GetTempPath();
            string checkImageFileName = Path.Combine( imagePath, string.Format( "check_front_{0}_{1}_{2}.tif", accountNumber, routingNumber, checkNumber ) );

            micrImage.TransmitCurrentImage( checkImageFileName, ref statusMsg );
            if ( !File.Exists( checkImageFileName ) )
            {
                throw new Exception( "Unable to retrieve image" );
            }

            micrImage.ClearBuffer();
        }

        #endregion

        #region Image Upload related

        /// <summary>
        /// Gets the scanner output directory.
        /// </summary>
        /// <returns></returns>
        private static string GetScannerOutputDirectory()
        {
            string fileDirectory = System.IO.Path.Combine( new FileInfo( System.Reflection.Assembly.GetExecutingAssembly().Location ).DirectoryName, "ScannerOutput" );

            if ( !Directory.Exists( fileDirectory ) )
            {
                Directory.CreateDirectory( fileDirectory );
            }

            return fileDirectory;
        }

        /// <summary>
        /// Gets the check image.
        /// </summary>
        /// <param name="side">The side.</param>
        /// <returns></returns>
        private BitmapImage GetCheckImage( Sides side )
        {
            ImageColorType colorType = RockConfig.Load().ImageColorType;

            int imageByteCount;
            imageByteCount = RangerScanner.GetImageByteCount( (int)side, (int)colorType );
            byte[] imageBytes = new byte[imageByteCount];

            // create the pointer and assign the Ranger image address to it
            IntPtr imgAddress = new IntPtr( RangerScanner.GetImageAddress( (int)side, (int)colorType ) );

            // Copy the bytes from unmanaged memory to managed memory
            Marshal.Copy( imgAddress, imageBytes, 0, imageByteCount );

            BitmapImage bitImageFront = new BitmapImage();

            bitImageFront.BeginInit();
            bitImageFront.StreamSource = new MemoryStream( imageBytes );
            bitImageFront.EndInit();

            return bitImageFront;
        }

        /// <summary>
        /// Handles the Click event of the btnUpload control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void btnUpload_Click( object sender, EventArgs e )
        {
            string rockUrl = RockConfig.Load().RockURL;

            getBinaryFileTypes( rockUrl ).ContinueWith( a =>
            {
                BinaryFileTypes = a.Result;
                UploadScannedChecks( rockUrl, ShowProgress );
            } );
        }

        /// <summary>
        /// Shows the progress.
        /// </summary>
        /// <param name="current">The current.</param>
        /// <param name="max">The max.</param>
        /// <param name="name">The name.</param>
        private void ShowProgress( int current, int max, string name )
        {
            //progressBar.Maximum = max;
            //progressBar.Value = current;
        }

        /// <summary>
        /// Gets the binary file types.
        /// </summary>
        /// <param name="rockBaseUrl">The rock base URL.</param>
        private static async System.Threading.Tasks.Task<List<BinaryFileType>> getBinaryFileTypes( string rockBaseUrl )
        {
            HttpClient client = new HttpClient();
            HttpContent resultContent;
            string restURL = rockBaseUrl.TrimEnd( new char[] { '/' } ) + "/api/BinaryFileTypes/";
            await client.GetAsync( restURL ).ContinueWith( ( postTask ) =>
                {
                    resultContent = postTask.Result.Content;
                    resultContent.ReadAsAsync<List<BinaryFileType>>().ContinueWith(
                        ( readResult ) =>
                        {
                            BinaryFileTypes = readResult.Result;
                        } ).Wait();
                } );

            return BinaryFileTypes;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="max">The max.</param>
        /// <param name="name">The name.</param>
        private delegate void ProgressUpdate( int position, int max, string name );

        /// <summary>
        /// Uploads the scanned checks.
        /// </summary>
        /// <param name="rockBaseUrl">The rock base URL.</param>
        private static async void UploadScannedChecks( string rockBaseUrl, ProgressUpdate progressFeedback )
        {
            string restURL = rockBaseUrl.TrimEnd( new char[] { '/' } ) + "/api/BinaryFiles/";
            var qryParams = new System.Collections.Generic.Dictionary<string, string>();
            restURL += "0?apikey=CcvRockApiKey";
            Guid fileTypeCheckFront = new Guid( "EF9B78C1-57A0-4D18-8275-51EECE0C8A6D" );
            Guid fileTypeCheckBack = new Guid( "DAC10DF2-D57F-45F6-94AD-8E27E3BC4682" );

            DirectoryInfo scannerOutputDirectory = new DirectoryInfo( GetScannerOutputDirectory() );
            var scannedFiles = scannerOutputDirectory.GetFiles( "*.jpg" ).ToList();

            int totalCount = scannedFiles.Count();
            int position = 1;

            foreach ( FileInfo scannedFile in scannedFiles )
            {
                var binaryFile = new BinaryFile();
                binaryFile.Id = 0;
                binaryFile.FileName = scannedFile.Name;
                binaryFile.Data = File.ReadAllBytes( scannedFile.FullName );
                if ( scannedFile.Name.EndsWith( "_front.jpg" ) )
                {
                    binaryFile.BinaryFileTypeId = BinaryFileTypes.First( a => a.Guid.Equals( fileTypeCheckFront ) ).Id;
                }
                else if ( scannedFile.Name.EndsWith( "_back.jpg" ) )
                {
                    binaryFile.BinaryFileTypeId = BinaryFileTypes.First( a => a.Guid.Equals( fileTypeCheckBack ) ).Id;
                }
                else
                {
                    continue;
                }

                binaryFile.IsSystem = false;
                binaryFile.MimeType = "image/jpeg";

                HttpClient client = new HttpClient();
                try
                {
                    await client.PostAsJsonAsync<BinaryFile>( restURL, binaryFile ).ContinueWith(
                        ( postTask ) =>
                        {
                            progressFeedback( position++, totalCount, scannedFile.Name );
                            postTask.Result.EnsureSuccessStatusCode();
                            scannedFile.Delete();
                        } );
                }
                catch ( Exception ex )
                {
                    MessageBox.Show( ex.Message );
                    break;
                }
            }

            progressFeedback( 0, 0, "Done" );
        }

        #endregion

        /// <summary>
        /// Handles the Loaded event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void Page_Loaded( object sender, RoutedEventArgs e )
        {
            ConnectToScanner();

            List<FinancialBatch> sampleData = new List<FinancialBatch>();
            sampleData.Add( new FinancialBatch { Name = "Sample Batch Name 1", IsClosed = false, BatchDate = DateTime.Now } );
            sampleData.Add( new FinancialBatch { Name = "Sample Batch Lonnnnng Name 2", IsClosed = false, BatchDate = DateTime.Now } );
            sampleData.Add( new FinancialBatch { Name = "Sample Batch Name 3", IsClosed = false, BatchDate = DateTime.Now } );

            grdBatches.DataContext = sampleData;
            if ( sampleData.Count > 0 )
            {
                grdBatches.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Connects to scanner.
        /// </summary>
        private void ConnectToScanner()
        {
            var rockConfig = RockConfig.Load();

            if ( rockConfig.ScannerInterfaceType == RockConfig.InterfaceType.MICRImageRS232 )
            {
                micrImage.CommPort = rockConfig.MICRImageComPort;

                object dummy = null;

                // converted from VB6 from MagTek's sample app
                if ( !micrImage.PortOpen )
                {
                    micrImage.PortOpen = true;
                    if ( micrImage.DSRHolding )
                    {
                        // Sets Switch Settings
                        // If you use the MicrImage1.Save command then these do not need to be sent
                        // every time you open the device
                        micrImage.MicrTimeOut = 1;
                        micrImage.MicrCommand( "SWA 00100010", ref dummy );
                        micrImage.MicrCommand( "SWB 00100010", ref dummy );
                        micrImage.MicrCommand( "SWC 00100000", ref dummy );
                        micrImage.MicrCommand( "HW 00111100", ref dummy );
                        micrImage.MicrCommand( "SWE 00000010", ref dummy );
                        micrImage.MicrCommand( "SWI 00000000", ref dummy );

                        // The OCX will work with any Micr Format.  You just need to know which
                        // format is being used to parse it using the FindElement Method
                        micrImage.FormatChange( "6200" );
                        micrImage.MicrTimeOut = 5;
                    }
                    else
                    {
                        MessageBox.Show( "A Check Scanner Device is not attached.", "Missing Scanner" );
                        return;
                    }
                }

                ScannerFeederType = FeederType.SingleItem;
            }
            else
            {
                RangerScanner.StartUp();
                string feederTypeName = RangerScanner.GetTransportInfo( "MainHopper", "FeederType" );
                if ( feederTypeName.Equals( "MultipleItems" ) )
                {
                    ScannerFeederType = FeederType.MultipleItems;
                }
                else
                {
                    ScannerFeederType = FeederType.SingleItem;
                }
            }
        }

        /// <summary>
        /// Handles the 1 event of the btnConnect_Click control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void btnConnect_Click( object sender, RoutedEventArgs e )
        {
            ConnectToScanner();
        }

        /// <summary>
        /// Handles the Click event of the btnScan control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void btnScan_Click( object sender, RoutedEventArgs e )
        {
            HandleScanButtonClick( sender, e, true );
        }

        /// <summary>
        /// Handles the scan button click.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        /// <param name="navigate">if set to <c>true</c> [navigate].</param>
        public void HandleScanButtonClick( object sender, RoutedEventArgs e, bool navigate )
        {
            Button scanButton = sender as Button;

            if ( ScanButtonText.IsStartScan( scanButton.Content as string ) )
            {
                if ( ScannerFeederType.Equals( FeederType.SingleItem ) )
                {
                    RangerScanner.StartFeeding( FeedSource.FeedSourceManualDrop, FeedItemCount.FeedOne );
                }
                else
                {
                    RangerScanner.StartFeeding( FeedSource.FeedSourceMainHopper, FeedItemCount.FeedContinuously );
                }

                if ( navigate )
                {
                    this.NavigationService.Navigate( ScanningPage );
                }
            }
            else
            {
                RangerScanner.StopFeeding();
                if ( navigate )
                {
                    this.NavigationService.Navigate( this );
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the btnOptions control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void btnOptions_Click( object sender, RoutedEventArgs e )
        {
            var optionsPage = new OptionsPage();
            this.NavigationService.Navigate( optionsPage );

            
        }

        private void MenuItem_Click_1( object sender, RoutedEventArgs e )
        {

        }

        private void mnuConnect_Click_1( object sender, RoutedEventArgs e )
        {

        }
    }
}