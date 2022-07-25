using Android.App;
using Android.OS;
using Android.Runtime;
using AndroidX.AppCompat.App;
using Android.Locations;
using System.Collections.Generic;
using System.Linq;

namespace DemoGPS
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, ILocationListener
    {
        Android.Widget.TextView textLongitude;
        Android.Widget.TextView textLatitude;

        Location currentLocation;
        LocationManager locationManager;

        string locationProvider;

        private void initializeLocationManager()
        {
            // Me conecto al servicio de Location del telefono o tablet
            locationManager = (LocationManager)GetSystemService(LocationService);

            // Se define el tipo de exactitud requerida
            Criteria criteria = new Criteria {
                Accuracy = Accuracy.Fine 
            };

            //Regresa la lista de proveedores en el telefono o tablet que cumplen con el criterio
            IList<string> providers = locationManager.GetProviders(criteria, true);

            if (providers.Any())
            {
                locationProvider = providers.First();
            }
            else
            {
                locationProvider = string.Empty;
            }
            System.Console.WriteLine($"El proveedor de GPS es: {locationProvider} ");
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            textLatitude = FindViewById<Android.Widget.TextView>(Resource.Id.txtLongitude);
            textLongitude = FindViewById<Android.Widget.TextView>(Resource.Id.txtLatitude);

            initializeLocationManager();
        }

        protected override void OnResume()
        {
            base.OnResume();
            locationManager.RequestLocationUpdates(locationProvider, 0, 0, this);
        }

        protected override void OnPause()
        {
            base.OnPause();
            locationManager.RemoveUpdates(this);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public void OnLocationChanged(Location location)
        {
            currentLocation = location;

            if (currentLocation == null)
            {
                System.Console.WriteLine("No se pudo obtener la ubicación actual...");
            }
            else
            {
                textLatitude.Text = currentLocation.Latitude.ToString();
                textLongitude.Text = currentLocation.Longitude.ToString();
            }
        }

        public void OnProviderDisabled(string provider)
        {
            throw new System.NotImplementedException();
        }

        public void OnProviderEnabled(string provider)
        {
            throw new System.NotImplementedException();
        }

        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
        {
            throw new System.NotImplementedException();
        }
    }
}