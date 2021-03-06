﻿using System;
using CityLife;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorCitylife.Models
{
    public enum ApartOccuStatus : int
    {
        Free = 0,
        Reserved = 1,
        Occupied = 2
    }

    public enum DisplayDevice
    {
        MOBILE,
        DESKTOP,
        ANY
    }
    public enum OrientationType : short
    {
        Landscape = 0,
        Portrait = 1
    }
    public enum PhotoType : short
    {
        Main = 0,
        Additional = 1,
        Thumbnail = 2
    }
    public partial class Apartment
    {
        /// <summary>
        /// The method gives the nightly price for the apartment, without discounts
        /// Note that discounts can be calculated only if you know the exact dates of 
        /// staying in the apartment
        /// </summary>
        /// <param name="adults">number of adults</param>
        /// <param name="children">number of fhildren</param>
        /// <param name="weekend">true for weekend, false for weekday</param>
        /// <param name="currencyCode">the currency code in which the price should be quoted.</param>
        /// <param name="atDate">the date for which you want to get the exchange rate</param>
        /// <returns>nightly price</returns>
        public Money PricePerNight(int adults, int children, bool weekend, string currencyCode, DateTime atDate)
        {

            var thePrice = from price in this.Pricings
                           where price.Adults == adults && price.Children == children
                           select price;
            if (thePrice.Count() == 0)
            {
                //We did not find a pricing record for that number of adults and children
                return null;
            }
            else
            {
                Pricing thePricing = thePrice.First();
                if (weekend)
                {
                    Money theMoney = thePricing.priceWeekendAsMoney();
                    return theMoney.converTo(currencyCode, atDate, rounded: true);
                }
                else
                {
                    Money theMoney = thePricing.priceWeekendAsMoney();
                    return theMoney.converTo(currencyCode, atDate, rounded: true);
                }
            }
        }

        /// <summary>
        /// Calculates the exact price to be paid for the stay in the apartment. It takes into account:
        /// number of adults, children, start date, end date, weekdays/weekends and discounts.
        /// OBSOLETE???
        /// </summary>
        /// <param name="thePricing">the pricing record by which to claculate the price (based on number of adults and children)</param>
        /// <param name="currencyCode">currency by which to show the price</param>
        /// <param name="fromDate">date of entrance</param>
        /// <param name="toDate">date of exit</param>
        /// <returns></returns>
        //public int pricePerStay(Pricing thePricing, string currencyCode, DateTime fromDate, DateTime toDate)
        //{
        //    return 0;//TBD
        //}

        /// <summary>
        /// The method returns the main landscape photo of the apartment, if such exists, and null if none. If more than one photo is tagged
        /// as "main"and "landscape" - return the first one
        /// </summary>
        /// <returns>apartment main photo or null if nont exists</returns>
        public ApartmentPhoto MainPhotoLandscape()
        {
            ApartmentPhoto theMainPhoto;
            if (this.ApartmentPhotoes.Count() == 0)
            {
                theMainPhoto = new ApartmentPhoto()
                {
                    FilePath = "/images/missing landscape photo.png",
                    Orientation = (short)OrientationType.Landscape,
                    Type = (short)PhotoType.Main
                };
            }
            else
            {
                theMainPhoto = this.ApartmentPhotoes.FirstOrDefault(aPhoto => aPhoto.Type == (short)PhotoType.Main &&
                                                                            aPhoto.Orientation == (short)OrientationType.Landscape);
            }
            return theMainPhoto;
        }
        /// <summary>
        /// The method returns the main portrait photo of the apartment, if such exists, and null if none. If more than one photo is tagged
        /// as "main" and "portrait" - return the first one
        /// </summary>
        /// <returns>apartment main photo or null if nont exists</returns>
        public ApartmentPhoto MainPhotoPortrait()
        {
            ApartmentPhoto theMainPhoto;
            if (this.ApartmentPhotoes.Count() == 0)
            {
                theMainPhoto = new ApartmentPhoto()
                {
                    FilePath = "/images/missing portrait photo.jpg",
                    Orientation = (short)OrientationType.Portrait,
                    Type = (short)PhotoType.Main
                };
                //theMainPhoto = this.ApartmentPhotoes.Single(aPhoto => aPhoto.filePath == "/images/missing portrait photo.jpg"); ;
            }
            else
            {
                citylifedb8_blContext db = new citylifedb8_blContext();
                theMainPhoto = db.ApartmentPhoto.FirstOrDefault(aPhoto => aPhoto.Type == (short)PhotoType.Main &&
                                                                            aPhoto.Orientation == (short)OrientationType.Portrait);
            }
            return theMainPhoto;
        }
        public ApartmentPhoto mainPhoto(DisplayDevice preferredDevice = DisplayDevice.ANY)
        {
            ApartmentPhoto theMainPhoto;
            if (this.ApartmentPhotoes.Count() == 0)
            {
                theMainPhoto = new ApartmentPhoto()
                {
                    FilePath = "/images/missing landscape photo.jpg",
                    Orientation = (short)OrientationType.Landscape,
                    Type = (short)PhotoType.Main
                };
            }
            else if (preferredDevice == DisplayDevice.ANY)
            {
                //Find any main photo, regardless of device
                theMainPhoto = this.ApartmentPhotoes.FirstOrDefault(aPhoto => aPhoto.Type == (short)PhotoType.Main);

            }
            else
            {
                //Find main photo for the preferred device
                if (preferredDevice == DisplayDevice.DESKTOP)
                {
                    theMainPhoto = this.ApartmentPhotoes.FirstOrDefault(aPhoto => aPhoto.Type == (short)PhotoType.Main &&
                                                                                 aPhoto.ForDesktop == true);
                }
                else
                {
                    //Find preferred main photo for mobile
                    theMainPhoto = this.ApartmentPhotoes.FirstOrDefault(aPhoto => aPhoto.Type == (short)PhotoType.Main &&
                                                                                 aPhoto.ForMobile == true);
                }

            }
            return theMainPhoto;
        }
        /// <summary>
        /// The method returns all photos (including the main photo for that apartment. If the preferred deivce is set - 
        /// will return only photos which are adequate for this device.
        /// </summary>
        /// <param name="prefferedDevice">MOBILE/DESKTOP/ANY</param>
        /// <returns></returns>
        public IEnumerable<ApartmentPhoto> galleryPhotos(DisplayDevice prefferedDevice = DisplayDevice.ANY)
        {
            IEnumerable<ApartmentPhoto> thePhotos;
            if (prefferedDevice == DisplayDevice.ANY)
            {
                thePhotos = this.ApartmentPhotoes;
            }
            else if (prefferedDevice == DisplayDevice.MOBILE)
            {
                thePhotos = this.ApartmentPhotoes.Where(aPhoto => aPhoto.ForMobile == true);
            }
            else
            {
                thePhotos = this.ApartmentPhotoes.Where(aPhoto => aPhoto.ForDesktop == true);
            }
            return thePhotos;
        }

        /// <summary>
        /// Returns a list of features (actually feature keys, as they need to go through translation).
        /// It extracts the data from the featureKey field which contains all features separated by ;
        /// </summary>
        /// <returns></returns>
        public List<string> featureKeyList()
        {
            List<string> featureList = this.FeaturesKeys.Split(';').ToList();
            return featureList;
        }

        /// <summary>
        /// returns a list of description keys. Each one will be translated into a single paragraph of description 
        /// Taken from the description field which contains: apartment 5 paragraph 1;apartment 5 paragraph 2;...
        /// </summary>
        /// <returns></returns>
        public List<string> descriptionKeyList()
        {
            List<string> descriptionList = this.DescriptionKey.Split(';').ToList();
            return descriptionList;
        }
    }
}
