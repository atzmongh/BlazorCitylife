﻿using BlazorCitylife.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CityLife
{
    
    /// <summary>
    /// BookedBy enum - it was not declared in the DB level, and for some strange reason could not be declared in its
    /// own file. Note that in the DB we keep the string values of the enum.
    /// </summary>
    public enum BookedBy
    {
        me,
        guest,
        booking,
        airbnb,
        other
    }

    /// <summary>
    /// Money represents an amount of money in a specific currency.
    /// </summary>
    public class Money
    {
        private decimal amount=0;
        private string currencyCode;
       // private static cityLifeDBContainer1 db = new cityLifeDBContainer1();
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="anAmount">the money amount</param>
        /// <param name="aCurrencyCode">should be a valid currency code, which exists in the DB. The constructor does not check for validity.
        /// Main currency codes are: USD, EUR, UAH, ILS... See list of currency codes in: https://www.iban.com/currency-codes
        /// </param>
        public Money(decimal anAmount, string aCurrencyCode)
        {
            amount = anAmount;
            currencyCode = aCurrencyCode;
        }
        public Money(Money m)
        {
            amount = m.amount;
            currencyCode = m.currencyCode;
        }
        /// <summary>
        /// This constructor accepts the amount in cents and creates a money object. This is the way
        /// Money is held in the DB
        /// </summary>
        /// <param name="sumInCents">amount in cents (so 12.50 is actually 1250)</param>
        /// <param name="aCurrencyCode"></param>
        public Money(int sumInCents, string aCurrencyCode)
        {
            this.amount = ((decimal)sumInCents) / 100;
            this.currencyCode = aCurrencyCode;
        }
        /// <summary>
        /// The constructor gets a string containing: $1,234.56. If currencyCode exists - uses it as the currency, regardless if thre is a currency symbol 
        /// or not. If not - creates the currency based on the symbol. If symbol does not exist, or it is an unknown symbol - aborts
        /// Note that empty string will be converted to 0
        /// </summary>
        /// <param name="moneyString"></param>
        /// <param name="currencyCode"></param>
        public Money (string moneyString, string currencyCode=null)
        {
            if (moneyString == "" || moneyString == null)
            {
                this.amount = 0;
                this.currencyCode = currencyCode;
                return;
            }
            //The string is not empty
            char firstChar = moneyString[0];
            if (firstChar<'0' || firstChar > '9')
            {
                //The amount start with a non-digit character - assuming it is a currency symbol
                citylifedb8_blContext db = new citylifedb8_blContext();
                string firstCharAsString = firstChar.ToString();
                Currency theCurrency = db.Currency.SingleOrDefault(a => a.Symbol == firstCharAsString);
                if (theCurrency == null)
                {
                    //Such currency does not exist in our DB - raise an exception
                    throw new AppException(117, null, moneyString);
                }
                this.currencyCode = theCurrency.CurrencyCode;
                string amountSt = moneyString.Substring(1);   //Take out the currency symbol - assuming this is a clean number
                if (!decimal.TryParse(amountSt, out this.amount))
                {
                    //The conversion did not succeed - raise an exception
                    throw new AppException(118, null, moneyString);
                }
            }
            else
            {
                //Assume that the number does not contain a currency symbol - convert it to decimal
                if (!decimal.TryParse(moneyString, out this.amount))
                {
                    //The conversion did not succeed - raise an exception
                    throw new AppException(118, null, moneyString);
                }
                this.currencyCode = currencyCode;
            }
        }
        public static Money operator * (Money m1, Money m2)
        {
            if (m1.currencyCode != m2.currencyCode)
            {
                throw new AppException(103, null, "Multiplication of money objects - the 2 objects have different currency, which is not supported. " +
                                       m1.ToString() + " " + m2.ToString());
            }
            Money result = new Money(m1.amount * m2.amount, m1.currencyCode);
            return result;
        }
        public static Money operator /(Money m1, int divider)
        {
            
            Money result = new Money(m1.amount / divider, m1.currencyCode);
            return result;
        }
        public static Money operator +(Money m1, Money m2)
        {
            if (m1.currencyCode != m2.currencyCode)
            {
                throw new AppException(103, null, "Addition of money objects - the 2 objects have different currency, which is not supported. " +
                                       m1.ToString() + " " + m2.ToString());
            }
            Money result = new Money(m1.amount + m2.amount, m1.currencyCode);
            return result;
        }
        public static Money operator -(Money m1, Money m2)
        {
            if (m1.currencyCode != m2.currencyCode)
            {
                throw new AppException(103, null, " Subtraction of money objects - the 2 objects have different currency, which is not supported. " +
                                       m1.ToString() + " " + m2.ToString());
            }
            Money result = new Money(m1.amount - m2.amount, m1.currencyCode);
            return result;
        }

        public static Money operator *(Money m1, decimal d2)
        {
            Money result = new Money(m1.amount * d2, m1.currencyCode);
            return result;
        }

        //public static bool operator == (Money m1, Money m2)
        //{
        //    return m1.amount == m2.amount && m1.currencyCode == m2.currencyCode;
        //}
        //public static bool operator !=(Money m1, Money m2)
        //{
        //    return m1.amount != m2.amount || m1.currencyCode != m2.currencyCode;
        //}
        public bool isZero()
        {
            return amount == 0m;
        }
        public static bool operator <=(Money m1, Money m2)
        {
            return m1.amount <= m2.amount && m1.currencyCode == m2.currencyCode;
        }
        public static bool operator >=(Money m1, Money m2)
        {
            return m1.amount >= m2.amount && m1.currencyCode == m2.currencyCode;
        }

        /// <summary>
        /// 
        /// returns a string representation of the Money object as : USD 156.32
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return currencyCode + " "+string.Format("{0:###0.00}",this.amount);
        }

        /// <summary>
        /// converts the money object to string representation
        /// </summary>
        /// <param name="showCents">when true will show cents as 123.45 When false will round to the closest integral value</param>
        /// <param name="showCurrency">when true will show the currency symbol to the left of the value.</param>
        /// <returns>$1,234.56 orf $1235 bsaed on the showCents param</returns>
        public string toMoneyString(bool showCents = false, bool showCurrency = false, bool showComma = true)
        {
            string currencySymbol = "";
            if (showCurrency)
            {
                citylifedb8_blContext db = new citylifedb8_blContext();
                var theCurrency = db.Currency.SingleOrDefault(aCurrency => aCurrency.CurrencyCode == this.currencyCode);
                if (theCurrency == null)
                {
                    throw new AppException(102, null, "such currency does not exist in DB:" + this.currencyCode);
                }
                currencySymbol = theCurrency.Symbol;
            }
            //At this point currency symbol contains either the currency symbol or empty string, if show currency is false
            string comma = "";
            if (showComma)
            {
                comma = ",";
            }
            string moneyString;
            if (showCents)
            {
                moneyString = string.Format("{0}{1:#"+comma+"##0.00}", currencySymbol, this.amount);
            }
            else 
            {
                moneyString = string.Format("{0}{1:#"+comma+"##0}", currencySymbol, decimal.Round(this.amount));
            }
            
            return moneyString;
        }

        /// <summary>
        /// Converts the amount of money from currency to currency based on currency exchange records in the DB.
        /// </summary>
        /// <param name="newCurrencyCode">target currency to which we need to convert</param>
        /// <param name="atDate">the date for which we would like to convert. The method will look for latest currency 
        /// exchange record which is still before or at that date. </param>
        /// <param name="rounded">When set to true - rounds the converted result to integral value</param>
        /// <returns>a Money object converted to the new currency.</returns>
        /// <exception cref="101">no suitable currency exchange record found in the DB. (Either the currency code was illegal, or no
        /// such record exists)</exception>
        public Money converTo(string newCurrencyCode, DateTime? atDate=null, bool rounded = false)
        {
            
            if (currencyCode == newCurrencyCode)
            {
                //current and target currencies are the same - no conversion needed.
                return new Money(this);
            }
            else
            {
                //Look for all exchange rates records that match the "to currency" and "from currency". 
                //Note that in the DB we do not keep both conversion rates: EUR->USD and USD->EUR - since one is the reciprocal
                //of the other. We only keep the pair where the "from" curency code is alphabetically lower than the "to"
                //So we keep EUR->USD, UAH->USD, etc.
                citylifedb8_blContext db = new citylifedb8_blContext();
                if (atDate == null)
                {
                    atDate = FakeDateTime.Now;
                }
                string lowerCurrencyCode;
                string upperCurrencyCOde;
                if (this.currencyCode.CompareTo(newCurrencyCode)<0)
                {
                    lowerCurrencyCode = this.currencyCode;
                    upperCurrencyCOde = newCurrencyCode;
                }
                else
                {
                    lowerCurrencyCode = newCurrencyCode;
                    upperCurrencyCOde = this.currencyCode;
                }
                var exchangeRates = from anExchangeRate in db.CurrencyExchange
                                    where anExchangeRate.FromCurrencyCurrencyCode == lowerCurrencyCode &&
                                          anExchangeRate.ToCurrencyCurrencyCode    == upperCurrencyCOde  &&
                                          anExchangeRate.Date <= atDate
                                    orderby anExchangeRate.Date descending
                                    select anExchangeRate;
                if (exchangeRates.Count() == 0)
                {
                    //no suitable exchange rate was found
                    throw new AppException(101, null, this.currencyCode, newCurrencyCode, atDate, this.amount);
                }
                var theExchangeRate = exchangeRates.First();  //take the latest record which is still before or at the requested date
                decimal result;
                if (theExchangeRate.FromCurrencyCurrencyCode == this.currencyCode)
                {
                    result = this.amount * theExchangeRate.ConversionRate;
                }
                else
                {
                    result = this.amount / theExchangeRate.ConversionRate;
                }
                if (rounded)
                {
                    result = decimal.Round(result);
                }
                return new Money(result, newCurrencyCode);
            }
        }

        public int toCents()
        {
            return (int)(amount * 100);
        }

        public string currency
        {
            get { return currencyCode; }
        }
    }
}