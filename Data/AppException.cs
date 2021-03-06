﻿using BlazorCitylife.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
// look in: https://stackoverflow.com/questions/4272579/how-to-print-full-stack-trace-in-exception
//for getting full stack trace
namespace CityLife
{
    public static class ErrorCodeCollection
    {
        private static Dictionary<int, string> errorCodesDict = new Dictionary<int, string>()
        {
            {101,"no suitable exchange rate was found for currencies: {0} and {1} for date {2} (or later). Sum was {3}"},
            {102,"such currency does not exist in DB:{0}"},
            {103,"Addition of money objects - the 2 objects have different currency, which is not supported. {0} and {1}"},
            {104,"Multiplication of money objects - the 2 objects have different currency, which is not supported. {0} and {1}"},
            {105,"Currency not found in DB:{0}"},
            {106,"an unsupported language requested:{0}"},
            {107,"Before running setStartSeries you must run startTestCycle" },
            {108,"checkbox name of unit test report has an unexpected format. Actual content:{0}. Expected format:<series>-<number>" },
            {109,"unit test series {0} number {1} was not found in DB" },
            {110,"HTTP exception occured. Code:{0} type:{1} message:{2}" },
            {111,"the name of each element in the translation form should be of the form XX-NN where XX is the language code and NN is the transaltion key id. Actual format was: {0}" },
            {112,"the apartment was not found in the List<ApaartmentPrce> collection in the Session variable. Apartment id was:{0}" },
            {113,"An order was created for a non-free day. Apartment:{0} date:{1} guest name:{2}" },
            {114,"a foreign key symbolic name was not found in a prevkious table id. symbolic name is:{0}. table name:{1}" },
            {115,"the variable dictionary does not contain the expected message variable: {0}" },
            {116, "the message contains either empty variable [], or something like: [asd[ (an open bracket inside a variable name). message is:{0}" },
            {117,"the money string contains an invalid currency symbol. money string is:{0}" },
            {118,"the money string could not be converted to decimal value. String is: {0}" },
            {119, "order {0} could not be deleted." },
            {120, "translation box got a null translation key" },
            {121, "country list file should have exactly 2 fields. Line contains {0} fields. Line content:{1}" },
            {122, "mail could not be sent. to:{0} mail name:{1}" },
            {123,"SQL statement for query {0} was: {1}" },
            {124, "Invalid month number. Expected values between 1 to 12. Actual value:{0}" },
            {125, "Trying to add an element to a 2 dimensional dictionary. Both keys already exist. Keys are:{0} and {1}" }



        };
        public static bool exists(int code)
        {
            return errorCodesDict.ContainsKey(code);
        }
        public static string message(int code)
        {
            if (exists(code))
            {
                return errorCodesDict[code];
            }
            else
            {
                return "undefined:" + code;
            }
        }
           
    }
    public class AppException : Exception
    {
        public AppException(int code, Exception innerException, params object[] parameters) : base(AppException.formatMessage(code, parameters),
                                                                                                  innerException)
        {
            writeException(code, innerException, this.StackTrace, parameters);
        }

        public static void writeException(int code, Exception innerException, string stackTrace, params object[] parameters)
        {
            citylifedb8_blContext db = new citylifedb8_blContext();
            ErrorCode theErrorCode = db.ErrorCode.SingleOrDefault(aRecord => aRecord.Code == code);
            if (theErrorCode != null)
            {
                //This error code already exists in the DB. 
                theErrorCode.LastOccurenceDate = DateTime.Now;
                theErrorCode.OccurenceCount++;
            }
            else
            {
                //The error code does not exist - create it
                theErrorCode = new ErrorCode() { Code = code, Message = ErrorCodeCollection.message(code),
                                                 LastOccurenceDate = DateTime.Now, OccurenceCount = 1 };
                db.ErrorCode.Add(theErrorCode);
            }
            db.SaveChanges();
            //check if the same error message exists in the error message table
            string formattedMessage = AppException.formatMessage(code, parameters);
            ErrorMessage theErrorMessage = (from anErrorMessage in theErrorCode.ErrorMessages
                                            where anErrorMessage.FormattedMessage == formattedMessage &&
                                                  anErrorMessage.StackTrace == stackTrace
                                            select anErrorMessage).FirstOrDefault();  //actually we expect only a single record to be found
            if (theErrorMessage != null)
            {
                //Such an exact error message already exists - increment the counter
                theErrorMessage.LastOccurenceDate = DateTime.Now;
                theErrorMessage.OccurenceCount++;
            }
            else
            {
                //such exact error message does not exist - add it
                theErrorMessage = new ErrorMessage()
                {
                   ErrorCodeCode = theErrorCode.Code,
                    FormattedMessage = formattedMessage,
                    LastOccurenceDate = DateTime.Now,
                    OccurenceCount = 1,
                    StackTrace = stackTrace,
                };
                db.ErrorMessage.Add(theErrorMessage);
            }
            db.SaveChanges();

        }
        private static string formatMessage(int code, params object[] parameters)
        {
            if (ErrorCodeCollection.exists(code))
            {
                string message = ErrorCodeCollection.message(code);
                string formattedMessage = string.Format(message, parameters);
                return formattedMessage;
            }
            else
            {
                return ErrorCodeCollection.message(code) + " " + parameters;
            }
        }
    }
}