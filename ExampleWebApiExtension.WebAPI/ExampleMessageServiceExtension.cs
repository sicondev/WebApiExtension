using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleWebApiExtension.WebAPI
{
    /// <summary>
    /// Class responsible for message service registration
    /// </summary>
    internal class ExampleMessageServiceExtension : Sicon.API.Sage200.WebAPI.Common.IMessageServiceRegistration
    {
        /// <summary>
        /// Subscribes to Message Service Events
        /// </summary>
        public void SubscribeToEvents()
        {
			try
			{
                Sage.Common.Messaging.MessageService messageService = Sage.Common.Messaging.MessageService.GetInstance();

                //Ensure no previous subscriptions still active
                messageService.Unsubscribe(Sage.Accounting.SOP.SOPLedgerMessageSource.SOPOrderSaved, new Sage.Common.Messaging.MessageHandler(HandleMessage));

                //Subscribe to the message
                messageService.Subscribe(Sage.Accounting.SOP.SOPLedgerMessageSource.SOPOrderSaved, new Sage.Common.Messaging.MessageHandler(HandleMessage));
            }
			catch (Exception)
			{

				throw;
			}
        }

        /// <summary>
        /// Handles the Message
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="args">Message Args</param>
        /// <returns>Message Response</returns>
        private Sage.Common.Messaging.Response HandleMessage(object sender, Sage.Common.Messaging.MessageArgs args)
        {
            //Handle the Message

            return new Sage.Common.Messaging.Response(Sage.Common.Messaging.ResponseArgs.Empty);
        }
    }
}
