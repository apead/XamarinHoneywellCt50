using System;
using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Widget;
using Com.Honeywell.Aidc;

namespace Sample.Droid
{
    [Activity(Label = "Automatic Barcode")]
    public class AutomaticBarcodeActivity : Activity, BarcodeReader.IBarcodeListener, BarcodeReader.ITriggerListener
    {
        private BarcodeReader _barcodeReader;
        private ListView _barcodeList;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Barcode);


            _barcodeReader = MainActivity.BarcodeReader;

            if (_barcodeReader != null)
            {

                // register bar code event listener
                _barcodeReader.AddBarcodeListener(this);

                try
                {
                    _barcodeReader.SetProperty(BarcodeReader.PropertyTriggerControlMode,
                        BarcodeReader.TriggerControlModeAutoControl);
                }
                catch (UnsupportedPropertyException e)
                {
                    Toast.MakeText(this, "Failed to apply properties", ToastLength.Short).Show();
                }


                _barcodeReader.AddTriggerListener(this);

                IDictionary<String, Java.Lang.Object> properties = new Dictionary<string, Java.Lang.Object>();
                // Set Symbologies On/Off
                properties.Add(BarcodeReader.PropertyCode128Enabled, true);
                properties.Add(BarcodeReader.PropertyGs1128Enabled, true);
                properties.Add(BarcodeReader.PropertyQrCodeEnabled, true);
                properties.Add(BarcodeReader.PropertyCode39Enabled, true);
                properties.Add(BarcodeReader.PropertyDatamatrixEnabled, true);
                properties.Add(BarcodeReader.PropertyUpcAEnable, true);
                properties.Add(BarcodeReader.PropertyEan13Enabled, false);
                properties.Add(BarcodeReader.PropertyAztecEnabled, false);
                properties.Add(BarcodeReader.PropertyCodabarEnabled, false);
                properties.Add(BarcodeReader.PropertyInterleaved25Enabled, false);
                properties.Add(BarcodeReader.PropertyPdf417Enabled, false);
                // Set Max Code 39 barcode length
                properties.Add(BarcodeReader.PropertyCode39MaximumLength, 10);
                // Turn on center decoding
                properties.Add(BarcodeReader.PropertyCenterDecode, true);
                // Disable bad read response, handle in onFailureEvent
                properties.Add(BarcodeReader.PropertyNotificationBadReadEnabled, false);
                // Apply the settings
                _barcodeReader.SetProperties(properties);
            }

            // get initial list
            _barcodeList = (ListView) FindViewById(Resource.Id.listViewBarcodeData);

        }

        public void OnBarcodeEvent(BarcodeReadEvent barcodeReadEvent)
        {
            RunOnUiThread(() =>
            {
                var list = new List<String>
                {
                    "Barcode data: " + barcodeReadEvent.BarcodeData,
                    "Character Set: " + barcodeReadEvent.Charset,
                    "Code ID: " + barcodeReadEvent.CodeId,
                    "AIM ID: " + barcodeReadEvent.AimId,
                    "Timestamp: " + barcodeReadEvent.Timestamp
                };

                var adapter = new ArrayAdapter(this,
                   Resource.Layout.list_item, Resource.Id.textview, list);

              _barcodeList.Adapter = adapter;
         Console.WriteLine(barcodeReadEvent.BarcodeData);
            });
        }

        public void OnFailureEvent(BarcodeFailureEvent barcodeFailureEvent)
        {
            RunOnUiThread(() =>
            {
                Toast.MakeText(
                    this,
                    "No data",
                    ToastLength.Short).
                    Show();
            });
        }

        public void OnTriggerEvent(TriggerStateChangeEvent triggerStateChangeEvent)
        {
            
        }

        protected override void OnResume()
        {
            base.OnResume();
            if (_barcodeReader != null)
            {
                try
                {
                    _barcodeReader.Claim();
                }
                catch (ScannerUnavailableException e)
                {
                    Toast.MakeText(this, "Scanner unavailable", ToastLength.Short).Show();
                }
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (_barcodeReader != null)
            {
                // unregister barcode event listener
                _barcodeReader.RemoveBarcodeListener(this);

                // unregister trigger state change listener
                _barcodeReader.RemoveTriggerListener(this);
            }
        }
    }
}