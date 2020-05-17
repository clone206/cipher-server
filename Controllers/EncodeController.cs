using System;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Web.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace cipher_server.Controllers
{
    /// <summary>
    /// Handles incoming encoding requests
    /// </summary>
    [ApiController]
    [Route ("api/[controller]")]
    public class EncodeController : ControllerBase
    {
        private readonly string _wwwRootPath;

        public EncodeController (IWebHostEnvironment env)
        {
            if (env.EnvironmentName == "Development")
            {
                this._wwwRootPath = env.ContentRootPath + "/wwwroot";
            }
            else
            {
                this._wwwRootPath = Path.GetDirectoryName (Assembly.GetEntryAssembly ().Location) + "/wwwroot";
            }
        }

        /// <summary>
        /// Encodes an incoming message string
        /// according to a passed-in shift value
        /// </summary>
        /// <param name="req">Message request consisting of a string and a shift value</param>
        /// <returns>Simple JSON object containing encoded result</returns>
        [HttpPost]
        [Produces ("application/json")]
        public ActionResult Post ([FromBody] MessageRequest req)
        {
            MessageResult res = new MessageResult ();
            try
            {
                res.EncodedMessage = ShiftChars (req.Message, req.Shift);
                string jsonString = JsonSerializer.Serialize<MessageResult> (res);
                string fileName = this._wwwRootPath + "/encoded.json";
                System.IO.File.WriteAllText (fileName, jsonString);
            }
            catch (Exception ex)
            {
                // Return empty Encoded
                // message with 500 error
                var er = new ExceptionResult (ex, false);
                res.EncodedMessage = "";
                er.Value = res;
                return er;
            }
            return Ok (res);
        }

        /// <summary>
        /// Performs encoding char by char and returns
        /// encoded string
        /// </summary>
        /// <param name="val">String to be encoded</param>
        /// <param name="shift">Shift value for encoding</param>
        /// <returns>Encoded string</returns>
        static string ShiftChars (string val, int shift)
        {
            // Can't shift more than 26 in either direction
            if (Math.Abs (shift) > 26)
            {
                throw new Exception ("Shift must have an absolute value of less than or equal to 26");
            }
            // Pass through the string if we're not effectively shifting
            if (shift == 0 || Math.Abs (shift) == 26)
            {
                return val;
            }

            // Buffer for shifting char by char
            char[] buff = val.ToCharArray ();

            for (int i = 0; i < buff.Length; ++i)
            {
                // This cipher will only shift [A-Za-z]
                if (buff[i] < 'A' || buff[i] > 'z' || (buff[i] > 'Z' && buff[i] < 'a'))
                {
                    continue;
                }

                char letter = (char) (buff[i] + shift);

                // Overflow
                if (letter > 'z' || (buff[i] <= 'Z' && letter > 'Z'))
                {
                    letter = (char) (letter - 26);
                }
                // Underflow
                else if ((buff[i] >= 'a' && letter < 'a') || letter < 'A')
                {
                    letter = (char) (letter + 26);
                }

                buff[i] = letter;
            }

            return new string (buff);
        }
    }
}