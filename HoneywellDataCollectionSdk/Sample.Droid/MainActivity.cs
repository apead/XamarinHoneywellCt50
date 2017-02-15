using System;
using Android.App;
using Android.Widget;
using Android.OS;
using Com.Honeywell.Aidc;

namespace Sample.Droid
{
    [Activity(Label = "Honeywell Barcode Example (Xam)", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity, AidcManager.ICreatedCallback
    {
        private int count = 1;
        public static BarcodeReader BarcodeReader;
        private Button _automaticButton;
        private Button _clientButton;
        private Button _manualTriggerButton;

        private AidcManager _aidcManager;


        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            _automaticButton = (Button) FindViewById<Button>(Resource.Id.buttonAutomaticBarcode);
            _clientButton = (Button)FindViewById<Button>(Resource.Id.buttonClientBarcode);
            _manualTriggerButton = (Button)FindViewById<Button>(Resource.Id.buttonManualTriggerBarcode);

            _automaticButton.Click += _automaticButton_Click;
            _clientButton.Click += _clientButton_Click; ;
            _manualTriggerButton.Click += _manualTriggerButton_Click; ;

            AidcManager.Create(this, this);

        }

        private void _manualTriggerButton_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(ManualTriggerBarcodeActivity));
        }

        private void _clientButton_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(ClientBarcodeActivity));
        }

        private void _automaticButton_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(AutomaticBarcodeActivity));
        }

        public void OnCreated(AidcManager aidcManager)
        {
            _aidcManager = aidcManager;
            BarcodeReader = _aidcManager.CreateBarcodeReader();
        }
    }
}
