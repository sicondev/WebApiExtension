using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace ExampleWebApiExtension.WebAPI
{
    /// <summary>
    /// Example Web Api Controller
    /// </summary>
    /// <remarks>The MethodGroup attribute determines whether the controller appears in the auto generated Web Api documentation</remarks>
    [Sicon.API.Sage200.WebAPI.Attributes.MethodGroup("Bespoke", Sicon.API.Sage200.WebAPI.Attributes.MethodGroupAttribute.MISC)]
    public class ExampleController : System.Web.Http.ApiController
    {
        private const string MODULE_KEY = "";
        private const string PRODUCT_NAME = "Example Extension";
        private const bool VALIDATE_ENABLE_STRING = false;

        /// <summary>
        /// Gets the Most Recent SOP Order Return
        /// </summary>
        /// <param name="companyID">The Company ID to Connect to</param>
        /// <returns>Document No or an empty string</returns>
        [ResponseType(typeof(string))]
        [HttpGet]
        public IHttpActionResult GetMostRecentSOPOrderReturnNo(int? companyID = null)
        {
            try
            {
                //Connect to Sage, if not company ID is specified, the default company in the web config will be used
                //You dont need to pass a module key for non Sicon Modules
                //Pass 'false' for Validate enable string
                Sicon.API.Sage200.WebAPI.Common.SageConnection.Connect(companyID, MODULE_KEY, PRODUCT_NAME, VALIDATE_ENABLE_STRING);

                using (Sage.Accounting.SOP.SOPOrderReturns sopOrderReturns = new Sage.Accounting.SOP.SOPOrderReturns())
                {
                    sopOrderReturns.Query.Sorts.Add(new Sage.ObjectStore.Sort(Sage.Accounting.SOP.SOPOrderReturn.FIELD_DATETIMECREATED, false));

                    sopOrderReturns.Find();

                    return Ok(sopOrderReturns.First?.DocumentNo ?? string.Empty);
                }
            }
            catch (Exception e)
            {
               return this.InternalServerError(e);
            }
        }

        /// <summary>
        /// Posts a new SOP Order
        /// </summary>
        /// <param name="customerAccountRef">The Customer Account Reference</param>
        /// <param name="secondReference">The Second Reference</param>
        /// <param name="orderDate">The Order Date</param>
        /// <param name="companyID">The Company Id to connect to</param>
        /// <returns>Document No</returns>
        [ResponseType(typeof(string))]
        [HttpPost]
        public IHttpActionResult PostSOPOrder(string customerAccountRef, string secondReference, DateTime? orderDate, int? companyID = null)
        {
            try
            {
                if (string.IsNullOrEmpty(customerAccountRef)) return BadRequest($"{nameof(customerAccountRef)} was not specified.");

                //Connect to Sage
                Sicon.API.Sage200.WebAPI.Common.SageConnection.Connect(companyID, MODULE_KEY, PRODUCT_NAME, VALIDATE_ENABLE_STRING);

                Sage.Accounting.SalesLedger.Customer customer = Sage.Accounting.SalesLedger.CustomerFactory.Factory.Fetch(customerAccountRef);

                if (customer == null) return BadRequest($"Customer with Reference '{customerAccountRef}' was not found.");

                using (Sage.Accounting.SOP.SOPOrder sopOrder = new Sage.Accounting.SOP.SOPOrder())
                {
                    sopOrder.Customer = customer;
                    sopOrder.CustomerDocumentNo = secondReference;

                    if (orderDate.HasValue)
                        sopOrder.DocumentDate = orderDate.Value;

                    sopOrder.Update();

                    return Ok(sopOrder.DocumentNo);
                }
            }
            catch (Exception e)
            {
                return this.InternalServerError(e);
            }
        }

        /// <summary>
        /// Performs a Get Request
        /// </summary>
        /// <param name="companyID"></param>
        /// <remarks>Applying the API_BasicAuthentication attribute will cause web api to check there is a basic auth header on the request which expects a username and password configured for Web Api users in Sage.
        /// 'WebAPISecurityEnabled' must be enabled in the web.config file </remarks>
        /// <returns></returns>
        [ResponseType(typeof(string))]
        [HttpGet]
        [Sicon.API.Sage200.WebAPI.Authentication.API_BasicAuthentication]
        public IHttpActionResult GetWithAuthentication(int? companyID = null)
        {
            try
            {
                return Ok("Success");
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }
    }
}
