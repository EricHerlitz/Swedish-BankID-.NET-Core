using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;

namespace Herlitz.BankID
{
    public class StatusHandler : IStatusHandler
    {
        public Dictionary<string, string> codes = new Dictionary<string, string>{
            {"RFA1", "Starta BankID-appen."},
            {"RFA2", "Du har inte BankID-appen installerad. Kontakta din bank."},
            {"RFA3", "Åtgärden avbruten. Försök igen."},
            {"RFA4", "En identifiering eller underskrift för det här personnumret är redan påbörjad. Försök igen."},
            {"RFA5", "Internt tekniskt fel. Försök igen."},
            {"RFA6", "Åtgärden avbruten."},
            {"RFA8", "BankID-appen svarar inte. Kontrollera att det är startat och att du har internetanslutning. Om du inte har något giltigt BankID kan du hämta ett hos din Bank. Försök sedan igen."},
            {"RFA9", "Skriv in din säkerhetskod i BankID-appen och välj Legitimera eller Skriv under."},
            {"RFA13", "Försöker starta BankID-appen."},
            {"RFA14A", "Söker efter BankID, det kan ta en liten stund… Om det har gått några sekunder och inget BankID har hittats har du sannolikt inget BankID som går att använda för den aktuella identifieringen/underskriften i den här datorn. Om du har ett BankIDkort, sätt in det i kortläsaren. Om du inte har något BankID kan du hämta ett hos din internetbank. Om du har ett BankID på en annan enhet kan du starta din BankID-app där."},
            {"RFA14B", "Söker efter BankID, det kan ta en liten stund… Om det har gått några sekunder och inget BankID har hittats har du sannolikt inget BankID som går att använda för den aktuella identifieringen/underskriften i den här enheten. Om du inte har något BankID kan du hämta ett hos din internetbank. Om du har ett BankID på en annan enhet kan du starta din BankID-app där."},
            {"RFA15A", "Söker efter BankID, det kan ta en liten stund… Om det har gått några sekunder och inget BankID har hittats har du sannolikt inget BankID som går att använda för den aktuella identifieringen/underskriften i den här datorn. Om du har ett BankIDkort, sätt in det i kortläsaren. Om du inte har något BankID kan du hämta ett hos din internetbank. "},
            {"RFA15B", "Söker efter BankID, det kan ta en liten stund… Om det har gått några sekunder och inget BankID har hittats har du sannolikt inget BankID som går att använda för den aktuella identifieringen/underskriften i den här enheten. Om du inte har något BankID kan du hämta ett hos din internetbank."},
            {"RFA16", "Det BankID du försöker använda är för gammalt eller spärrat. Använd ett annat BankID eller hämta ett nytt hos din bank."},
            {"RFA17A", "BankID-appen verkar inte finnas i din dator eller telefon. Installera den och hämta ett BankID hos din internetbank. Installera appen från din appbutik eller https://install.bankid.com."},
            {"RFA17B", "Misslyckades att läsa av QR koden. Starta BankID-appen och läs av QR koden.  Om du inte har BankID-appen måste du installera den och hämta ett BankID hos din internetbank. Installera appen från din appbutik eller https://install.bankid.com."},
            {"RFA18", "Starta BankID-appen"},
            {"RFA19", "Vill du identifiera dig eller skriva under med BankID på den här datorn eller med ett Mobilt BankID?"},
            {"RFA20", "Vill du identifiera dig eller skriva under med ett BankID på den här enheten eller med ett BankID på en annan enhet?"},
            {"RFA21", "Identifiering eller underskrift pågår."},
            {"RFA22", "Okänt fel. Försök igen."},
            {"CUSTOM01", "Personnummret du angivit är inte korrekt"},
        };

        /// <summary>
        /// Get status message
        /// </summary>
        /// <param name="hintCode">Hint code from BankID API</param>
        /// <param name="rpAutoStart">If RP tried to start the client automatically or not</param>
        public string GetStatus(string hintCode, bool rpAutoStart = false)
        {
            switch (hintCode)
            {
                case "outstandingTransaction":
                    // The order is pending.The client has not yet received the order.
                    // The hintCode will later change to noClient, started or userSign.
                    return rpAutoStart ? codes["RFA13"] : codes["RFA1"];
                case "noClient":
                    // The order is pending. The client has not yet received the order. 
                    return codes["RFA1"];
                case "started":
                    // The order is pending. A client has been started with the autostarttoken
                    // but a usable ID has not yet been found in the started client. When the client
                    // starts there may be a short delay until all ID:s are registered. The user
                    // may not have any usable ID:s at all, or has not yet inserted their smart card
                    return rpAutoStart ? codes["RFA14B"] : codes["RFA15B"];
                case "userSign":
                    // The order is pending. The client has received the order.
                    return codes["RFA9"];
                case "expiredTransaction":
                    // The order has expired. The BankID security app / program did not start,
                    // the user did not finalize the signing or the RP called collect too late
                    return codes["RFA8"];
                case "certificateErr":
                    // This error is returned if:
                    // 1) The user has entered wrong security code too many times.The BankID cannot be used.
                    // 2) The users BankID is revoked.
                    // 3) The users BankID is invalid
                    return codes["RFA16"];
                case "userCancel":
                    // The user decided to cancel the order.
                    return codes["RFA6"];
                case "cancelled":
                    // The order was cancelled. The system received a new order for the user
                    return codes["RFA3"];
                case "startFailed":
                    // The user did not provide her ID, or the RP requires autoStartToken to be used,
                    // but the client did not start within a certain time limit. The reason may be:
                    // 1) RP did not use autoStartToken when starting BankID security program/ app.RP must correct this in their implementation.
                    // 2) The client software was not installed or other problem with the user’s computer.
                    return codes["RFA17"];
                default:
                    // To use the status code
                    return codes[hintCode];
            }

            return null;
        }

        public string GetError(HttpStatusCode httpStatus, IErrorResponse errorResponse = null)
        {
            // Todo! Log errorResponse.Details

            switch (httpStatus)
            {
                case HttpStatusCode.BadRequest:
                    // An auth or sign request with personal number was sent, but an order for the
                    // user is already in progress. The order is aborted. No order is created
                    // Either invalidParameters or alreadyInProgress 
                    return errorResponse != null && errorResponse.ErrorCode.Equals("invalidParameters") ? codes["RFA22"] : codes["RFA4"];
                case HttpStatusCode.Unauthorized:
                case HttpStatusCode.Forbidden:
                    // RP does not have access to the service. 
                    return codes["RFA22"];
                case HttpStatusCode.NotFound:
                    // An erroneously URL path was used.
                    return codes["RFA22"];
                case HttpStatusCode.RequestTimeout:
                    // It took too long time to transmit the request
                    return codes["RFA5"];
                case HttpStatusCode.UnsupportedMediaType:
                    // Adding a "charset" parameter after 'application/json' is not allowed since the MIME type
                    // "application/json" has neither optional nor required parameters. The type is missing or erroneously. 
                    return codes["RFA22"];
                case HttpStatusCode.InternalServerError:
                    // Internal technical error in the BankID system
                    return codes["RFA5"];
                case HttpStatusCode.ServiceUnavailable:
                    // The service is temporarily out of service
                    return codes["RFA5"];
            }

            return null;
        }
    }
}
