namespace Extensions {

    #region Usings
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Net.Mail;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks; 
    #endregion

    public static class MailMessageExtensions {

        /// <summary>
        /// Saves a mail message as an EML file
        /// </summary>
        /// <param name="mailMessage">The mail message</param>
        /// <param name="fileName">The file</param>
        [DebuggerStepThroughAttribute]
        public static void Save(this MailMessage mailMessage, string fileName) {
            Assembly assembly = typeof(SmtpClient).Assembly;
            Type mailWriterType = assembly.GetType("System.Net.Mail.MailWriter");
            BindingFlags bindingFlags = (BindingFlags.Instance | BindingFlags.NonPublic);

            using (var fileStream = new FileStream(fileName, FileMode.Create)) {
                // Get reflection info for MailWriter contructor
                ConstructorInfo mailWriterContructor =
                    mailWriterType.GetConstructor(
                        bindingFlags,
                        null,
                        new Type[] { typeof(Stream) },
                        null);

                // Construct MailWriter object with our FileStream
                object mailWriter = mailWriterContructor.Invoke(new object[] { fileStream });

                object[] sendMethodParameters = new object[] { mailWriter, true };
                // for .NET 4.5 signature: internal void Send(BaseWriter writer, bool sendEnvelope, bool allowUnicode)
                object[] newSendMethodParameters = new object[] { mailWriter, true, true };
                object[] closeMethodParameters = new object[] { };

                // Get reflection info for Send() method on MailMessage
                MethodInfo sendMethod =
                    typeof(MailMessage).GetMethod(
                        "Send",
                        bindingFlags);

                // Call method passing in MailWriter
                try {
                    sendMethod.Invoke(
                        mailMessage,
                        bindingFlags,
                        null,
                        sendMethodParameters,
                        CultureInfo.CurrentCulture);
                }
                catch (TargetParameterCountException) {
                    sendMethod.Invoke(
                        mailMessage,
                        bindingFlags,
                        null,
                        newSendMethodParameters,
                        CultureInfo.CurrentCulture);
                }

                // Finally get reflection info for Close() method on our MailWriter
                MethodInfo closeMethod =
                    mailWriter.GetType().GetMethod(
                        "Close",
                        bindingFlags);

                // Call close method
                closeMethod.Invoke(
                    mailWriter,
                    bindingFlags,
                    null,
                    closeMethodParameters,
                    CultureInfo.CurrentCulture);
            }
        }
    }
}
