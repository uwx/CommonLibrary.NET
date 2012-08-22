using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ComLib.LocationSupport;


namespace ComLib.Maps
{
    /// <summary>
    /// View model for a GeoAddress.
    /// </summary>
    public class GeoAddressViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GeoAddressViewModel"/> class.
        /// </summary>
        public GeoAddressViewModel()
        {
            Width = 300;
            Height = 300;
        }


        /// <summary>
        /// Creates a new instance of this class.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <param name="provider">The provider.</param>
        /// <param name="height">The height.</param>
        /// <param name="width">The width.</param>
        public GeoAddressViewModel(GeoAddress address, GeoProvider provider, int width, int height)
        {
            Init(address, provider, width, height);
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="GeoAddressViewModel"/> class.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <param name="providerName">Name of provider.</param>
        /// <param name="providerSourceUrl">Source url of provider.</param>
        /// <param name="providerApiKey">API key of provider.</param>
        /// <param name="height">The height.</param>
        /// <param name="width">The width.</param>
        public GeoAddressViewModel(Address address, string providerName, string providerSourceUrl, string providerApiKey, int width, int height)
        {
            Init(new GeoAddress() { Location = address }, 
                 new GeoProvider() { Name = providerName, SourceUrl = providerSourceUrl, ApiKey = providerApiKey },
                 width, height);
        }
        

        /// <summary>
        /// Initializes a new instance of the <see cref="GeoAddressViewModel"/> class.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <param name="provider">The provider.</param>
        /// <param name="height">The height.</param>
        /// <param name="width">The width.</param>
        public GeoAddressViewModel(Address address, GeoProvider provider, int width, int height)
        {
            Init(new GeoAddress() { Location = address }, provider, width, height);
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="GeoAddressViewModel"/> class.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <param name="height">The height.</param>
        /// <param name="width">The width.</param>
        public GeoAddressViewModel(Address address, int width, int height)
        {
            Init(new GeoAddress() { Location = address }, GeoProvider.Provider, width, height);
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="GeoAddressViewModel"/> class.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <param name="height">The height.</param>
        /// <param name="width">The width.</param>
        public GeoAddressViewModel(string address, int width, int height)
        {
            var add = new Address();
            add.FullAddress = address;
            Init(new GeoAddress() { Location = add }, GeoProvider.Provider, width, height);
        }
        

        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        /// <value>The address.</value>
        public GeoAddress Address { get; set; }


        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        /// <value>The address.</value>
        public GeoProvider Provider { get; set; }


        /// <summary>
        /// Gets or sets the width in pixels to show the map.
        /// </summary>
        /// <value>The width.</value>
        public int Width { get; set; }


        /// <summary>
        /// Gets or sets the height in pixels to show the map.
        /// </summary>
        /// <value>The height.</value>
        public int Height { get; set; }


        /// <summary>
        /// Inits with the data supplied.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <param name="provider">The provider.</param>
        /// <param name="height">The height.</param>
        /// <param name="width">The width.</param>
        public void Init(GeoAddress address, GeoProvider provider, int width, int height)
        {
            Address = address;
            Provider = provider;
            Width = height;
            Height = width;
        }
    }



    /// <summary>
    /// The geo provider to use.
    /// </summary>
    public class GeoProvider
    {
        private static GeoProvider _current = new GeoProvider() { Name = "bing", SourceUrl = "http://ecn.dev.virtualearth.net/mapcontrol/mapcontrol.ashx?v=6.2", ApiKey = "" };


        /// <summary>
        /// Inits the current geoprovider with the one specified.
        /// </summary>
        /// <param name="provider">The provider.</param>
        public static void Init(GeoProvider provider)
        {
            _current = provider;
        }


        /// <summary>
        /// Gets the provider.
        /// </summary>
        /// <value>The provider.</value>
        public static GeoProvider Provider
        {
            get { return _current; }
        }


        /// <summary>
        /// Gets or sets the provider "bing" | "google"
        /// </summary>
        /// <value>The provider.</value>
        public string Name { get; set; }


        /// <summary>
        /// Gets or sets the provider http://ecn.dev.virtualearth.net/mapcontrol/mapcontrol.ashx?v=6.2
        /// </summary>
        /// <value>The provider.</value>
        public string SourceUrl { get; set; }


        /// <summary>
        /// Api key used for provider
        /// </summary>
        public string ApiKey { get; set; }
    }
    


    /// <summary>
    /// GeoAddress, can be either address based or Latitude/Longitude based.
    /// </summary>
    public class GeoAddress 
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GeoAddress"/> class.
        /// </summary>
        public GeoAddress() : this(string.Empty)
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="GeoAddress"/> class.
        /// </summary>
        /// <param name="address">The address.</param>
        public GeoAddress(string address)
        {       
            Location = new Address();
            IsAddressBased = true;
            if (!string.IsNullOrEmpty(address))
                Location.FullAddress = address;
        }


        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        /// <value>The location.</value>
        public Address Location { get; set; }


        /// <summary>
        /// Gets or sets the latitude.
        /// </summary>
        /// <value>The latitude.</value>
        public double Latitude { get; set; }


        /// <summary>
        /// Gets or sets the longitude.
        /// </summary>
        /// <value>The longitude.</value>
        public double Longitude { get; set; }


        /// <summary>
        /// Gets or sets a value indicating whether this instance is address based.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is address based; otherwise, <c>false</c>.
        /// </value>
        public bool IsAddressBased { get; set; }


        /// <summary>
        /// Gets the full address.
        /// </summary>
        /// <value>The full address.</value>
        public string FullAddress
        {
            get { return Location.FullAddress; }
        }
    }
}
