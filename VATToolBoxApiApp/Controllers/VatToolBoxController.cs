using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;
using VATToolBoxApiApp.Models;
using VATToolBoxApiApp.Repository.Implementation;
using VATToolBoxApiApp.Repository.Interfaces;

namespace VATToolBoxApiApp.Controllers
{
    [RoutePrefix("VATToolBox")]
    // ReSharper disable once InconsistentNaming
    public class VATToolBoxController : ApiController
    {
        [HttpGet]
        [Route("CheckVatId/{countryCode}/{vatNumber}")]
        public ResponseObject ValidateVatId(string countryCode, string vatNumber)
        {
            var checkVatService = new eu.europa.ec.checkVatPortTypeClient();
            var responseObject = new ResponseObject();

            try
            {
                bool valid;
                string name;
                string address;

                // uncomment to shut this endpoint down
                // throw new Exception("UPGRADE_NOW");
                string countryCode1 = countryCode.ToUpper();
                checkVatService.checkVat(ref countryCode1, ref vatNumber, out valid, out name, out address);

                responseObject.IsValid = valid;
                if (valid)
                {
                    responseObject.Name = name;
                    responseObject.Address = address;
                }
            }
            catch (Exception exception)
            {
                responseObject.ServerResponse = exception.Message;
            }

            return responseObject;
        }


        [HttpGet]
        [Route("GetLatestRatesVersion")]
        public RateVersion GetRatesVersion()
        {
            //            var rateVersion = new RateVersion { Version = 2.02, VersionDate = new DateTime(2013, 01, 12).ToString("yyyyMMdd"), Status = "OK" };
            var rateVersion = new RateVersion { Version = 3.50, VersionDate = new DateTime(2016, 11, 01).ToString("yyyyMMdd") };
            // uncomment to shut this endpoint down
            //                var rateVersion = new RateVersion { Version = 2.06, VersionDate = new DateTime(2013, 01, 16).ToString("yyyyMMdd"), Status = "UPGRADE_NOW" };
            return rateVersion;
        }

        [HttpGet]
        [Route("GetWhatsNewInfoMore/{versionId}/")]
        public WhatsNewResultMore GetWhatsNewMore(string versionId)
        {
            var messages = new List<WhatsNewResultMore>
                               {
                                   new WhatsNewResultMore
                                       {
                                           Body =
                                               "Minor Bug fixes",
                                           ButtonTitle = "Ok",
                                           NotificationId = 1,
                                           Title = "What's new in V2.18",
                                           VersionId = "2.18"
                                       },

                                   new WhatsNewResultMore
                                       {
                                           Body =
                                               "2 new In-App Purchases added to VAT Toolbox. \n"
                                               + "1.   20,000 Tap bundle for those who use the Tap system. \n"
                                               + "2.   Unlimited Use - This single purchase grants you UNLIMITED access to ALL VAT Toolbox screens across ALL your devices. "
                                               + "This eliminates the need to buy any screens or Taps. \n\n\n"
                                               + "Introductory Taps now increased from 30 to 3,000 for NEW users.",
                                           ButtonTitle = "Ok",
                                           NotificationId = 1,
                                           Title = "What's new in V2.17",
                                           VersionId = "2.17"
                                       },

                                   new WhatsNewResultMore
                                       {
                                           Body =
                                               "A new modal screen that displays your tap usage has been added to the Store page. Please tap on the 'Log' icon at the top right corner of the Store page to reveal the Tap Log screen.",
                                           ButtonTitle = "Ok",
                                           NotificationId = 1,
                                           Title = "What's new in V2.15",
                                           VersionId = "2.15"
                                       },
                                  new WhatsNewResultMore
                                       {
                                           Body =
                                               "Maintenance Release to prepare for iOS7",
                                           ButtonTitle = "Ok",
                                           NotificationId = 1,
                                           Title = "What's new in V2.19",
                                           VersionId = "2.19"
                                       },
                                  new WhatsNewResultMore
                                       {
                                           Body =
                                               "VAT Toolbox is now fully optimised for iOS7",
                                           ButtonTitle = "Ok",
                                           NotificationId = 1,
                                           Title = "What's new in V3.00",
                                           VersionId = "3.00"
                                       },
                                  new WhatsNewResultMore
                                       {
                                           Body =
                                               "Minor UI tweaks on the Settings page.",
                                           ButtonTitle = "Ok",
                                           NotificationId = 1,
                                           Title = "What's new in V3.01",
                                           VersionId = "3.01"
                                       },
                                  new WhatsNewResultMore
                                       {
                                           Body =
                                               "Minor UI tweaks on the In-App purchase page.",
                                           ButtonTitle = "Ok",
                                           NotificationId = 1,
                                           Title = "What's new in V3.02",
                                           VersionId = "3.02"
                                       },
                                  new WhatsNewResultMore
                                       {
                                           Body =
                                               "Updated for iOS 8 to iOS10.",
                                           ButtonTitle = "Ok",
                                           NotificationId = 1,
                                           Title = "What's new in V3.50",
                                           VersionId = "3.50"
                                       }


                               };
            return messages.First(msg => msg.VersionId == versionId);
        }


        [HttpGet]
        [Route("GetLeastVersion")]
        public VATVersionObject GetEarliestAllowedVersion()
        {
            var allowed = new VATVersionObject
            {
                Message = "You are running an unsupported version of VAT Toolbox." +
                                            " Please download the latest version from the App Store to ensure that VAT Toolbox runs without problems ",
                VersionId = 3.50

            };
            return allowed;
        }

        [Route("GetRates")]
        public IEnumerable<CountryRates> GetRates()
        {
            IGetRatesRepository repo = new GetRatesRepository();
            var rates = repo.GetReturnRates();
            return rates;
        }

        [Route("SendFeedback")]
        public async Task<AppUserMessageResponse> SendFeedback([FromBody]AppUserMessage message)
        {
            try
            {
                var formContent = new StringContent(JsonConvert.SerializeObject(message), Encoding.ASCII,
                    "application/json");

                HttpClient myHttpClient = new HttpClient
                {
                    BaseAddress = new Uri(
                        "https://prod-07.westeurope.logic.azure.com:443/workflows/4b8767e51a7a4345aae216ca0146e0c0/triggers/manual/run?api-version=2016-06-01&sp=%2Ftriggers%2Fmanual%2Frun&sv=1.0&sig=-9MtJG7KT2vTbxFMnEytN1a7cpw-TNise7OpLTMgTn4")
                };
                var response = await myHttpClient.PostAsync("", formContent);

                var stringContent = await response.Content.ReadAsStringAsync();

                var responseObject = JsonConvert.DeserializeObject<AppUserMessageResponse>(stringContent);

                return responseObject;
            }
            catch (Exception ex)
            {
                return new AppUserMessageResponse { Success = false, Message = ex.Message };
            }

        }



    }
   


}
