namespace Extensions.Tests {

    #region Usings
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net.Mail;
    using System.Net.Mime;
    using System.Text;
    using System.Threading.Tasks; 
    #endregion

    [TestClass]
    public class MailMessageExtensionsUnitTests {

        [TestMethod]
        public void Test_MailMessageSave() {

            var uniqueMessageId = Guid.NewGuid();
            var fromAddress = "fakeFrom@contoso.com";
            var senderAddress = "fakeSender@contoso.com";
            var recipientAddress = "fakeRecipient@contoso.com";
            var bccRecipientAddress = "fakeBccRecipient@contoso.com";
            var bccRecipientAddress2 = "fakeBccRecipient2@contoso.com";
            var subject = "Fake message subject";
            var messageContent = $"Fake message content. {Guid.NewGuid().ToString()}";
            var messageContentBase64 = Encoding.UTF8.GetBytes(messageContent).ToBase64String();
            var attachmentContents = $"Fake attachment contents. ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890. {Guid.NewGuid().ToString()}";
            var attachmentContentsBase64 = Encoding.UTF8.GetBytes(attachmentContents).ToBase64String();

            var mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(fromAddress);
            mailMessage.Sender = new MailAddress(senderAddress);
            mailMessage.To.Add(new MailAddress(recipientAddress));
            mailMessage.Bcc.Add(bccRecipientAddress);
            mailMessage.Bcc.Add(bccRecipientAddress2);

            mailMessage.Headers["X-Unique-Id"] = uniqueMessageId.ToString();
            mailMessage.Body = messageContent;
            mailMessage.Priority = MailPriority.High;
            mailMessage.Subject = subject;
            mailMessage.IsBodyHtml = true;
            mailMessage.BodyEncoding = Encoding.UTF8;

            var dataDirectoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
            if (!Directory.Exists(dataDirectoryPath)) {
                Directory.CreateDirectory(dataDirectoryPath);
            }

            var attachmentFileName = $"FakeAttachment-{uniqueMessageId}.txt";
            var attachmentPath = Path.Combine(dataDirectoryPath, attachmentFileName);
            if (File.Exists(attachmentPath)) {
                File.Delete(attachmentPath);
            }
            File.WriteAllText(attachmentPath, attachmentContents);

            if (!string.IsNullOrWhiteSpace(attachmentPath) && File.Exists(attachmentPath)) {
                // Create  the file attachment for this e-mail message.
                var attachment = new Attachment(attachmentPath, MediaTypeNames.Text.Plain);
                // Add time stamp information for the file.
                ContentDisposition disposition = attachment.ContentDisposition;
                disposition.CreationDate = File.GetCreationTime(attachmentPath);
                disposition.ModificationDate = File.GetLastWriteTime(attachmentPath);
                disposition.ReadDate = File.GetLastAccessTime(attachmentPath);
                // Add the file attachment to this e-mail message.
                mailMessage.Attachments.Add(attachment);
            }

            // save a copy of the message for auditing
            var fileName = $"{mailMessage.To.First().Address}-{Guid.NewGuid().ToString()}.eml";
            Path.GetInvalidFileNameChars().ToList().ForEach(x => fileName = fileName.Replace(x, '_'));
            string savedMessageFilePath = Path.Combine(dataDirectoryPath, fileName);

            mailMessage.Save(savedMessageFilePath);

            Assert.IsTrue(File.Exists(savedMessageFilePath));
            var fileInfo = new FileInfo(savedMessageFilePath);

            Assert.IsTrue(fileInfo.Length > 0);

            var fileLines = File.ReadAllLines(savedMessageFilePath);
            Assert.IsTrue(fileLines.Length > 0);
            Assert.IsTrue(fileLines.Any(x => x.StartsWith($"X-Unique-Id: {uniqueMessageId}")));
            Assert.IsTrue(fileLines.Any(x => x.StartsWith($"X-Sender: {senderAddress}")));
            Assert.IsTrue(fileLines.Any(x => x.StartsWith($"X-Receiver: {recipientAddress}")));
            Assert.IsTrue(fileLines.Any(x => x.StartsWith($"X-Receiver: {bccRecipientAddress}")));
            Assert.IsTrue(fileLines.Any(x => x.StartsWith($"X-Receiver: {bccRecipientAddress2}")));
            Assert.IsTrue(fileLines.Any(x => x.StartsWith($"Sender: {senderAddress}")));
            Assert.IsTrue(fileLines.Any(x => x.StartsWith($"From: {fromAddress}")));
            Assert.IsTrue(fileLines.Any(x => x.StartsWith($"To: {recipientAddress}")));
            Assert.IsTrue(fileLines.Any(x => x.StartsWith($"Subject: {subject}")));
            Assert.IsTrue(fileLines.Any(x => x.Contains($"name={attachmentFileName}")));

            var fileText = File.ReadAllText(savedMessageFilePath);
            var fileTextNoLineBreaks = fileText.Replace(Environment.NewLine, string.Empty);
            Assert.IsTrue(fileTextNoLineBreaks.Contains(messageContentBase64));
            Assert.IsTrue(fileTextNoLineBreaks.Contains(attachmentContentsBase64));
        }
    }
}
