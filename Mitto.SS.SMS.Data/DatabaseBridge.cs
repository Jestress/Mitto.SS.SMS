using Mitto.SS.SMS.Middleware.Core;
using Mitto.SS.SMS.Middleware.Core.Abstractions;
using ServiceStack.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mitto.SS.SMS.Data
{
    /// <summary>
    /// Database factory provider for performing CRUD operations on given db connection.
    /// </summary>
    public class SmsEntitiesDataProvider : ISmsEntitiesDataProvider
    {
        /// <summary>
        /// Command pattern for selecting countries from database
        /// </summary>
        private static readonly string GetCountriesCommandPattern = "SELECT `Code`, `Name`, `MobileCode`, `PricePerSms` FROM `mittosms`.`country`;";

        /// <summary>
        /// Command pattern for executing GetSentSms SP on the database
        /// </summary>
        private static readonly string GetSentSmsCommandPattern = "CALL GetSentSms(DATE('{0}'), DATE('{1}'), {2}, {3});";

        /// <summary>
        /// Command pattern for inserting a new SMS entity onto database
        /// </summary>
        private static readonly string SendSmsCommandPattern = "INSERT INTO `mittosms`.`sms` (`From`, `To`, `Text`, `SentDate`, `Country`, `IsSuccessful`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', 1);";

        /// <summary>
        /// Command pattern for retrieving statistics from database
        /// </summary>
        private static readonly string RetrieveStatisticsCommandPattern = "CALL GetSmsStats(DATE('{0}'), DATE('{1}'), '{2}');";

        /// <summary>
        /// Singleton dbFactory
        /// </summary>
        private IDbConnectionFactory dbFactory;
        public SmsEntitiesDataProvider(IDbConnectionFactory provider)
        {
            this.dbFactory = provider;
        }

        /// <summary>
        /// Returns sent sms entities that match parameter criteria
        /// </summary>
        /// <param name="from">Starting date boundary</param>
        /// <param name="to">End date boundary</param>
        /// <param name="skip">Skip first N records</param>
        /// <param name="take">Take returning N records</param>
        /// <param name="totalCount">Out parameter that indicates total number of results matched</param>
        /// <returns>Returns filtered sms entities</returns>
        public Sms[] GetSentSms(DateTime from, DateTime to, int skip, int take, out int totalCount)
        {
            var smsList = new List<Sms>();
            totalCount = 0;
            
            //Open database connection
            using (var connection = dbFactory.OpenDbConnection())
            {
                //Create a new database command
                using (var command = connection.CreateCommand())
                {
                    //Pass parameters to the command
                    command.CommandText = string.Format(GetSentSmsCommandPattern, from.ToDatabaseDateTimeFormat(), to.ToDatabaseDateTimeFormat(), skip, take);

                    //Iterate through data
                    using (var reader = command.ExecuteReader(System.Data.CommandBehavior.Default))
                    {
                        while (reader.Read())
                        {
                            smsList.Add(new Sms
                            {
                                SentDate = reader["SentDate"].ToDateTime(),
                                CountryCode = reader["MobileCode"].ToInt(),
                                From = reader["From"].ToString(),
                                To = reader["To"].ToString(),
                                Price = reader["PricePerSms"].ToDecimal(),
                                State = (SmsExecutionState)(reader["IsSuccessful"].ToInt()),
                            });

                            totalCount = reader["TotalCount"].ToInt();
                        }
                    }
                }
            }

            return smsList.ToArray();
        }

        /// <summary>
        /// Returns all countries stored in the system as configuration
        /// </summary>
        /// <returns>Array of <see cref="Country"/></returns>
        public Country[] RetrieveCountries()
        {
            var countries = new List<Country>();

            //Open database connection
            using (var connection = dbFactory.OpenDbConnection())
            {
                //Create a new database command
                using (var command = connection.CreateCommand())
                {
                    //Set the query
                    command.CommandText = GetCountriesCommandPattern;

                    //Iterate through data
                    using (var reader = command.ExecuteReader(System.Data.CommandBehavior.Default))
                    {
                        while (reader.Read())
                        {
                            countries.Add(new Country
                            {
                                Code = reader["Code"].ToLong(),
                                MobileCode = reader["MobileCode"].ToString(),
                                Name = reader["Name"].ToString(),
                                PricePerSms = reader["PricePerSms"].ToDecimal()
                            });
                        }
                    }
                }
            }

            return countries.ToArray();
        }

        /// <summary>
        /// Aggragates sent SMS data and filters according to the parameters
        /// Executes GetSmsStats SP on the database
        /// </summary>
        /// <param name="from">Start date of the filter</param>
        /// <param name="to">End date of the filter</param>
        /// <param name="mobileCodes">Codes will be joint together by ',' and will be passed to the SP</param>
        /// <returns></returns>
        public SmsStat[] RetrieveStatistics(DateTime from, DateTime to, string[] mobileCodes)
        {
            var stats = new List<SmsStat>();

            //Open new connection
            using (var connection = dbFactory.OpenDbConnection())
            {
                //Create an EXEC command
                using (var command = connection.CreateCommand())
                {
                    //Populate parameters
                    command.CommandText = string.Format(RetrieveStatisticsCommandPattern, from.ToDatabaseDateTimeFormat(), to.ToDatabaseDateTimeFormat(), string.Join(",", mobileCodes));

                    //EXECUTE
                    var reader = command.ExecuteReader(System.Data.CommandBehavior.Default);

                    //Iterate through data
                    while (reader.Read())
                    {
                        stats.Add(new SmsStat
                        {
                            Day = reader["SentDate"].ToDateTime(),
                            MobileCountryCode = reader["MobileCode"].ToString(),
                            PricePerSms = reader["PricePerSms"].ToDecimal(),
                            TotalPrice = reader["TotalCost"].ToDecimal()
                        });
                    }

                    reader.Dispose();
                }
            }

            return stats.ToArray();
        }

        /// <summary>
        /// Inserts a new Sms entitiy into database with given attributes
        /// </summary>
        /// <param name="sms">Sms entity</param>
        /// <returns>Returns a boolean that indicates operation success</returns>
        public bool SendSms(Sms sms)
        {
            var smsList = new List<Country>();

            //Open database connection
            using (var connection = dbFactory.OpenDbConnection())
            {
                //Create new insert command
                using (var command = connection.CreateCommand())
                {
                    //Pass parameters to the command
                    command.CommandText = string.Format(SendSmsCommandPattern, sms.From, sms.To, sms.Text, sms.SentDate.ToDatabaseDateTimeFormat(), sms.CountryCode);

                    //Execute
                    return command.ExecuteNonQuery() == 1;
                }
            }
        }
    }
}
